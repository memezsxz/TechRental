using Database.Persistence;
using Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers
{
    public class Dashboard : Controller
    {
        private readonly RentalDBContext _context;
        public Dashboard(RentalDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            if (!(User.IsInRole(RoleConstants.Admin) || User.IsInRole(RoleConstants.Manager)))
            {
                return View("Forbidden");
            }

            var role = User.IsInRole("Admin") ? "Admin" : "Manager";

            var pending = await _context.RentalRequests
                .CountAsync(r => r.Status.StatusName == "Pending");

            var approved = await _context.RentalRequests
                .CountAsync(r => r.Status.StatusName == "Approved");

            var rejected = await _context.RentalRequests
                .CountAsync(r => r.Status.StatusName == "Rejected");

            var totalRequests = await _context.RentalRequests.CountAsync();

            var damaged = await _context.Equipment
                .CountAsync(e => e.ConditionStatus.ConditionName == "Needs Repair");

            var totalCost = await _context.RentalRecords
                .SumAsync(r => (decimal?)r.TotalCost) ?? 0;

            var recent = await _context.RentalRequests
                .OrderByDescending(r => r.CreatedAt)
                .Where(r => r.Status.StatusName == "Pending")
                .Take(5)
                .Select(r => new
                {
                    RequestId = r.Id,
                    EquipmentName = r.Equipment.Name,
                    Category = r.Equipment.Category.Name,
                    Status = r.Status.StatusName,
                    CustomerName = r.Customer.FirstName + " " + r.Customer.LastName,
                    StartDate = r.StartDate,
                    ReturnDate = r.ReturnDate
                })
                .ToListAsync();

            int totalUsers = 0, admins = 0, managers = 0;

            if (role == "Admin")
            {
                totalUsers = await _context.Users
                    .Where(u => u.IsActive.Value).CountAsync();

                admins = await _context.Users
                    .Where(u => u.IsActive.Value && u.Role.RoleName == "Admin")
                    .CountAsync();

                managers = await _context.Users
                    .Where(u => u.IsActive.Value && u.Role.RoleName == "Manager")
                    .CountAsync();
            }

            var stats = new
            {
                PendingRequests = pending,
                ApprovedRequests = approved,
                RejectedRequests = rejected,
                TotalRequests = totalRequests,
                DamagedEquipment = damaged,
                TotalCost = totalCost,
                RecentRequests = recent,
                TotalUsers = totalUsers,
                Admins = admins,
                Managers = managers
            };

            ViewBag.Role = role;
            ViewBag.Stats = stats;

            return View();
        }



        public async Task<IActionResult> ChartData()
        {
            if (!(User.IsInRole(RoleConstants.Admin) || User.IsInRole(RoleConstants.Manager)))
            {
                return View("Forbidden");
            }

            var statusCounts = await _context.RentalRequests
                .Include(r => r.Status)
                .GroupBy(r => r.Status.StatusName)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                }).ToListAsync();

            var mostRented = await _context.RentalRequests
                .GroupBy(r => r.Equipment.Name)
                .Select(g => new
                {
                    EquipmentName = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .Take(4)
                .ToListAsync();

            return Json(new
            {
                StatusCounts = statusCounts,
                MostRented = mostRented
            });
        }

    }
}
