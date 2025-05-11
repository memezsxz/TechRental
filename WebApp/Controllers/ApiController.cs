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

        public async Task<IActionResult> notificationsAsync() {

            if (!User.Identity.IsAuthenticated) {
                return View("Unauthorized");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return View("NotFound");
            }

            var id = currentUser.UserID;

            var notifications = _unitOfWork.Notifications.GetAllAsync().Result.OrderByDescending(u => u.CreatedAt);

            var notificationList = new List<Notification>();

            foreach (var notification in notifications)
            {
                if (notification.UserId != id)
                    continue;

                notificationList.Add(notification);
            }
            return Json(notificationList);
        }

        [HttpPost("/api/notifications/markread/{id}")]
        public IActionResult MarkAsRead(int id)
        {
            var notification = _unitOfWork.Notifications.GetAsync(id).Result;
            if (notification == null) return NotFound();

            notification.IsRead = true;
            _unitOfWork.SaveChanges(); // Or SaveChanges()

            return Ok();
        }

        public IActionResult Index() {
            return Content("To use api there is two functions: Categories and notifications/{id}");
        }
    }
}
