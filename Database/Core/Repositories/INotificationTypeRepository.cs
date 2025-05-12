using Database.Core.Domain;
using Database.Interfaces;

namespace Database.Core.Repositories
{
    /// <summary>
    /// Repository interface for managing <see cref="NotificationType"/> entities.
    /// Inherits basic CRUD operations and status listing/filtering via <see cref="IRepository{T}"/> and <see cref="IStatus"/>.
    /// </summary>
    public interface INotificationTypeRepository : IRepository<NotificationType>, IStatus
    {
    }
}