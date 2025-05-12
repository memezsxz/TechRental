using Database.Core;
using Database.Core.Domain;
using Database.Core.Repositories;
using Database.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Database.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for managing RentalRequest entities.
    /// </summary>
    internal class RentalRequestRepository : Repository<RentalRequest>, IRentalRequestRepository
    {
        #region Notification Configuration

        private static readonly List<(int requestStatusId, string messageTemplate, int notificationTypeId)> StatusNotifications = new()
        {
            (2, "Rental request #{0} has been approved.", 1),
            (3, "Rental request #{0} has been rejected.", 2),
            (4, "Rental request #{0} has been canceled.", 5),
        };

        #endregion

        #region Constructor

        public RentalRequestRepository(RentalDBContext context, int? userId)
            : base(context, userId)
        {
        }

        #endregion

        #region Context Accessor

        /// <summary>
        /// Gets the current database context cast to <see cref="RentalDBContext"/>.
        /// </summary>
        public RentalDBContext RentalDBContext => context as RentalDBContext;

        #endregion

        #region IRentalRequestRepository Implementation

        /// <inheritdoc/>
        public RentalRequest GetWithRecordDetails(int id)
        {
            return GetWithRecordDetailsQuery().FirstOrDefault(r => r.Id == id);
        }

        /// <inheritdoc/>
        public async Task<WeeklyStats> GetWeeklyDashboardStatsAsync()
        {
            DateTime today = DateTime.Today;

            // Calculate current week's Monday and Sunday
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime weekStart = today.AddDays(-diff);
            DateTime weekEnd = weekStart.AddDays(7);

            // Load all requests within the current week
            var weeklyRequests = await RentalDBContext.RentalRequests
                .Include(r => r.RentalRecords)
                .ThenInclude(rr => rr.ReturnCondition)
                .Where(r => r.StartDate >= weekStart && r.StartDate < weekEnd)
                .ToListAsync();

            // Flatten to all rental records
            var weeklyRecords = weeklyRequests.SelectMany(r => r.RentalRecords).ToList();

            return new WeeklyStats
            {
                TodaysPickups = weeklyRecords.Count(r => r.PickupDate.Date == today),
                TotalRentals = weeklyRecords.Count,
                OngoingRentals = weeklyRecords.Count(r => r.ActualReturnDate == null),
                CompletedRentals = weeklyRecords.Count(r => r.ActualReturnDate != null),
                OverdueRentals = weeklyRequests.Count(r =>
                    r.RentalRecords.Any(rr => rr.ActualReturnDate == null) &&
                    r.ReturnDate < today),
                DamagedReturns = weeklyRecords.Count(r =>
                    r.ReturnCondition != null &&
                    r.ReturnCondition.ConditionName.ToLower().Contains("damaged"))
            };
        }

        #endregion

        #region Internal Helpers

        private IQueryable<RentalRequest> GetWithDetailsQuery()
        {
            return RentalDBContext.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .Include(r => r.Status);
        }

        private IQueryable<RentalRequest> GetWithRecordDetailsQuery()
        {
            return GetWithDetailsQuery()
                .Include(r => r.RentalRecords);
        }

        #endregion

        #region Metadata Overrides

        /// <inheritdoc/>
        public override Dictionary<string, string> GetEntityColumnsWithTypes()
        {
            var d = base.GetEntityColumnsWithTypes();
            if (d.ContainsKey("EquipmentId")) d["EquipmentId"] = "Int32";
            if (d.ContainsKey("CustomerId")) d["CustomerId"] = "Int32";
            return d;
        }

        /// <inheritdoc/>
        public override IQueryable<object> SelectViewColumns(IQueryable query)
        {
            return query.Cast<RentalRequest>().Select(rr => new
            {
                Id = rr.Id,
                Equipment = rr.Equipment != null ? rr.Equipment.Id + " - " + rr.Equipment.Name : "",
                Customer = rr.Customer != null
                    ? rr.Customer.Id + " - " + rr.Customer.FirstName + " " + rr.Customer.LastName
                    : "",
                StartDate = rr.StartDate,
                EndDate = rr.ReturnDate,
                Status = rr.Status != null ? rr.Status.StatusName : ""
            }).Cast<object>();
        }

        /// <inheritdoc/>
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

        #endregion

        #region INotifiable Implementation
        /// <inheritdoc/>
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

                var config = StatusNotifications.FirstOrDefault(n => n.requestStatusId == currentStatusId);

                // Only notify if status changed and matched one of the configured notifications
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

        #endregion
    }
}
