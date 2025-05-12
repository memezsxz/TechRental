using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.Interfaces;
using Database.ViewModels;

namespace Database.Core.Repositories
{
    /// <summary>
    /// Repository interface for managing <see cref="RentalRecord"/> entities.
    /// Inherits basic CRUD operations from <see cref="IRepository{T}"/> and
    /// Tracking and logging capabilities <see cref="IToBeTracked"/>.
    /// Notifications capabilities from <see cref="INotifiable"/>.
    /// </summary>
    public interface IRentalRecordRepository : IRepository<RentalRecord>, IToBeTracked, INotifiable
    {
        /// <summary>
        /// Retrieves quarterly earnings for a given year, grouped by quarter and month.
        /// </summary>
        /// <param name="year">The year to calculate earnings for.</param>
        /// <returns>
        /// A list of <see cref="QuarterEarnings"/> objects representing aggregated financial data by quarter.
        /// </returns>
        Task<List<QuarterEarnings>> GetQuarterEarningsByYearAsync(int year);

        /// <summary>
        /// Retrieves rental counts for each category over the current week.
        /// Useful for generating category-based weekly charts.
        /// </summary>
        /// <param name="categoryLimit">Optional limit for how many top categories to return. Default is 10.</param>
        /// <returns>A dictionary where keys are category names and values are rental counts.</returns>
        Task<Dictionary<string, int>> GetWeeklyCategoryRentalDataAsync(int categoryLimit = 10);

        /// <summary>
        /// Retrieves a rental record along with its related data (e.g., equipment, user, payment).
        /// </summary>
        /// <param name="id">The ID of the rental record to retrieve.</param>
        /// <returns>The rental record with all relevant navigation properties populated.</returns>
        RentalRecord GetWithDetails(int id);

        /// <summary>
        /// Retrieves a rental record by its associated rental request ID, including related details.
        /// </summary>
        /// <param name="id">The rental request ID.</param>
        /// <returns>The matching rental record with all related data populated.</returns>
        RentalRecord GetWithDetailsByRentalRequest(int id);
    }
}
