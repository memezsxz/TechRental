using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database.Core.Domain;
using Database.Persistence;
using Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Controllers
{
    [Route("RentalRequest")]
    public class RentalRequestController : Controller
    {
        private readonly RentalDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RentalRequestController(RentalDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: RentalRequest
        [HttpGet("")]
        /// <summary>
        /// Displays a paginated, filtered, and sortable list of rental requests.
        /// Behavior differs based on user role:
        /// - Customers see only their own rental requests.
        /// - Admins and Managers can see all rental requests.
        /// </summary>
        public async Task<IActionResult> Index(string search, string statusFilter, string sortBy, int page = 1, int pageSize = 10)
        {

            if (!User.Identity.IsAuthenticated) {
                return Unauthorized();
            }


            //  Ensure page number is valid (avoid OFFSET negative errors)
            if (page < 1) page = 1;

            //  Base query with necessary includes for related data
            var query = _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .Include(r => r.Status)
                .AsQueryable();

            //  Restrict to only logged-in customer's records if user is Customer
            if (User.IsInRole(RoleConstants.Customer))
            {
                var loggedInEmail = User.Identity?.Name;
                query = query.Where(r => r.Customer.Email == loggedInEmail);
            }

            //  Apply search filter across equipment name, customer email, or request ID
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(r =>
                    r.Equipment.Name.Contains(search) ||
                    r.Customer.Email.Contains(search) ||
                    r.Id.ToString().Contains(search));
            }

            //  Apply status filter if selected
            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                query = query.Where(r => r.Status.StatusName == statusFilter);
            }

            // Apply sorting logic
            query = sortBy switch
            {
                "date_asc" => query.OrderBy(r => r.StartDate),
                "date_desc" => query.OrderByDescending(r => r.StartDate),
                _ => query.OrderByDescending(r => r.CreatedAt) // Default sort by newest first
            };

            //  Get total number of filtered records
            var total = await query.CountAsync();

            //  Recalculate page number if it exceeds total available pages
            var totalPages = (int)Math.Ceiling(total / (double)pageSize);
            if (page > totalPages && totalPages > 0)
                page = totalPages;

            //  Fetch paginated results
            var requests = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            //  Pass filters and paging info to the view via ViewBag
            ViewBag.SortBy = sortBy;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.Search = search;
            ViewBag.StatusFilter = statusFilter;
            ViewBag.StatusOptions = await _context.RentalRequestStatuses
                .Select(s => s.StatusName)
                .Distinct()
                .ToListAsync();

            return View(requests);
        }

        // GET: RentalRequest/Details/5
        /// <summary>
        /// Displays detailed information for a single rental request.
        /// Access is restricted to:
        /// - Logged-in users only
        /// - Customers can only view their own requests
        /// - Admins and Managers can view any request
        /// </summary>
        /// <param name="id">The ID of the rental request to view</param>
        /// <returns>The details view if authorized; otherwise, appropriate error result</returns>
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            //  Block unauthenticated users
            if (!User.Identity.IsAuthenticated) return View("Unauthorized"); // HTTP 401
           
            //  Validate input and data context
            if (id == null || _context.RentalRequests == null) return View("NotFound"); // HTTP 404

            // Fetch rental request and include related entities
            var rentalRequest = await _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rentalRequest == null) return View("NotFound"); // HTTP 404 if not found

            // Restrict customer access to only their own requests
            if (User.IsInRole(RoleConstants.Customer))
            {
                var loggedInEmail = User.Identity?.Name;
                if (!string.Equals(rentalRequest.Customer.Email, loggedInEmail, StringComparison.OrdinalIgnoreCase)) return View("Forbidden"); // HTTP 403 Forbidden
            }

            //  Check if a transaction record exists for this rental
            var rentalRecord = await _context.RentalRecords
                .FirstOrDefaultAsync(r => r.RentalRequestId == id);

            //  Store transaction info in ViewBag for conditional buttons
            ViewBag.HasTransaction = rentalRecord != null;
            ViewBag.TransactionId = rentalRecord?.Id;

            return View(rentalRequest); // Render the view with populated data
        }

        // GET: RentalRequest/Create
        [HttpGet("Create")]
        /// <summary>
        /// Displays the Create Rental Request form for a specific equipment item.
        /// Only accessible to authenticated customers (not admins/managers).
        /// </summary>
        /// <param name="equipmentId">ID of the equipment the user wants to rent.</param>
        /// <returns>Returns the Create view if authorized; otherwise, returns appropriate error views.</returns>
        public IActionResult Create(int equipmentId)
        {
            // Ensure user is logged in
            if (!User.Identity.IsAuthenticated)
                return View("Unauthorized"); // Shows HTTP 401 error page

            // Prevent Admins and Managers from creating rental requests
            if (User.IsInRole(RoleConstants.Manager) || User.IsInRole(RoleConstants.Admin))
                return View("Forbidden"); // Shows HTTP 403 error page

            // Retrieve the equipment record with related feedback and condition data
            var equipment = _context.Equipment
                .Include(e => e.Feedbacks)
                .Include(e => e.ConditionStatus)
                .FirstOrDefault(e => e.Id == equipmentId);

            // If the equipment ID is invalid or not found, show HTTP 404
            if (equipment == null)
                return View("NotFound");

            // call helper to get unavailable date strings
            ViewBag.UnavailableDates = GetUnavailableDates(equipmentId);

            // Pass equipment details to the view for display
            ViewBag.Equipment = equipment;

            return View(); // Returns the rental request creation form
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Processes the rental request form submission.
        /// Validates input, ensures logged-in customer, checks for model state and reserved dates.
        /// </summary>
        /// <param name="rentalRequest">The rental request submitted from the form.</param>
        /// <returns>
        /// Redirects to index on success, redisplays the form with errors on failure,
        /// or shows an error view if an exception occurs.
        /// </returns>
        public async Task<IActionResult> Create(RentalRequest rentalRequest)
        {

            // Ensure user is logged in
            if (!User.Identity.IsAuthenticated)
                return View("Unauthorized"); // Shows HTTP 401 error page

            // Prevent Admins and Managers from creating rental requests
            if (User.IsInRole(RoleConstants.Manager) || User.IsInRole(RoleConstants.Admin))
                return View("Forbidden"); // Shows HTTP 403 error page


            // Ensure both StartDate and ReturnDate are selected
            if (rentalRequest.StartDate == default || rentalRequest.ReturnDate == default)
            {
                TempData["MessageText"] = "Please select both start and return dates.";
                TempData["MessageType"] = "error";
            }

            // If the model is invalid OR missing essential dates
            // Redisplay the form with validation errors and repopulated dropdowns/viewbags
            if (!ModelState.IsValid || rentalRequest.ReturnDate == default || rentalRequest.StartDate == default)
            {
                // Re-fetch the related equipment data to repopulate the form
                var equipment = _context.Equipment
                    .Include(e => e.ConditionStatus)
                    .Include(e => e.Feedbacks)
                    .FirstOrDefault(e => e.Id == rentalRequest.EquipmentId);

                ViewBag.Equipment = equipment;

                // Recompute the list of unavailable rental dates
                ViewBag.UnavailableDates = GetUnavailableDates(rentalRequest.EquipmentId ?? 0);

                return View(rentalRequest); // Redisplay form with validation errors
            }

            // TRY TO SAVE THE RENTAL REQUEST
            // Includes linking the request to the current user and persisting to DB
            try
            {
                // Get the logged-in application user
                var user = (ApplicationUser)await _userManager.GetUserAsync(User);

                // Assign customer ID to the rental request
                rentalRequest.CustomerId = user.UserID;

                // Add and save the new request
                _context.Add(rentalRequest);
                await _context.SaveChangesAsync();

                // Flash success message to user
                TempData["MessageText"] = "Rental request submitted successfully.";
                TempData["MessageType"] = "success";

                // Redirect to the rental request listing page
                return RedirectToAction("Index");
            }
            catch
            {
                return View("InternalServerError"); // If saving fails, return the error view (generic 500 page)
            }
        }

        // GET: RentalRequest/Edit/5
        [HttpGet("Edit/{id}")]
        /// <summary>
        /// Displays the edit form for a rental request with the given ID.
        /// Loads the existing rental data and related entities for dropdowns and date validation.
        /// </summary>
        /// <param name="id">The ID of the rental request to be edited.</param>
        /// <returns>
        /// Returns the Edit view if the request is found; otherwise, shows a 404 Not Found view.
        /// </returns>
        public async Task<IActionResult> Edit(int? id)
        {
            // Ensure a valid ID is passed; otherwise return custom 404 view
            if (id == null) return View("NotFound");

            // Ensure user is logged in
            if (!User.Identity.IsAuthenticated)
                return View("Unauthorized"); // Shows HTTP 401 error page

            // Prevent Admins and Managers from creating rental requests
            if (User.IsInRole(RoleConstants.Manager) || User.IsInRole(RoleConstants.Admin))
                return View("Forbidden"); // Shows HTTP 403 error page

            

            // Get the rental request and its related data (Customer, Equipment, Status)
            var rentalRequest = await _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(m => m.Id == id);

            // If rental request doesn't exist, return a NotFound page
            if (rentalRequest == null) return View("NotFound");

            //make sure only rental requests accessed by the rlated customer 
            if (rentalRequest.Customer.Email.ToLower() != User.Identity.Name.ToLower()) {
                return View("Forbidden");
            }


            // POPULATE DROPDOWNS FOR ADMIN/MANAGER EDIT FORM
            // These are used to display prefilled values and allow modifications
            // ----------------------------------------------------------------------------------
            ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Email", rentalRequest.CustomerId);
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "Id", "Name", rentalRequest.EquipmentId);
            ViewData["StatusId"] = new SelectList(_context.RentalRequestStatuses, "Id", "StatusName", rentalRequest.StatusId);

            // GET UNAVAILABLE DATES FOR CALENDAR DISABLING (excludes current request)
            // ViewBag.UnavailableDates is used by JS to prevent selecting already booked dates
            ViewBag.UnavailableDates = GetUnavailableDates(rentalRequest.EquipmentId ?? 0, excludeRequestId: id.Value);

            // RETURN EDIT VIEW WITH POPULATED RENTAL REQUEST
            return View(rentalRequest);
        }


        // POST: RentalRequest/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Handles the POST submission to update a rental request.
        /// The behavior varies depending on the user’s role:
        /// - Customers can only edit the Notes field if the status is still "Pending".
        /// - Managers/Admins can edit StartDate, ReturnDate, StatusId, and Notes.
        /// The method also validates date overlap with other approved rentals.
        /// </summary>
        /// <param name="id">The ID of the rental request being edited.</param>
        /// <param name="rentalRequest">The updated rental request values submitted from the form.</param>
        /// <returns>
        /// Redirects to the Details view on success, or re-renders the Edit page with error messages on failure.
        /// </returns>
        public async Task<IActionResult> Edit(int id, RentalRequest rentalRequest)
        {

            // Ensure route ID matches the posted model ID to prevent tampering
            if (id != rentalRequest.Id) return View("NotFound");


            // Ensure user is logged in
            if (!User.Identity.IsAuthenticated)
                return View("Unauthorized"); // Shows HTTP 401 error page

            // Prevent Admins and Managers from creating rental requests
            if (User.IsInRole(RoleConstants.Manager) || User.IsInRole(RoleConstants.Admin))
                return View("Forbidden"); // Shows HTTP 403 error page



            try
            {
                // RETRIEVE EXISTING RECORD
                // Include Customer and Status to determine edit permissions
                var existing = await _context.RentalRequests
                    .Include(r => r.Customer)
                    .Include(r => r.Status)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (existing == null) return View("NotFound");

                // DETERMINE USER ROLE & PERMISSIONS
                // - Customers: can only edit Notes if status is Pending
                // - Managers/Admins: can edit full record (dates, status, notes)
                var isCustomer = User.IsInRole(RoleConstants.Customer);
                var isManagerOrAdmin = User.IsInRole(RoleConstants.Manager) || User.IsInRole(RoleConstants.Admin);
                var isPending = existing.Status.StatusName == "Pending";

                if (isCustomer && isPending)
                {
                    // Customers can only change Notes
                    existing.Notes = rentalRequest.Notes;
                }
                else if (isManagerOrAdmin)
                {
                    // Validate required fields for admin/manager
                    if (rentalRequest.StartDate == default || rentalRequest.ReturnDate == default)
                    {
                        TempData["MessageText"] = "Start and Return dates are required.";
                        TempData["MessageType"] = "error";
                        return RedirectToAction("Edit", new { id });
                    }

                    // Update editable fields
                    existing.StartDate = rentalRequest.StartDate;
                    existing.ReturnDate = rentalRequest.ReturnDate;
                    existing.StatusId = rentalRequest.StatusId;
                    existing.Notes = rentalRequest.Notes;
                }
                else
                {
                    // Users not authorized to edit
                    return View("Forbidden");
                }

                // CHECK FOR CONFLICTING RENTAL DATES
                // Prevent overlap with other approved bookings
                var hasConflict = await _context.RentalRequests
                    .Include(r => r.Status)
                    .Where(r => r.EquipmentId == rentalRequest.EquipmentId && r.Id != rentalRequest.Id)
                    .Where(r => r.Status.StatusName == "Approved")
                    .AnyAsync(r =>
                        rentalRequest.StartDate <= r.ReturnDate &&
                        rentalRequest.ReturnDate >= r.StartDate);

                if (hasConflict)
                {
                    TempData["MessageText"] = "Selected dates overlap with an existing approved rental.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction(nameof(Edit), new { id = rentalRequest.Id });
                }

                // SAVE CHANGES TO DATABASE
                // Update timestamp and persist changes
                existing.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                int notificationType = 0; 
                if (rentalRequest.StatusId == 2)
                {
                    notificationType = 1;
                }
                else if (rentalRequest.StatusId == 3)
                {
                    notificationType = 2;
                }
                else if (rentalRequest.StatusId == 4)
                {
                    notificationType = 5;
                }

                NotificationManager.CreateAsync(_context, rentalRequest.CustomerId.Value, notificationType, "request", rentalRequest.Id);

                TempData["MessageText"] = "Rental updated successfully.";
                TempData["MessageType"] = "success";

                return RedirectToAction("Details", new { id });
            }
            catch
            {
                // Unexpected server-side error
                return View("InternalServerError"); // Render 500 error page
            }
        }

        /// <summary>
        /// Retrieves a list of unavailable dates (as "yyyy-MM-dd" strings) for a specific equipment item.
        /// Dates are considered unavailable if they fall within an approved rental request period.
        /// An optional rental request ID can be excluded (useful during editing).
        /// </summary>
        /// <param name="equipmentId">The ID of the equipment to check availability for.</param>
        /// <param name="excludeRequestId">
        /// Optional. If provided, this rental request will be excluded from conflict checking
        /// (commonly used during editing to ignore the currently edited request's date range).
        /// </param>
        /// <returns>
        /// A list of date strings (in "yyyy-MM-dd" format) that are already booked and should be disabled in the calendar.
        /// </returns>
        private List<string> GetUnavailableDates(int equipmentId, int? excludeRequestId = null)
        {
            // FETCH APPROVED RENTALS
            // Only consider rentals with Status = "Approved" for the specified equipment
            var approvedRequests = _context.RentalRequests
                .Include(r => r.Status)
                .Where(r => r.EquipmentId == equipmentId && r.Status.StatusName == "Approved");

            // EXCLUDE CURRENT REQUEST (DURING EDIT)
            // Prevent self-overlap check when editing an existing request
            if (excludeRequestId.HasValue)
            {
                approvedRequests = approvedRequests.Where(r => r.Id != excludeRequestId);
            }

            // BUILD LIST OF UNAVAILABLE DATES
            // Loop through each approved rental range and collect individual dates
            // Only future and current dates are considered (past dates are allowed)
            var unavailableDates = new List<string>();

            foreach (var range in approvedRequests)
            {
                for (DateTime date = range.StartDate.Date; date <= range.ReturnDate.Date; date = date.AddDays(1))
                {
                    if (date >= DateTime.Today)
                        unavailableDates.Add(date.ToString("yyyy-MM-dd"));
                }
            }

            return unavailableDates;
        }
    }
}
