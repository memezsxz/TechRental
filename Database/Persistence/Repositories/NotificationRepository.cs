using Database.Core.Domain;
using Database.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Database.Persistence.Repositories
{
    internal class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(RentalDBContext context) : base(context)
        {
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }


        public List<Notification> GetAllByUser(int userID)
        {
            return RentalDBContext.Notifications.Include(n => n.NotificationType).Where(n => n.UserId == userID).ToList();
        }

        public bool MarkAllAsRead(int userID)
        {
            var list = RentalDBContext.Notifications
                .Where(n => n.UserId == userID && !(n.IsRead ?? true));
            foreach (Notification notification in list)
            {
                notification.IsRead = true;
            }

            try
            {
                RentalDBContext.Notifications.UpdateRange(list);
                return RentalDBContext.SaveChanges() >= 0;
            }
            catch (Exception e)
            {
                return false;
            }

        }

    }

}