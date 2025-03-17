using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;

namespace Database.Core.Repositories
{
    public interface IEquipmentConditionStatusRepository : IRepository<EquipmentConditionStatus>, IStatus
    {
    }
}
