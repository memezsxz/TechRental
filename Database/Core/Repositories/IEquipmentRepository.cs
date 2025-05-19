using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.Interfaces;
using Database.ViewModels;

namespace Database.Core.Repositories
{
    /// <summary>
    /// Repository interface for managing <see cref="Equipment"/> entities.
    /// Inherits basic CRUD operations from <see cref="IRepository{T}"/> and
    /// Tracking and logging capabilities <see cref="IToBeTracked"/>.
    /// </summary>
    public interface IEquipmentRepository : IRepository<Equipment>, IStatus, IToBeTracked
    {
        /// <summary>
        /// Retrieves statistics for the top 5 most rented equipment items,
        /// including total rentals, average duration, revenue, and rating.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TopRentedEquipmentStats"/> containing ranking and usage metrics.
        /// </returns>
        Task<List<TopRentedEquipmentStats>> GetTop5RentedEquipmentStatsAsync();

        /// <summary>
        /// Retrieves an equipment entity along with its associated image data (if available).
        /// </summary>
        /// <param name="id">The ID of the equipment to retrieve.</param>
        /// <returns>
        /// An <see cref="Equipment"/> instance with image information included, or null if not found.
        /// </returns>
        Equipment? GetEquipmentWithImage(int id);

        /// <summary>
        /// Determines whether the specified equipment is currently referenced
        /// by other entities (e.g., rental records), useful for safe deletion checks.
        /// </summary>
        /// <param name="id">The ID of the equipment to check.</param>
        /// <returns>True if the equipment is referenced; otherwise, false.</returns>
        bool IsReferenced(int id);

        /// <summary>
        /// Checks whether the specified equipment is currently in use.
        /// </summary>
        /// <param name="id">The equipment ID to check.</param>
        /// <returns>
        /// <c>true</c> if there exists at least one active rental record (i.e., not yet returned) 
        /// associated with the given equipment ID; otherwise, <c>false</c>.
        /// </returns>
        bool IsInUse(int id);
    }
}