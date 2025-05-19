using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.Core.Repositories;
using Database.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Database.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for managing <see cref="Equipment"/> entities.
    /// Includes image lookup, detailed navigation includes, and usage analytics.
    /// </summary>
    internal class EquipmentRepository : Repository<Equipment>, IEquipmentRepository
    {
        #region Constructor

        public EquipmentRepository(RentalDBContext context, int userId)
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

        #region IEquipmentRepository Implementation

        /// <inheritdoc/>
        public bool IsReferenced(int id)
        {
            return RentalDBContext.RentalRequests.Any(r => r.EquipmentId == id);
        }

        /// <inheritdoc/>
        public Equipment? GetEquipmentWithImage(int id)
        {
            return GetWithDetails()
                .FirstOrDefault(e => e.Id == id);
        }

        /// <inheritdoc/>
        public async Task<List<TopRentedEquipmentStats>> GetTop5RentedEquipmentStatsAsync()
        {
            // Step 1: Identify top 5 equipment IDs based on number of rental records
            var top5EquipmentIds = await context.RentalRecords
                .GroupBy(r => r.RentalRequest.EquipmentId)          // Group records by equipment ID through rental request
                .OrderByDescending(g => g.Count())                  // Order by rental count descending
                .Take(5)                                             // Take the top 5
                .Select(g => g.Key)                                 // Select the equipment ID
                .ToListAsync();

            // Step 2: Query full equipment records and project relevant statistics
            return await context.Equipment
                .Where(e => top5EquipmentIds.Contains(e.Id))        // Filter only the top 5 equipment
                .Select(e => new TopRentedEquipmentStats
                {
                    EquipmentId = e.Id,                             // Equipment ID
                    Name = e.Name,                                  // Equipment name

                    // Count of rental requests (each may correspond to one rental)
                    TotalRentals = e.RentalRequests.Count(),

                    // Total revenue from all completed rentals (with RentalRecord)
                    TotalRevenue = e.RentalRequests
                        .Where(rq => rq.RentalRecord != null)
                        .Sum(rq => rq.RentalRecord!.TotalCost),

                    // Average rental duration in days, only for valid records with pickup and return dates
                    AvgRentalDuration = e.RentalRequests
                        .Any(rq => rq.RentalRecord != null &&
                                   rq.RentalRecord.ActualReturnDate != null)
                        ? (float)(e.RentalRequests
                            .Where(rq => rq.RentalRecord != null &&
                                         rq.RentalRecord.ActualReturnDate != null)
                            .Average(rq => (int?)EF.Functions.DateDiffDay(
                                rq.RentalRecord!.PickupDate,
                                rq.RentalRecord.ActualReturnDate)) ?? 0)
                        : 0f,

                    // Average rating from visible feedback (non-hidden)
                    Rating = e.Feedbacks
                        .Any(f => !f.IsHidden.HasValue || !f.IsHidden.Value)
                        ? (float)(e.Feedbacks
                            .Where(f => !f.IsHidden.HasValue || !f.IsHidden.Value)
                            .Average(f => (decimal?)f.Rate) ?? 0)
                        : 0f,

                    // Optional image info
                    ImageGuid = e.Image != null ? e.Image.Guid : null,
                    ImageFormat = e.Image != null ? e.Image.ImageType : null
                })
                .ToListAsync();  // Execute the query asynchronously
        }

        /// <inheritdoc/>
        public bool IsInUse(int id)
        {
            return RentalDBContext.RentalRecords
                   .Any(r =>
                       r.RentalRequest.EquipmentId == id &&
                       r.ActualReturnDate == null);
        }

        #endregion

        #region IStatus Implementation (IEquipmentRepository)

        /// <inheritdoc/>
        public Dictionary<int, string> GetAllByName()
        {
            return context.Equipment.ToDictionary(e => e.Id, e => e.Name);
        }

        #endregion

        #region Internal Helpers

        /// <summary>
        /// Returns an IQueryable that includes navigation properties for Equipment.
        /// </summary>
        private IQueryable<Equipment> GetWithDetails()
        {
            return RentalDBContext.Equipment
                .Include(e => e.AvailabilityStatus)
                .Include(e => e.Category)
                .Include(e => e.ConditionStatus)
                .Include(e => e.Image);
        }

        #endregion

        #region Metadata Overrides

        /// <inheritdoc/>
        public override Dictionary<string, string> GetEntityColumnsWithTypes()
        {
            var d = base.GetEntityColumnsWithTypes();
            d.Remove("Image"); // Exclude navigation property
            return d;
        }

        /// <inheritdoc/>
        protected override bool ShouldIgnoreProperty(string propertyName)
        {
            return propertyName switch
            {
                nameof(Equipment.CreatedAt) => true,
                nameof(Equipment.UpdatedAt) => true,
                nameof(Equipment.AvailabilityStatus) => true,
                nameof(Equipment.Category) => true,
                nameof(Equipment.ConditionStatus) => true,
                nameof(Equipment.Feedbacks) => true,
                nameof(Equipment.Image) => true,
                nameof(Equipment.RentalRequests) => true,
                _ => base.ShouldIgnoreProperty(propertyName)
            };
        }

        #endregion

        #region View Projection

        /// <inheritdoc/>
        public override IQueryable<object> SelectViewColumns(IQueryable query)
        {
            query = query.Cast<Equipment>().Select(e => new
            {
                Id = e.Id,
                Name = e.Name,
                Price = e.RentalPricePerDay,
                Availability = e.AvailabilityStatus != null ? e.AvailabilityStatus.StatusName : "",
                Condition = e.ConditionStatus != null ? e.ConditionStatus.ConditionName : "",
                Category = e.Category != null ? e.Category.Name : "",
                IsActive = e.IsActive ?? true,
            });

            return query.Cast<object>();
        }

        #endregion
    }
}
