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
            {
                var role = User.IsInRole("Admin") ? "Admin" : "Manager";

                var stats = new
                {

                    PendingRequests = await _context.RentalRequests
                        .Include(r => r.Status)
                        .CountAsync(r => r.Status.StatusName == "Pending"),

                    ApprovedRequests = await _context.RentalRequests
                        .Include(r => r.Status)
                        .CountAsync(r => r.Status.StatusName == "Approved"),

                    RejectedRequests = await _context.RentalRequests
                        .Include(r => r.Status)
                        .CountAsync(r => r.Status.StatusName == "Rejected"),

                    TotalRequests = await _context.RentalRequests.CountAsync(),

                    DamagedEquipment = await _context.Equipment
                        .Include(e => e.ConditionStatus)
                        .CountAsync(e => e.ConditionStatus.ConditionName == "Needs Repair"),

                    TotalRepairCost = await _context.RentalRecords
                        .SumAsync(r => (decimal?)r.TotalCost) ?? 0,

                    RecentRequests = await _context.RentalRequests
                        .Include(r => r.Equipment)
                        .ThenInclude(e => e.Category)
                        .Include(r => r.Status)
                        .Include(r => r.Customer)
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
                        .ToListAsync(),

                    TotalUsers = role == "Admin" ? await _context.Users
                    .Where(u => u.IsActive == true)
                    .CountAsync() : 0,

                    Admins = role == "Admin" ?
                        await _context.Users
                            .Include(u => u.Role)
                            .Where(u => u.IsActive == true)
                            .CountAsync(u => u.Role.RoleName == "Admin") : 0,

                    Managers = role == "Admin" ?
                        await _context.Users
                            .Include(u => u.Role)
                            .Where(u => u.IsActive == true)
                            .CountAsync(u => u.Role.RoleName == "Manager") : 0
                };

                ViewBag.Role = role;
                ViewBag.Stats = stats;

                return View();
            }
        }
    }
}
