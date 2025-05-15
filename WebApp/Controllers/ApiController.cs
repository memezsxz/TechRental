using Database.Core;
using Database.Core.Domain;
using Database.Persistence;
using Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ApiController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RentalDBContext _contxet;
        private readonly UserManager<ApplicationUser> _userManager;



        public ApiController(IUnitOfWork unitOfWork, RentalDBContext context, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _contxet = context;
            _userManager = userManager;
        }

        public IActionResult ping()
        {
            return Content("pong");
        }

        public IActionResult categories()
        {
            var categories = _unitOfWork.Categories.GetAllAsync().Result;

            List<string> categorieslist = new List<string>();

            foreach (var category in categories)
            {
                if(category.IsActive == false)
                    continue;

                categorieslist.Add(category.Name);
            }


            return Json(categorieslist);
        }

        [HttpGet("/api/notifications")]
        public async Task<IActionResult> GetCurrentUserNotifications()
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            if (User.IsInRole(RoleConstants.Manager) || User.IsInRole(RoleConstants.Admin)) {
                return View("Forbidden");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return NotFound();

            var id = currentUser.UserID;

            var notifications = (await _unitOfWork.Notifications.GetAllAsync())
                .Where(n => n.UserId == id)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();

            return Json(notifications);
        }



        [HttpPost("/api/notifications/markread/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return NotFound();

            var notification = await _unitOfWork.Notifications.GetAsync(id);
            if (notification == null)
                return NotFound();

            // Prevent access to other users’ notifications
            if (notification.UserId != currentUser.UserID)
                return Forbid();

            notification.IsRead = true;
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }

        public IActionResult Index() {
            return Content("To use api there is two functions: Categories and notifications/{id}");
        }
    }
}
