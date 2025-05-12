using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for managing EquipmentConditionStatus entities.
    /// </summary>
    internal class EquipmentConditionStatusRepository : Repository<EquipmentConditionStatus>, IEquipmentConditionStatusRepository
    {
        #region Constructor

        public EquipmentConditionStatusRepository(RentalDBContext context, int? userId)
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

        #region IStatus Implementation (IEquipmentConditionStatusRepository)

        /// <inheritdoc/>
        public Dictionary<int, string> GetAllByName()
        {
            return RentalDBContext.EquipmentConditionStatuses
                .ToDictionary(ecs => ecs.Id, ecs => ecs.ConditionName);
        }

        #endregion
    }
}