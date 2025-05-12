using Database.Core.Domain;
using Database.Interfaces;

namespace Database.Core.Repositories
{
    /// <summary>
    /// Repository interface for <see cref="Category"/> entity,
    /// Inherits basic CRUD operations from <see cref="IRepository{T}"/>. 
    /// Tracking and logging capabilities from <see cref="IToBeTracked"/>. 
    /// Notification capabilities from <see cref="IToBeTracked"/>.
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>, IStatus, IToBeTracked
    {
        #region Main

        /// <summary>
        /// Asynchronously checks whether a category with the specified ID exists in the database.
        /// </summary>
        /// <param name="id">The ID of the category to check.</param>
        /// <returns>True if the category exists; otherwise, false.</returns>
        Task<bool> ExistsAsync(int id);

        /// <summary>
        /// Determines whether the specified category is referenced by other entities (e.g., Equipment).
        /// Useful for enforcing referential integrity or preventing deletion.
        /// </summary>
        /// <param name="id">The ID of the category to check for references.</param>
        /// <returns>True if the category is referenced; otherwise, false.</returns>
        bool IsReferenced(int id);

        #endregion
    }
}