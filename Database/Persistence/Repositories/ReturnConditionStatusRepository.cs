using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for managing ReturnConditionStatus entities.
    /// </summary>
    internal class ReturnConditionStatusRepository : Repository<ReturnConditionStatus>, IReturnConditionStatusRepository
    {
        #region Constructor

        public ReturnConditionStatusRepository(RentalDBContext context, int? userId)
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

        #region IStatus Implementation (IReturnConditionStatusRepository)

        /// <inheritdoc/>
        public Dictionary<int, string> GetAllByName()
        {
            return context.ReturnConditionStatuses.ToDictionary(rrs => rrs.Id, rrs => rrs.ConditionName);
        }

        #endregion
    }
}