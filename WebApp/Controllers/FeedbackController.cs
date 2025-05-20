using Microsoft.AspNetCore.Mvc;
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
            if (!User.Identity.IsAuthenticated) return View("Unauthorized");
            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager)) return View("Forbidden");
            if (id == 0) return View("NotFound");

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
        public async Task<IActionResult> Create(int rentalRecordId)
        {
            if (!User.Identity.IsAuthenticated) return View("Unauthorized");
            if (!User.IsInRole(RoleConstants.Customer)) return View("Forbidden");

            var record = await _context.RentalRecords
                .Include(r => r.RentalRequest)
                .ThenInclude(r => r.Customer)
                .FirstOrDefaultAsync(r => r.Id == rentalRecordId);

            if (record == null) return View("NotFound");
            if (record.RentalRequest.Customer.Email.ToLower() != User.Identity.Name.ToLower()) return View("Forbidden");

            var feedback = new Feedback
            {
                RentalRecordId = rentalRecordId,
                UserId = record.RentalRequest.CustomerId,
                EquipmentId = record.RentalRequest.EquipmentId,
                TimeDate = DateTime.UtcNow
            };

            return View(feedback);
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
        public async Task<IActionResult> Create(Feedback feedback)
        {
            if (!User.Identity.IsAuthenticated) return View("Unauthorized");
            if (!User.IsInRole(RoleConstants.Customer)) return View("Forbidden");

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(m => m.Value.Errors.Any())
                    .Select(m => new {
                        Field = m.Key,
                        Errors = m.Value.Errors.Select(e => e.ErrorMessage)
                    });

                TempData["MessageText"] = "Validation failed: " + string.Join(" | ",
                    errors.Select(e => $"{e.Field}: {string.Join(", ", e.Errors)}"));
                TempData["MessageType"] = "error";
                return View(feedback);
            }

            try
            {
                var record = await _context.RentalRecords
                    .Include(r => r.RentalRequest)
                    .ThenInclude(r => r.Customer)
                    .FirstOrDefaultAsync(r => r.Id == feedback.RentalRecordId);

                if (record == null) return View("NotFound");
                if (record.RentalRequest.Customer.Email.ToLower() != User.Identity.Name.ToLower()) return View("Forbidden");

                feedback.UserId = record.RentalRequest.CustomerId;
                feedback.EquipmentId = record.RentalRequest.EquipmentId;
                feedback.CreatedAt = DateTime.UtcNow;
                feedback.UpdatedAt = DateTime.UtcNow;
                feedback.TimeDate = DateTime.UtcNow;
                feedback.IsHidden = false;

                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();

                TempData["MessageText"] = "Feedback submitted successfully.";
                TempData["MessageType"] = "success";
                return RedirectToAction("Details", "RentalRecords", new { id = feedback.RentalRecordId });
            }
            catch
            {
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
            if (!User.Identity.IsAuthenticated) return View("Unauthorized");
            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager)) return View("Forbidden");

            try
            {
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
            catch
            {
                return View("InternalServerError");
            }
        }
    }
}
