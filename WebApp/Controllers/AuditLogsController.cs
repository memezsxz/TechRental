using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Database.Persistence;
using Microsoft.AspNetCore.Authorization;
using Identity;

namespace WebApp.Controllers
{
    public class AuditLogsController : Controller
    {
        private readonly RentalDBContext _context;

        public AuditLogsController(RentalDBContext context)
        {
            _context = context;
        }

        // GET: AuditLogs
        /// <summary>
        /// Displays the Audit Logs page with support for:
        /// - Search by multiple fields
        /// - Filtering by Source Entity
        /// - Sorting by timestamp
        /// - Pagination
        /// 
        /// Accessible by Admins only.
        /// </summary>
        /// <param name="search">Search query text (applied to ActionType, SourceEntity, Source, DataBeforeAction, DataAfterAction)</param>
        /// <param name="sortBy">Sort order (timestamp_asc or timestamp_desc)</param>
        /// <param name="sourceFilter">Exact match filter by SourceEntity</param>
        /// <param name="userIdFilter">[DEPRECATED] Filter by user ID (kept for backward compatibility)</param>
        /// <param name="page">Current page number (defaults to 1)</param>
        /// <param name="pageSize">Number of records per page (defaults to 15)</param>
        /// <returns>Paginated and filtered list of AuditLog entries</returns>
        public async Task<IActionResult> Index(string search, string sortBy, string sourceFilter, string userIdFilter, int page = 1, int pageSize = 15)
        {
            if (!User.IsInRole(RoleConstants.Admin)) {
                return View("Forbidden");
            }


            // Base query with related User included
            var query = _context.AuditLogs
                .Include(a => a.User)
                .AsQueryable();

            // Apply search filter across multiple relevant fields
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(a =>
                    a.ActionType.Contains(search) ||
                    a.SourceEntity.Contains(search) ||
                    a.Source.Contains(search) ||
                    a.DataBeforeAction.Contains(search) ||
                    a.DataAfterAction.Contains(search));
            }

            // Apply exact match filter on SourceEntity
            if (!string.IsNullOrWhiteSpace(sourceFilter))
            {
                query = query.Where(a => a.SourceEntity == sourceFilter);
            }

            // Apply sorting based on timestamp
            query = sortBy switch
            {
                "timestamp_asc" => query.OrderBy(a => a.Timestamp),
                _ => query.OrderByDescending(a => a.Timestamp) // Default to newest first
            };

            // Pagination setup
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Ensure current page is within bounds
            page = Math.Max(1, page);
            if (totalPages > 0 && page > totalPages) page = totalPages;

            // Fetch paged records
            var logs = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Populate ViewBag for rendering current filters and pagination state
            ViewBag.Search = search;
            ViewBag.SortBy = sortBy;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SourceFilter = sourceFilter;

            // Provide all distinct source entity names for dropdown filtering
            ViewBag.AllSources = await _context.AuditLogs
                .Select(a => a.SourceEntity)
                .Where(s => s != null)
                .Distinct()
                .ToListAsync();

            return View(logs);
        }

    }
}
