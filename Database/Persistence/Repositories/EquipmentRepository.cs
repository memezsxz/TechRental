using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.Core.Repositories;
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
