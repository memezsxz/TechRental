using Database.Core.Domain;
using Database.Interfaces;

namespace Database.Core.Repositories
{
    /// <summary>
    /// Repository interface for managing <see cref="UserRole"/> entities.
    /// Inherits standard CRUD operations from <see cref="IRepository{T}"/> and
    /// status listing capabilities from <see cref="IStatus"/>.
    /// </summary>
    public interface IUserRoleRepository : IRepository<UserRole>, IStatus
    {
        /// <summary>
        /// Retrieves the name of a role based on its unique ID.
        /// </summary>
        /// <param name="id">The ID of the user role.</param>
        /// <returns>The name of the role as a string, or null if not found.</returns>
        string getRoleNameByID(int id);
    }
}