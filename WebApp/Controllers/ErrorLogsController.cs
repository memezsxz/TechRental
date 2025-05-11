using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Database.Persistence;

namespace WebApp.Controllers
{
    public class ErrorLogsController : Controller
    {
        private readonly RentalDBContext _context;

        public ErrorLogsController(RentalDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays the Error Logs page with support for:
        /// - Keyword search (across message, source, and procedure)
        /// - Filtering by source (ErrorSource)
        /// - Sorting by timestamp
        /// - Pagination of results
        /// 
        /// Accessible by Admins only.
        /// 
        /// </summary>
        /// <param name="search">Search term to match in ErrorMessage, ErrorSource, or SourceProcedure</param>
        /// <param name="sortBy">Sort order by timestamp: "timestamp_asc" or default "timestamp_desc"</param>
        /// <param name="sourceFilter">Filter by exact ErrorSource value</param>
        /// <param name="userIdFilter">[DEPRECATED] Filter logs by User ID</param>
        /// <param name="page">Current page number (defaults to 1)</param>
        /// <param name="pageSize">Items per page (defaults to 15)</param>
        /// <returns>View with filtered and paginated error logs</returns>
        public async Task<IActionResult> Index(string search, string sortBy, string sourceFilter, string userIdFilter, int page = 1, int pageSize = 15)
        {
            // Base query includes related user for display
            var query = _context.SystemErrorLogs
                .Include(e => e.User)
                .AsQueryable();

            // Search logic across 3 fields
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(e =>
                    e.ErrorMessage.Contains(search) ||
                    e.ErrorSource.Contains(search) ||
                    e.SourceProcedure.Contains(search));
            }

            // Filter by exact ErrorSource (e.g., "Website")
            if (!string.IsNullOrWhiteSpace(sourceFilter))
            {
                query = query.Where(e => e.ErrorSource == sourceFilter);
            }

            // Sort by timestamp
            query = sortBy switch
            {
                "timestamp_asc" => query.OrderBy(e => e.Timestamp),
                _ => query.OrderByDescending(e => e.Timestamp)
            };

            // Pagination calculation
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Ensure page number is valid
            page = Math.Max(1, page);
            if (totalPages > 0 && page > totalPages) page = totalPages;

            // Get paginated logs
            var logs = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Set ViewBag for UI state persistence
            ViewBag.Search = search;
            ViewBag.SortBy = sortBy;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SourceFilter = sourceFilter;

            // Load distinct ErrorSources for filter dropdown
            ViewBag.AllSources = await _context.SystemErrorLogs
                .Select(e => e.ErrorSource)
                .Where(s => s != null)
                .Distinct()
                .ToListAsync();

            return View(logs);
        }

    }
}
