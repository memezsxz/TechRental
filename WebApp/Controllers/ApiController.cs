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
            Console.WriteLine("here");
            var categories = _unitOfWork.Categories.GetAllAsync().Result;

            // Fix for CS0165: Initialize the variable
            var categorieslist = new List<string>();

            // Fix for CA1806: Use the result of Append correctly
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
                if (notification.Id != id)
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
