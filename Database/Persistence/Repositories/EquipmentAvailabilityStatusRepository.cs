using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    internal class EquipmentAvailabilityStatusRepository : Repository<EquipmentAvailabilityStatus>, IEquipmentAvailabilityStatusRepository
    {
        public EquipmentAvailabilityStatusRepository(RentalDBContext context, int? userId) : base(context, userId)
        {
        }

        public RentalDBContext RentalDBContext => context as RentalDBContext;

        public Dictionary<int, string> GetAllByName()
        {
           return  context.EquipmentAvailabilityStatuses.ToDictionary(eas => eas.Id, eas => eas.StatusName);
        }
    }
}
