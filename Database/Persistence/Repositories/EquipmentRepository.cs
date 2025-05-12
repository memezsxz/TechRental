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

        public EquipmentRepository(RentalDBContext context, int? userId)
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
            // Get the top 5 equipment IDs based on rental count
            var top5EquipmentIds = await context.RentalRecords
                .Where(r => r.RentalRequest != null && r.RentalRequest.EquipmentId != null)
                .GroupBy(r => r.RentalRequest.EquipmentId.Value)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => g.Key)
                .ToListAsync();

            // Query the full equipment records and project statistics
            return await context.Equipment
         // Only include equipment that is in the top 5 most rented
         .Where(e => top5EquipmentIds.Contains(e.Id))

         // Project the selected equipment into TopRentedEquipmentStats view model
         .Select(e => new TopRentedEquipmentStats
         {
             // Equipment ID
             EquipmentId = e.Id,

             // Equipment name
             Name = e.Name,

             // Count of rental records associated with this equipment
             TotalRentals = e.RentalRequests
                 .SelectMany(rq => rq.RentalRecords) // flatten requests to records
                 .Count(),

             // Sum of all total costs from associated rental records
             TotalRevenue = e.RentalRequests
                 .SelectMany(rq => rq.RentalRecords)
                 .Sum(rr => (decimal?)rr.TotalCost) ?? 0, // fallback to 0 if null

             // Average number of rental days for records that have both pickup and return dates
             AvgRentalDuration = e.RentalRequests
                 .SelectMany(rq => rq.RentalRecords)
                 .Any(rr => rr.PickupDate != null && rr.ActualReturnDate != null)
                 ? (float)(
                     e.RentalRequests
                         .SelectMany(rq => rq.RentalRecords)
                         .Where(rr => rr.PickupDate != null && rr.ActualReturnDate != null)
                         .Average(rr => (int?)EF.Functions.DateDiffDay(rr.PickupDate, rr.ActualReturnDate.Value)) ?? 0)
                 : 0f,

             // Average rating from visible (non-hidden) feedback records
             Rating = e.Feedbacks
                 .Any(f => !f.IsHidden.HasValue || !f.IsHidden.Value) // check if any visible feedback exists
                 ? (float)(
                     e.Feedbacks
                         .Where(f => !f.IsHidden.HasValue || !f.IsHidden.Value)
                         .Average(f => (decimal?)f.Rate) ?? 0)
                 : 0f,

             // Image metadata: GUID (nullable) and MIME type
             ImageGuid = e.Image.Guid ?? null,
             ImageFormat = e.Image.ImageType
         })

         // Execute the query asynchronously and return the results as a list
         .ToListAsync();

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
