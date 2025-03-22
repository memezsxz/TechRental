using Database.Core.Domain;

namespace Database.Core.Repositories
{
    public interface INotificationTypeRepository : IRepository<NotificationType>, IStatus
    {
    }
}