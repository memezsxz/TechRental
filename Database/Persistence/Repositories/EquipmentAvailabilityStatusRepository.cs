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
    /// <summary>
    /// Repository implementation for managing EquipmentAvailabilityStatus entities.
    /// </summary>
    internal class EquipmentAvailabilityStatusRepository : Repository<EquipmentAvailabilityStatus>, IEquipmentAvailabilityStatusRepository
    {
        #region Constructor

        public EquipmentAvailabilityStatusRepository(RentalDBContext context, int userId)
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

        #region IStatus Implementation (IEquipmentAvailabilityStatusRepository)

        /// <inheritdoc/>
        public Dictionary<int, string> GetAllByName()
        {
            return context.EquipmentAvailabilityStatuses
                .ToDictionary(eas => eas.Id, eas => eas.StatusName);
        }

        #endregion
    }
}