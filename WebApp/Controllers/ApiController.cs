using Database.Core;
using Database.Core.Domain;
using Database.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ApiController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RentalDBContext _contxet;
        

        public ApiController(IUnitOfWork unitOfWork, RentalDBContext context)
        {
            _unitOfWork = unitOfWork;
            _contxet = context;
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

        public IActionResult notifications() {

            if (User.Identity.Name == null || User.Identity.Name == "") {
                return Unauthorized("What are you doing here ??? Go and Login");
            }

            var id = _contxet.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id;

            var notifications = _unitOfWork.Notifications.GetAllAsync().Result;

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
