using Database.Core.Domain;
using Database.Interfaces;
using Database.ViewModels;

namespace Database.Core.Repositories
{
    /// <summary>
    /// Repository interface for managing <see cref="RentalRequest"/> entities.
    /// Inherits basic CRUD operations from <see cref="IRepository{T}"/> and
    /// Tracking and logging capabilities <see cref="IToBeTracked"/>.
    /// Notifications capabilities from <see cref="INotifiable"/>.
    /// </summary>
    public interface IRentalRequestRepository : IRepository<RentalRequest>, IToBeTracked, INotifiable
    {
        /// <summary>
        /// Retrieves a rental request with all related records,
        /// such as associated equipment, rental record, or payment details.
        /// </summary>
        /// <param name="id">The ID of the rental request.</param>
        /// <returns>
        /// A <see cref="RentalRequest"/> instance with related navigation properties populated.
        /// </returns>
        RentalRequest GetWithRecordDetails(int id);

        /// <summary>
        /// Retrieves weekly summary statistics such as total pickups, overdue items,
        /// completed rentals, and damaged returns.
        /// Useful for populating dashboard overviews.
        /// </summary>
        /// <returns>A <see cref="WeeklyStats"/> view model containing key weekly metrics.</returns>
        Task<WeeklyStats> GetWeeklyDashboardStatsAsync();
    }
}