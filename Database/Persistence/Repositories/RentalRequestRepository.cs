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

        public RentalRequestRepository(RentalDBContext context, int userId)
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
        /// <summary>
        /// Computes weekly dashboard statistics for rental activity, including pickups, completions, overdue, and damages.
        /// </summary>
        /// <returns>A populated WeeklyStats object for the current week.</returns>
        public async Task<WeeklyStats> GetWeeklyDashboardStatsAsync()
        {
            DateTime today = DateTime.Today;

            // Get the start (Monday) and end (next Monday) of this week
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime weekStart = today.AddDays(-diff);
            DateTime weekEnd = weekStart.AddDays(7);

            // Load rental requests from this week, including their record and return condition
            var weeklyRequests = await RentalDBContext.RentalRequests
                .Include(r => r.RentalRecord)
                   .ThenInclude(rr => rr.ReturnCondition)
                .Where(r => r.StartDate >= weekStart && r.StartDate < weekEnd)
                .ToListAsync();

            // Extract valid rental records (one-to-one relationship)
            var weeklyRecords = weeklyRequests
                .Where(r => r.RentalRecord != null)
                .Select(r => r.RentalRecord!)
                .ToList();

            return new WeeklyStats
            {
                // Rentals scheduled for pickup today
                TodaysPickups = weeklyRecords.Count(r => r.PickupDate.Date == today),

                // Total records created this week
                TotalRentals = weeklyRecords.Count,

                // Records without return date
                OngoingRentals = weeklyRecords.Count(r => r.ActualReturnDate == null),

                // Records with completed return
                CompletedRentals = weeklyRecords.Count(r => r.ActualReturnDate != null),

                // Requests that are overdue and not yet returned
                OverdueRentals = weeklyRequests.Count(r =>
                    r.RentalRecord != null &&
                    r.RentalRecord.ActualReturnDate == null &&
                    r.ReturnDate < today),

                // Records marked with a return condition that includes "damaged"
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
                .Include(r => r.RentalRecord);
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
                nameof(RentalRequest.RentalRecord) => true,
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
                        UserId = customerId.Value,
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
