using Database.Core.Domain;

namespace Database.Core.Repositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
         public List<Notification> GetAllByUser(int userID);
         public bool MarkAllAsRead(int userID);
    }
}