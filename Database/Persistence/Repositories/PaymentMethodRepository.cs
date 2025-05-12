using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for managing PaymentMethod entities.
    /// </summary>
    internal class PaymentMethodRepository : Repository<PaymentMethod>, IPaymentMethodRepository
    {
        #region Constructor

        public PaymentMethodRepository(RentalDBContext context, int? userId)
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

        #region IStatus Implementation (IPaymentMethodRepository)

        /// <inheritdoc/>
        public Dictionary<int, string> GetAllByName()
        {
            return context.PaymentMethods.ToDictionary(pm => pm.Id, pm => pm.MethodName);
        }

        #endregion
    }
}