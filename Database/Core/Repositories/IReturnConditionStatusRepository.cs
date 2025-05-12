using Database.Core.Domain;
using Database.Interfaces;

namespace Database.Core.Repositories
{
    /// <summary>
    /// Repository interface for managing <see cref="ReturnConditionStatus"/> entities.
    /// Inherits standard CRUD operations from <see cref="IRepository{T}"/> and
    /// status listing/filtering functionality from <see cref="IStatus"/>.
    /// </summary>

    public interface IReturnConditionStatusRepository : IRepository<ReturnConditionStatus> , IStatus
    {
    }
}