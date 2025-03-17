using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Repositories;
using Database.Persistence.Repositories;

namespace Database.Core
{
    internal interface IUnitOfWork  : IDisposable
    {
        public IEquipmentAvailabilityStatusRepository EquipmentAvailabilityStatuses { get;  }
        public IEquipmentConditionStatusRepository EquipmentConditionStatuses { get; }
        public IEquipmentRateRepository EquipmentRates { get; }
        public IEquipmentRepository Equipment { get; }
        public ICategoryRepository Categories { get; }
        int Complete();
    }
}
