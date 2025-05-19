using Database.Core.Domain;
using Database.Core.Repositories;
using Database.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace Database.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for managing RentalRecord entities.
    /// </summary>
    internal class RentalRecordRepository : Repository<RentalRecord>, IRentalRecordRepository
    {
        #region Constructor

        public RentalRecordRepository(RentalDBContext context, int userId)
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

        #region IRentalRecordRepository Implementation (IRentalRecordRepository)

        /// <inheritdoc/>
        public RentalRecord GetWithDetails(int id)
        {
            return GetWithDetailsQuery().FirstOrDefault(r => r.Id == id);
        }

        /// <inheritdoc/>
        public RentalRecord GetWithDetailsByRentalRequest(int id)
        {
            return GetWithDetailsQuery().FirstOrDefault(r => r.RentalRequestId == id);
        }

        /// <inheritdoc/>
        public async Task<List<QuarterEarnings>> GetQuarterEarningsByYearAsync(int year)
        {
            var records = await RentalDBContext.RentalRecords
                .Where(r => r.ActualReturnDate.HasValue && r.ActualReturnDate.Value.Year == year)
                .ToListAsync();

            // Group totals by month
            var earningsByMonth = records
                .GroupBy(r => r.ActualReturnDate.Value.Month)
                .ToDictionary(g => g.Key, g => g.Sum(r => (int)r.TotalCost + r.LateReturnFees ?? 0));

            var result = new List<QuarterEarnings>();

            // Construct quarterly breakdown
            for (int q = 1; q <= 4; q++)
            {
                // Create a new QuarterEarnings instance for the current quarter
                var quarterData = new QuarterEarnings { Quarter = q };

                // Each quarter has 3 months: iterate through them
                for (int i = 0; i < 3; i++)
                {
                    // Calculate the actual month number (1-based index)
                    int month = (q - 1) * 3 + i + 1;

                    // Convert the month number into its abbreviated label (e.g., "Jan", "Feb")
                    string label = new DateTime(year, month, 1).ToString("MMM");

                    // Get the total earnings for this month, or 0 if not present
                    decimal value = earningsByMonth.GetValueOrDefault(month, 0);

                    // Add this month's earnings to the quarter data
                    quarterData.Data.Add(new MonthlyEarnings { Month = label, Value = value });
                }

                // Add the completed quarter data to the result list
                result.Add(quarterData);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, int>> GetWeeklyCategoryRentalDataAsync(int categoryLimit)
        {
            // Query rental records that are linked to valid equipment with a category
            var rawData = await RentalDBContext.RentalRecords
                .Where(rr =>
                    rr.RentalRequest != null &&
                    rr.RentalRequest.Equipment != null &&
                    rr.RentalRequest.Equipment.Category != null)

                // Group records by category name
                .GroupBy(rr => rr.RentalRequest.Equipment.Category.Name)

                // Project each group to a label and count pair
                .Select(g => new { Label = g.Key, Value = g.Count() })

                // Order categories by descending rental count
                .OrderByDescending(g => g.Value)

                // Execute the query asynchronously and get the list
                .ToListAsync();

            // Take the top (categoryLimit - 1) most rented categories
            var top = rawData.Take(categoryLimit - 1).ToList();

            // Gather the remaining categories as "Others"
            var others = rawData.Skip(categoryLimit - 1).ToList();

            // If there are remaining categories, summarize them into an "Others" entry
            if (others.Any())
            {
                top.Add(new { Label = "Others", Value = others.Sum(x => x.Value) });
            }

            // Convert the result into a dictionary mapping category name to rental count
            return top.ToDictionary(x => x.Label, x => x.Value);
        }

        #endregion

        #region Internal Helpers

        private IQueryable<RentalRecord> GetWithDetailsQuery()
        {
            return RentalDBContext.RentalRecords
                .Include(r => r.RentalRequest)
                .Include(r => r.Payments)
                .Include(r => r.ReturnCondition);
        }

        private IQueryable<RentalRecord> GetWithFeedbackDetailsQuery()
        {
            return GetWithDetailsQuery().Include(r => r.Feedbacks);
        }

        #endregion

        #region Metadata Overrides

        /// <inheritdoc/>
        public override Dictionary<string, string> GetEntityColumnsWithTypes()
        {
            var d = base.GetEntityColumnsWithTypes();
            d.Remove("ExtraChargeDescription");
            d["RentalRequestId"] = "Int32";
            return d;
        }

        /// <inheritdoc/>
        public override IQueryable<object> SelectViewColumns(IQueryable query)
        {
            return query.Cast<RentalRecord>().Select(r => new
            {
                Id = r.Id,
                RequestId = r.RentalRequestId,
                EquipmentName = r.EquipmentName,
                PickupDate = r.PickupDate.Date,
                ActualReturnDate = r.ActualReturnDate,
                ReturnCondition = r.ReturnCondition != null ? r.ReturnCondition.ConditionName : "",
                TotalCost = r.TotalCost,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            }).Cast<object>();
        }

        /// <inheritdoc/>
        protected override bool ShouldIgnoreProperty(string propertyName)
        {
            return propertyName switch
            {
                nameof(RentalRecord.Feedbacks) => true,
                nameof(RentalRecord.Payments) => true,
                nameof(RentalRecord.RentalRequest) => true,
                nameof(RentalRecord.ReturnCondition) => true,
                nameof(RentalRecord.UpdatedAt) => true,
                nameof(RentalRecord.CreatedAt) => true,
                _ => base.ShouldIgnoreProperty(propertyName)
            };
        }

        #endregion

        #region INotifiable Implementation
        /// <inheritdoc/>

        public IEnumerable<Notification> GetPendingNotifications()
        {
            // Retrieve all RentalRecord entities that were modified in the current DbContext tracking session
            var entries = context.ChangeTracker.Entries<RentalRecord>()
                .Where(e => e.State == EntityState.Modified);

            // Iterate through all modified RentalRecord entries tracked by the context
            foreach (var entry in entries)
            {
                // Get the previous (original) and current values of the ActualReturnDate property
                var originalReturnDate = entry.Property("ActualReturnDate").OriginalValue as DateTime?;
                var currentReturnDate = entry.Property("ActualReturnDate").CurrentValue as DateTime?;

                // Trigger notification only if the return date was previously null and now set (i.e., return confirmed)
                if (originalReturnDate == null && currentReturnDate != null)
                {
                    // Retrieve the RentalRequestId from the modified RentalRecord
                    var rentalRequestId = entry.Property("RentalRequestId").CurrentValue as int?;

                    // Fetch the corresponding RentalRequest from the database to get the user ID
                    var rentalRequest = context.RentalRequests.Find(rentalRequestId);
                    var userId = rentalRequest.CustomerId;

                    // If a valid user ID was found, yield a new notification
                    if (userId != null)
                    {
                        yield return new Notification
                        {
                            UserId = userId,
                            NotificationTypeId = 4, // Hardcoded type ID for "Return confirmed"
                            MessageContent = $"Return for rental #{rentalRequestId} has been confirmed.",
                            IsRead = false,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                    }
                }
            }
        }

        #endregion
    }
}
