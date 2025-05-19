using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using Microsoft.Extensions.Options;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailSettings _emailSettings;


        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, IOptions<EmailSettings> emailSettings)
        {
            _logger = logger;
            _userManager = userManager;
            _emailSettings = emailSettings.Value;
        }

        public async Task<IActionResult> Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Admin") || roles.Contains("Manager"))
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }

            return View();
        }

        public IActionResult Policy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactFormModel model)
        {
            if (!ModelState.IsValid) return View("About");
        
            // Send the email
            var body = $"From: {model.FirstName} {model.LastName} ({model.Email})\n\nMessage:\n{model.Message}";
            var subject = $"Contact Form Submission from: {model.FirstName} {model.LastName} ";
            try
            {
                await SendEmailAsync("techrental9@gmail.com", subject, body);
                TempData["MessageText"] = "Your message has been sent!";
                TempData["MessageType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["MessageText"] = "Error while trying to send message, Try again later.";
                TempData["MessageType"] = "error";
            }
            
            return RedirectToAction("About");
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(_emailSettings.FromEmail, _emailSettings.AppPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = false,
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }

        public IActionResult ErrorHandler(int? statusCode = null)
        {
            switch (statusCode)
            {
                case 401:
                    return View("~/Views/Shared/Unauthorized.cshtml");
                case 403:
                    return View("~/Views/Shared/Forbidden.cshtml");                
                case 405:
                    return View("~/Views/Shared/MethodNotAllowed.cshtml");
                case 500:
                    return View("~/Views/Shared/InternalServerError.cshtml");
                default:
                    return View("~/Views/Shared/NotFound.cshtml");
            }
        }

    }
}
