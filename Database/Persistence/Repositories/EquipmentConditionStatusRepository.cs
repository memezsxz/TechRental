using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    internal class EquipmentConditionStatusRepository : Repository<EquipmentConditionStatus>, IEquipmentConditionStatusRepository
    {
        public EquipmentConditionStatusRepository(RentalDBContext context) : base(context)
        {
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }

        public Dictionary<int, string> GetAllByName()
        {
            return RentalDBContext.EquipmentConditionStatuses.ToDictionary(ecs => ecs.Id, ecs => ecs.ConditionName);
        }
    }
}
