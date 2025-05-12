using Database.Core.Domain;
using Database.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Database.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for managing Notification entities.
    /// </summary>
    internal class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        #region Constructor

        public NotificationRepository(RentalDBContext context, int? userId)
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

        #region INotificationRepository Implementation

        /// <inheritdoc/>
        public List<Notification> GetAllByUser(int userID)
        {
            return RentalDBContext.Notifications
                .Include(n => n.NotificationType)
                .Where(n => n.UserId == userID)
                .ToList();
        }

        /// <inheritdoc/>

        public bool MarkAllAsRead(int userID)
        {
            // Select all unread notifications for the user
            var list = RentalDBContext.Notifications
                .Where(n => n.UserId == userID && !(n.IsRead ?? true));

            // Mark each as read
            foreach (Notification notification in list)
            {
                notification.IsRead = true;
            }

            try
            {
                RentalDBContext.Notifications.UpdateRange(list);
                return RentalDBContext.SaveChanges() >= 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}
