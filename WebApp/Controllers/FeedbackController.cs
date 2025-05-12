using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database.Core.Domain;
using Database.Persistence;
using Identity;

namespace WebApp.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly RentalDBContext _context;

        public FeedbackController(RentalDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a list of feedback entries for a specific equipment item,
        /// filtered by visibility status ("Hidden" or "Unhidden").
        ///
        /// Accessible by Admins and Managers.
        /// </summary>
        /// <param name="id">ID of the equipment for which feedback is shown</param>
        /// <param name="status">Visibility filter: "Hidden" or "Unhidden" (default is "Unhidden")</param>
        /// <returns>View with the filtered list of feedback entries</returns>
        public async Task<IActionResult> Index(int id, string status = "Unhidden")
        {
            if (!User.Identity.IsAuthenticated) {
                return View("Unauthorized");
            }

            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager)) {
                return View("Forbidden");

            }

            // Pass equipment ID and current status to the view
            ViewBag.EquipmentId = id;
            ViewBag.Status = status;

            // Build query to fetch feedback for the specified equipment and visibility status
            var feedbacks = _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Equipment)
                .Where(f => f.EquipmentId == id && (status == "Hidden" ? f.IsHidden == true : f.IsHidden == false));

            // Return the view with filtered feedback list
            return View(await feedbacks.ToListAsync());
        }


        // GET: Feedback/Create
        /// <summary>
        /// Renders the form to create a new feedback entry.
        /// 
        /// Provides dropdowns for selecting:
        /// - Equipment (by Name)
        /// - User (by Email)
        ///
        /// Accessible by Customers only.
        /// </summary>
        /// <returns>View for creating a new feedback record</returns>
        public IActionResult Create()
        {

            if (!User.Identity.IsAuthenticated)
            {
                return View("Unauthorized");
            }


            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager))
            {
                return View("Forbidden");

            }

            // Populate dropdowns for Equipment and Users
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");

            return View();
        }

        // POST: Feedback/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Handles form submission to create a new feedback entry submitted by a customer.
        /// 
        /// On success:
        /// - Saves the new feedback to the database
        /// - Sets a success flash message
        /// - Redirects to the Feedback Index page
        ///
        /// On failure:
        /// - Returns a generic Internal Server Error page
        /// 
        /// If validation fails:
        /// - Repopulates the dropdowns and redisplays the form with validation messages
        ///
        /// Accessible by: Customers only.
        /// </summary>
        /// <param name="feedback">Feedback model bound from the form</param>
        /// <returns>Redirect to Index on success, or redisplay form with errors, or error view on exception</returns>
        public async Task<IActionResult> Create([Bind("Id,Note,Rate,TimeDate,UserId,EquipmentId,IsHidden,CreatedAt,UpdatedAt")] Feedback feedback)
        {

            if (!User.Identity.IsAuthenticated)
            {
                return View("Unauthorized");
            }


            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager))
            {
                return View("Forbidden");

            }

            // If form data is invalid, repopulate dropdowns and return the form
            if (!ModelState.IsValid)
            {
                ViewData["EquipmentId"] = new SelectList(_context.Equipment, "Id", "Name", feedback.EquipmentId);
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", feedback.UserId);
                return View(feedback);
            }

            try
            {
                // Save new feedback entry to the database
                _context.Add(feedback);
                await _context.SaveChangesAsync();

                // Set success message using TempData to show after redirect
                TempData["MessageText"] = "Feedback submitted successfully.";
                TempData["MessageType"] = "success";

                // Redirect to Feedback Index page
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                // Handle unexpected database or server error
                return View("InternalServerError");
            }
        }

        // POST: Feedback/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Toggles the visibility status (Hide/Unhide) of a feedback entry.
        /// 
        /// On success:
        /// - Updates the feedback’s IsHidden flag
        /// - Sets a success message
        /// - Redirects to the filtered Feedback Index
        /// 
        /// On failure:
        /// - Sets an error message and redirects to the Feedback Index
        ///
        /// Accessible by: Managers and Admins only.
        /// </summary>
        /// <param name="id">ID of the feedback entry to update</param>
        /// <returns>Redirect to Feedback Index with status preserved</returns>
        public async Task<IActionResult> Edit(int id)
        {


            if (!User.Identity.IsAuthenticated)
            {
                return View("Unauthorized");
            }

            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager))
            {
                return View("Forbidden");

            }
            // Find the feedback by ID
            var feedback = await _context.Feedbacks.FindAsync(id);

            // If not found, show error and redirect to index
            if (feedback == null)
            {
                TempData["MessageType"] = "error";
                TempData["MessageText"] = "Feedback not found.";
                return RedirectToAction("Index", new
                {
                    id = feedback?.EquipmentId,
                    status = feedback?.IsHidden == true ? "Hidden" : "Unhidden"
                });
            }

            // Toggle visibility (hide/unhide) and update timestamp
            feedback.IsHidden = !feedback.IsHidden;
            feedback.UpdatedAt = DateTime.UtcNow;

            // Save changes
            _context.Update(feedback);
            await _context.SaveChangesAsync();

            // Set success message
            TempData["MessageType"] = "success";
            TempData["MessageText"] = feedback.IsHidden == true
                ? "Feedback successfully hidden."
                : "Feedback successfully unhidden.";

            // Redirect to Feedback Index, preserving current status filter
            return RedirectToAction("Index", new
            {
                id = feedback.EquipmentId,
                status = feedback.IsHidden == true ? "Hidden" : "Unhidden"
            });
        }
    }
}
