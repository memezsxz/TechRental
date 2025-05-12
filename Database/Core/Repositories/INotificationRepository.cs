using Database.Core.Domain;

namespace Database.Core.Repositories
{
    /// <summary>
    /// Repository interface for managing <see cref="Notification"/> entities.
    /// Inherits basic CRUD operations from <see cref="IRepository{T}"/> and
    /// </summary>
    public interface INotificationRepository : IRepository<Notification>
    {
        /// <summary>
        /// Retrieves all notifications associated with a specific user.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <returns>A list of <see cref="Notification"/> objects for the given user.</returns>
        List<Notification> GetAllByUser(int userID);

        /// <summary>
        /// Marks all notifications as read for a specific user.
        /// </summary>
        /// <param name="userID">The ID of the user whose notifications will be marked as read.</param>
        /// <returns>True if the update succeeded; otherwise, false.</returns>
        bool MarkAllAsRead(int userID);
    }
}