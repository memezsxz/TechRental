using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for managing RentalRequestStatus entities.
    /// </summary>
    internal class RentalRequestStatusRepository : Repository<RentalRequestStatus>, IRentalRequestStatusRepository
    {
        #region Constructor

        public RentalRequestStatusRepository(RentalDBContext context, int userId)
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

        #region IStatus Implementation (IRentalRequestStatusRepository)

        /// <inheritdoc/>
        public Dictionary<int, string> GetAllByName()
        {
            return context.RentalRequestStatuses.ToDictionary(rrs => rrs.Id, rrs => rrs.StatusName);
        }

        #endregion
    }
}