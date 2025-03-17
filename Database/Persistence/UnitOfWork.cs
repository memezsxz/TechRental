using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Core;
using Database.Core.Domain;
using Database.Core.Repositories;
using Database.Persistence;
using Database.Persistence.Repositories;

namespace Database.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RentalDBContext _context;
        public UnitOfWork(RentalDBContext context)
        {
            _context = context;
            EquipmentAvailabilityStatuses = new Repositories.EquipmentAvailabilityStatusRepository(_context);
            EquipmentConditionStatuses = new EquipmentConditionStatusRepository(_context);
            EquipmentRates = new EquipmentRateRepository(_context);
            Equipment = new EquipmentRepository(_context);
            Categories = new CategoryRepository(_context);
        }

        public IEquipmentAvailabilityStatusRepository EquipmentAvailabilityStatuses { get; private set; }
        public IEquipmentConditionStatusRepository EquipmentConditionStatuses { get; private set; }
        public IEquipmentRateRepository EquipmentRates { get; private set; }
        public IEquipmentRepository Equipment { get; private set; }
        public ICategoryRepository Categories { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
