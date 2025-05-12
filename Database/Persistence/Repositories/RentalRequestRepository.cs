using Database.Core;
using Database.Core.Domain;
using Database.Core.Repositories;
using Database.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Database.Persistence.Repositories
{
    internal class RentalRequestRepository : Repository<RentalRequest>, IRentalRequestRepository
    {
        private static readonly List<(int requestStatusId, string messageTemplate, int notificationTypeId)> StatusNotifications = new()
        {
            (2, "Rental request #{0} has been approved.", 1) ,  // 1 = Approved
            (3, "Rental request #{0} has been rejected.", 2) ,  // 3 = Rejected
            (4, "Rental request #{0} has been canceled.", 5) ,  // 4 = Cancelled
        };
        public RentalRequestRepository(RentalDBContext context, int? userId) : base(context, userId)
        {
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }

        private IQueryable<RentalRequest> GetWithDetails()
        {
            return RentalDBContext.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .Include(r => r.Status)
                .AsQueryable();
        }

        private IQueryable<RentalRequest> GetWithRecordDetails()
        {
            return GetWithDetails()
                .Include(r => r.RentalRecords)
                .AsQueryable();
        }

        public RentalRequest GetWithRecordDetails(int id)
        {
            return GetWithRecordDetails().Where(r => r.Id == id).FirstOrDefault();
        }

        #region View Models

        public async Task<WeeklyStats> GetWeeklyDashboardStatsAsync()
        {
            DateTime today = DateTime.Today;
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime weekStart = today.AddDays(-diff);
            DateTime weekEnd = weekStart.AddDays(7);

            var weeklyRequests = await RentalDBContext.RentalRequests
                .Include(r => r.RentalRecords)
                .ThenInclude(rr => rr.ReturnCondition)
                .Where(r => r.StartDate >= weekStart && r.StartDate < weekEnd)
                .ToListAsync();

            var weeklyRecords = weeklyRequests.SelectMany(r => r.RentalRecords).ToList();

            return new WeeklyStats
            {
                TodaysPickups = weeklyRecords.Count(r => r.PickupDate.Date == today),
                TotalRentals = weeklyRecords.Count,
                OngoingRentals = weeklyRecords.Count(r => r.ActualReturnDate == null),
                CompletedRentals = weeklyRecords.Count(r => r.ActualReturnDate != null),
                OverdueRentals = weeklyRequests
                    .Where(r => r.RentalRecords.Any(rr => rr.ActualReturnDate == null))
                    .Count(r => r.ReturnDate < today),
                DamagedReturns = weeklyRecords.Count(r =>
                    r.ReturnCondition != null &&
                    r.ReturnCondition.ConditionName.ToLower().Contains("damaged"))
            };
        }


        #endregion

        #region Search

        public override Dictionary<string, string> GetEntityColumnsWithTypes()
        {
            var d = base.GetEntityColumnsWithTypes();
            if (d.ContainsKey("EquipmentId")) d["EquipmentId"] = "Int32";
            if (d.ContainsKey("CustomerId")) d["CustomerId"] = "Int32";
            return d;
        }

        public override IQueryable<object> SelectViewColumns(IQueryable query)
        {
            query = query.Cast<RentalRequest>().Select(rr => new
            {
                Id = rr.Id,
                Equipment = rr.Equipment != null ? rr.Equipment.Id + " - " + rr.Equipment.Name : "",
                Customer = rr.Customer != null
                    ? rr.Customer.Id + " - " + rr.Customer.FirstName + " " + rr.Customer.LastName
                    : "",
                StartDate = rr.StartDate,
                EndDate = rr.ReturnDate,
                Status = rr.Status != null ? rr.Status.StatusName : ""
            });

            return query.Cast<object>();
        }

        #endregion

        #region Audit Trails

        public (bool needsLog, int id, string dataBeforeAction, string dataAfterAction) GenerateLogDetails(
            RentalRequest item)
        {
            Console.WriteLine("here1");
            var entry = RentalDBContext.Entry(item);
            Console.WriteLine("here1");
            if (entry.State != EntityState.Modified)
                return (false, item.Id, "", "");

            Console.WriteLine("state is modified");
            var originalValues = new Dictionary<string, object>();
            var currentValues = new Dictionary<string, object>();

            foreach (var prop in entry.Properties)
            {
                var original = prop.OriginalValue;
                var current = prop.CurrentValue;

                if (!Equals(original, current))
                {
                    originalValues[prop.Metadata.Name] = original ?? "null";
                    currentValues[prop.Metadata.Name] = current ?? "null";
                }
            }

            bool hasChanged = originalValues.Count > 0;

            if (hasChanged) item.UpdatedAt = DateTime.Now;

            return (
                hasChanged,
                item.Id,
                JsonSerializer.Serialize(originalValues),
                JsonSerializer.Serialize(currentValues)
            );
        }


        #endregion

        protected override bool ShouldIgnoreProperty(string propertyName)
        {
            return propertyName switch
            {
                nameof(RentalRequest.CreatedAt) => true,
                nameof(RentalRequest.UpdatedAt) => true,
                nameof(RentalRequest.RentalRecords) => true,
                nameof(RentalRequest.Status) => true,
                _ => base.ShouldIgnoreProperty(propertyName)
            };
        }

        public IEnumerable<Notification> GetPendingNotifications()
        {
            var entries = context.ChangeTracker.Entries<RentalRequest>()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var currentStatusId = entry.Property("StatusId").CurrentValue as int?;
                var originalStatusId = entry.Property("StatusId").OriginalValue as int?;
                var customerId = entry.Property("CustomerId").CurrentValue as int?;
                var requestId = entry.Property("Id").CurrentValue?.ToString() ?? "?";

                // Find matching tuple in list
                var config = StatusNotifications.FirstOrDefault(n => n.requestStatusId == currentStatusId);

                // Only proceed if status changed and match found
                if (currentStatusId != originalStatusId && config != default)
                {
                    var (statusId, template, typeId) = config;

                    yield return new Notification
                    {
                        UserId = customerId,
                        MessageContent = string.Format(template, requestId),
                        NotificationTypeId = typeId,
                        IsRead = false,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                }
            }
        }
    }
}