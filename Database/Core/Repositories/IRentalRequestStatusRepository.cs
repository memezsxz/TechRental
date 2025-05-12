using Database.Core.Domain;
using Database.Interfaces;

namespace Database.Core.Repositories
{
    /// <summary>
    /// Repository interface for managing <see cref="RentalRequestStatus"/> entities.
    /// Inherits basic CRUD operations from <see cref="IRepository{T}"/> and
    /// status listing/filtering capabilities from <see cref="IStatus"/>.
    /// </summary>

    public interface IRentalRequestStatusRepository : IRepository<RentalRequestStatus>, IStatus
    {
    }
}