using Database.Core;
using Database.Core.Domain;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ApiController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        

        public ApiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        public IActionResult notifications(int id) {
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

        public IActionResult Index() {
            return Content("To use api there is two functions: Categories and notifications/{id}");
        }
    }
}
