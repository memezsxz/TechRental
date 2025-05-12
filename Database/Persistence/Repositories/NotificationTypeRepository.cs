using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for managing NotificationType entities.
    /// </summary>
    internal class NotificationTypeRepository : Repository<NotificationType>, INotificationTypeRepository
    {
        #region Constructor

        public NotificationTypeRepository(RentalDBContext context, int? userId)
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

        #region IStatus Implementation (INotificationTypeRepository)

        /// <inheritdoc/>
        public Dictionary<int, string> GetAllByName()
        {
            return context.NotificationTypes.ToDictionary(nt => nt.Id, nt => nt.TypeName);
        }

        #endregion
    }
}