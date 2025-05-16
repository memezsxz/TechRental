using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for managing PaymentStatus entities.
    /// </summary>
    internal class PaymentStatusRepository : Repository<PaymentStatus>, IPaymentStatusRepository
    {
        #region Constructor

        public PaymentStatusRepository(RentalDBContext context, int userId)
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

        #region IStatus Implementation (IPaymentStatusRepository)

        /// <inheritdoc/>
        public Dictionary<int, string> GetAllByName()
        {
            return context.PaymentStatuses.ToDictionary(ps => ps.Id, ps => ps.StatusName);
        }

        #endregion
    }
}