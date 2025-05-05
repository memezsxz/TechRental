using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.Core.Repositories;
using Database.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Database.Persistence.Repositories
{
    internal class EquipmentRepository : Repository<Equipment>, IEquipmentRepository
    {
        public EquipmentRepository(RentalDBContext context) : base(context)
        {
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }


        #region Async

        public async Task<IEnumerable<Equipment>> GetAllWithDetailsAsync()
        {
            return await RentalDBContext.Equipment
                .Include(e => e.AvailabilityStatus)
                .Include(e => e.Category)
                .Include(e => e.ConditionStatus)
                .Include(e => e.Image)
                .ToListAsync();
        }

        public async Task<Equipment?> GetByIdWithDetailsAsync(int id)
        {
            return await RentalDBContext.Equipment
                .Include(e => e.AvailabilityStatus)
                .Include(e => e.Category)
                .Include(e => e.ConditionStatus)
                .Include(e => e.Image)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Equipment?> GetEquipmentWithImageAsync(int id)
        {
            return await RentalDBContext.Equipment
                .Include(e => e.Image)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> EquipmentExistsAsync(int id)
        {
            return await RentalDBContext.Equipment.AnyAsync(e => e.Id == id);
        }

        #endregion

        public async Task<List<TopRentedEquipmentStats>> GetTop5RentedEquipmentStatsAsync()
        {

            var top5EquipmentIds = await context.RentalRecords
                .Where(r => r.RentalRequest != null && r.RentalRequest.EquipmentId != null)
                .GroupBy(r => r.RentalRequest.EquipmentId.Value)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => g.Key)
                .ToListAsync();

            return await context.Equipment
                .Where(e => top5EquipmentIds.Contains(e.Id))
                .Select(e => new TopRentedEquipmentStats
                {
                    EquipmentId = e.Id,
                    Name = e.Name,

                    TotalRentals = e.RentalRequests
                        .SelectMany(rq => rq.RentalRecords)
                        .Count(),

                    TotalRevenue = e.RentalRequests
                        .SelectMany(rq => rq.RentalRecords)
                        .Sum(rr => (decimal?)rr.TotalCost) ?? 0,

                    AvgRentalDuration = e.RentalRequests
                        .SelectMany(rq => rq.RentalRecords)
                        .Any(rr => rr.PickupDate != null && rr.ActualReturnDate != null)
                        ? (float)(e.RentalRequests
                            .SelectMany(rq => rq.RentalRecords)
                            .Where(rr => rr.PickupDate != null && rr.ActualReturnDate != null)
                            .Average(rr => (int?)EF.Functions.DateDiffDay(rr.PickupDate, rr.ActualReturnDate.Value)) ?? 0)
                        : 0f,

                    Rating = e.Feedbacks
                        .Any(f => !f.IsHidden.HasValue || !f.IsHidden.Value)
                        ? (float)(e.Feedbacks
                            .Where(f => !f.IsHidden.HasValue || !f.IsHidden.Value)
                            .Average(f => (decimal?)f.Rate) ?? 0)
                        : 0f,
                    ImageGuid = e.Image.Guid ?? null,
                    ImageFormat = e.Image.ImageType
                })
                .ToListAsync();

            //var top5EquipmentIds = await RentalDBContext.RentalRecords
            //    .Where(r => r.RentalRequest != null && r.RentalRequest.EquipmentId != null)
            //    .GroupBy(r => r.RentalRequest.EquipmentId.Value)
            //    .OrderByDescending(g => g.Count())
            //    .Take(5)
            //    .Select(g => g.Key)
            //    .ToListAsync();

            //return await RentalDBContext.Equipment
            //    .Where(e => top5EquipmentIds.Contains(e.Id))
            //    .Select(e => new TopRentedEquipmentStats
            //    {
            //        EquipmentId = e.Id,
            //        Name = e.Name,
            //        TotalRentals = e.RentalRequests.SelectMany(rq => rq.RentalRecords).Count(),
            //        TotalRevenue = e.RentalRequests.SelectMany(rq => rq.RentalRecords).Sum(rr => (decimal?)rr.TotalCost) ?? 0,
            //        AvgRentalDuration = e.RentalRequests.SelectMany(rq => rq.RentalRecords).Any(rr => rr.PickupDate != null && rr.ActualReturnDate != null)
            //            ? (float)(e.RentalRequests.SelectMany(rq => rq.RentalRecords).Where(rr => rr.PickupDate != null && rr.ActualReturnDate != null).Average(rr => (int?)EF.Functions.DateDiffDay(rr.PickupDate, rr.ActualReturnDate.Value)) ?? 0)
            //            : 0f,
            //        Rating = e.Feedbacks.Any(f => !f.IsHidden.HasValue || !f.IsHidden.Value)
            //            ? (float)(e.Feedbacks.Where(f => !f.IsHidden.HasValue || !f.IsHidden.Value).Average(f => (decimal?)f.Rate) ?? 0)
            //            : 0f,
            //        ImageGuid = e.Image.Guid,
            //        ImageFormat = e.Image.ImageType
            //    })
            //    .ToListAsync();
        }


        #region IStatus

        public Dictionary<int, string> GetAllByName()
        {
            return context.Equipment.ToDictionary(e => e.Id, e => e.Name);
        }



        #endregion

        #region Search

        public override Dictionary<string, string> GetEntityColumnsWithTypes()
        {
            var d = base.GetEntityColumnsWithTypes();
            //d.Remove("Description");
            return d;
        }

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
                IsActive = e.IsActive != null,
            });

            return query.Cast<object>();
        }


        #endregion
    }
}
