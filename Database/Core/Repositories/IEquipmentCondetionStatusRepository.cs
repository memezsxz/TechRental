using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.Interfaces;

namespace Database.Core.Repositories
{
    /// <summary>
    /// Repository interface for managing <see cref="EquipmentConditionStatus"/> entities.
    /// Inherits standard CRUD operations from <see cref="IRepository{T}"/> and status listing from <see cref="IStatus"/>.
    /// </summary>

    public interface IEquipmentConditionStatusRepository : IRepository<EquipmentConditionStatus>, IStatus
    {
    }
}
