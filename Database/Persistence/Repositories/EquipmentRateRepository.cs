using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    internal class EquipmentRateRepository : Repository<EquipmentRate>, IEquipmentRateRepository
    {
        public EquipmentRateRepository(RentalDBContext context) : base(context)
        {
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }
    }
}
