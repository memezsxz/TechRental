using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database.Core.Domain;
using Database.Persistence;
using Helper;
using Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly RentalDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public EquipmentController(RentalDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Equipment
        /// <summary>
        /// Displays a paginated, searchable, filterable, and sortable list of equipment items.
        /// - Customers see only active equipment (IsActive == true).
        /// - Admins and Managers see all equipment.
        /// 
        /// Features:
        /// - Search by equipment name
        /// - Filter by category name
        /// - Sort by name or price
        /// - Pagination with bounds validation
        /// 
        /// ViewBag keys:
        /// - Search, Category, SortBy, CurrentPage, TotalPages, Categories
        /// </summary>
        public async Task<IActionResult> Index(string search, string category, string sortBy, int page = 1, int pageSize = 9)
        {
            // Build query with relationships
            var query = _context.Equipment
                .Include(e => e.Category)
                .Include(e => e.ConditionStatus)
                .Include(e => e.Image)
                .AsQueryable();

            // Restrict to active equipment for customers only
            if (User.IsInRole(RoleConstants.Customer))
            {
                query = query.Where(e => e.IsActive == true);
            }

            // Apply search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(e => e.Name.Contains(search));
            }

            // Apply category filter
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(e => e.Category.Name == category);
            }

            // Apply sorting
            query = sortBy switch
            {
                "name_asc" => query.OrderBy(e => e.Name),
                "name_desc" => query.OrderByDescending(e => e.Name),
                "price_asc" => query.OrderBy(e => e.RentalPricePerDay),
                "price_desc" => query.OrderByDescending(e => e.RentalPricePerDay),
                _ => query.OrderBy(e => e.Name)
            };

            // Pagination calculation and validation
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            page = Math.Clamp(page, 1, Math.Max(1, totalPages));

            // Fetch paged records
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // ViewBag state for UI
            ViewBag.Categories = await _context.Categories
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Category = category;
            ViewBag.SortBy = sortBy;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(items);
        }

        // GET: Equipment/Details/5
        /// <summary>
        /// Displays the full details of a single equipment item by ID.
        /// - Loads related entities: image, availability status, condition status, feedbacks, and feedback authors.
        /// - If the user is a Customer and the equipment is inactive (IsActive == false), access is denied (404).
        /// 
        /// Returns: NotFound if the equipment doesn't exist or is restricted from the current user.
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            // Validate ID and context
            if (id == null) return View("NotFound");

            // Load equipment with related entities
            var equipment = await _context.Equipment
                .Include(e => e.Image)
                .Include(e => e.AvailabilityStatus)
                .Include(e => e.ConditionStatus)
                .Include(e => e.Feedbacks)
                    .ThenInclude(f => f.User)
                .FirstOrDefaultAsync(e => e.Id == id);

            // Return 404 if not found
            if (equipment == null) return View("NotFound");

            // Customers can only view active equipment
            if (User.IsInRole(RoleConstants.Customer) && equipment.IsActive == false) return View("NotFound");

            // Filter out hidden feedback
            equipment.Feedbacks = equipment.Feedbacks
                .Where(f => f.IsHidden == false)
                .ToList();

            return View(equipment);
        }


        // GET: Equipment/Create
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.Manager}")]
        /// <summary>
        /// Displays the equipment creation form for Admins and Managers only.
        /// Loads all necessary dropdown lists: availability status, category, and condition.
        /// 
        /// Access Control:
        /// - Restricted to Admins and Managers via [Authorize] and manual role check
        /// - Redirects to "Forbidden" view if accessed by unauthorized user (fallback)
        /// </summary>
        public IActionResult Create()
        {
            // Fallback check (extra safety in addition to [Authorize])
            if (!User.IsInRole(RoleConstants.Manager) && !User.IsInRole(RoleConstants.Admin)) return View("Forbidden");

            // Populate dropdowns for form
            ViewData["AvailabilityStatusId"] = new SelectList(_context.EquipmentAvailabilityStatuses, "Id", "StatusName");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["ConditionStatusId"] = new SelectList(_context.EquipmentConditionStatuses, "Id", "ConditionName");

            return View();
        }

        // POST: Equipment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.Manager}")]
        /// <summary>
        /// Handles the submission of the equipment creation form.
        /// - Restricted to Admins and Managers.
        /// - Validates the uploaded image and binds the data to the Equipment model.
        /// - If validation passes, saves the new equipment record and its image to the database.
        /// - Uses TempData to show success or error messages on redirect.
        /// 
        /// Notes:
        /// - If no image is uploaded, or upload fails, model state is invalid.
        /// - Dropdowns are repopulated if the model is returned with validation errors.
        /// </summary>
        public async Task<IActionResult> Create(Equipment equipment)
        {
            // Fallback check in addition to [Authorize]
            if (!User.IsInRole(RoleConstants.Manager) && !User.IsInRole(RoleConstants.Admin)) return View("Forbidden");

            // Handle image upload
            var uploadedFile = Request.Form.Files["ImageFile"];
            int? uploadedImageId = null;

            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await uploadedFile.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                uploadedImageId = await ImageManager.UploadImageAndSaveToDatabase(
                    _context,
                    memoryStream,
                    uploadedFile.FileName,
                    uploadedFile.ContentType
                );

                if (uploadedImageId == null)
                {
                    ModelState.AddModelError("", "Failed to upload the image.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Please upload an image.");
            }

            // Proceed only if model is valid
            if (ModelState.IsValid)
            {
                try
                {
                    if (uploadedImageId.HasValue)
                    {
                        equipment.ImageId = uploadedImageId.Value;
                    }

                    _context.Add(equipment);
                    await _context.SaveChangesAsync();

                    TempData["MessageText"] = "Equipment was saved successfully!";
                    TempData["MessageType"] = "success";

                    return RedirectToAction(nameof(Details), new { id = equipment.Id });
                }
                catch (Exception)
                {
                    TempData["MessageText"] = "An error occurred while saving the equipment.";
                    TempData["MessageType"] = "error";

                    return RedirectToAction(nameof(Details), new { id = equipment.Id });
                }
            }

            // If model is invalid, repopulate dropdowns and return to form
            ViewData["AvailabilityStatusId"] = new SelectList(_context.EquipmentAvailabilityStatuses, "Id", "StatusName", equipment.AvailabilityStatusId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", equipment.CategoryId);
            ViewData["ConditionStatusId"] = new SelectList(_context.EquipmentConditionStatuses, "Id", "ConditionName", equipment.ConditionStatusId);

            return View(equipment);
        }

        // GET : Equipment/Edit/5
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.Manager}")]
        // <summary>
        /// Displays the equipment edit form for a specific equipment item.
        /// - Restricted to Admins and Managers only.
        /// - Loads the equipment from the database using the provided ID.
        /// - Populates dropdowns for category, availability, condition, and image selection.
        /// 
        /// Access Control:
        /// - Returns 403 Forbidden if the user is not Admin or Manager.
        /// - Returns 404 NotFound if the ID is null or equipment not found.
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            // Extra fallback role check (redundant but explicit)
            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager)) return View("Forbidden");

            // Validate ID and context availability
            if (id == null) return View("NotFound");

            // Retrieve equipment record
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null) return View("NotFound"); // HTTP 404

            // Populate dropdowns for edit form
            ViewData["AvailabilityStatusId"] = new SelectList(_context.EquipmentAvailabilityStatuses, "Id", "StatusName", equipment.AvailabilityStatusId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", equipment.CategoryId);
            ViewData["ConditionStatusId"] = new SelectList(_context.EquipmentConditionStatuses, "Id", "ConditionName", equipment.ConditionStatusId);
            ViewData["ImageId"] = new SelectList(_context.Images, "ImageId", "ImageName", equipment.ImageId);

            return View(equipment);
        }

        // POST: Equipment/Edit/5
        /// <summary>
        /// Handles the form submission for editing an existing equipment item.
        /// - Restricted to Admins and Managers.
        /// - Validates and updates core fields and image if a new one is uploaded.
        /// - If model is valid, saves changes and redirects to Details view.
        /// 
        /// Notes:
        /// - Manual property mapping ensures protection from overposting.
        /// - Optionally supports image replacement and preserves old image ID unless replaced.
        /// - Dropdowns are repopulated if validation fails.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.Manager}")]
        public async Task<IActionResult> Edit(int id, Equipment equipment)
        {
            // Extra role check for safety
            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager)) return View("Forbidden"); // HTTP 403 Forbidden

            // Ensure route ID matches model ID
            if (id != equipment.Id) return View("NotFound"); // HTTP 404

            // Load existing record
            var existingEquipment = await _context.Equipment.FindAsync(id);
            if (existingEquipment == null) return View("NotFound"); // HTTP 404

            // Handle image upload
            var uploadedFile = Request.Form.Files["ImageFile"];
            int? uploadedImageId = null;

            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await uploadedFile.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                uploadedImageId = await ImageManager.UploadImageAndSaveToDatabase(
                    _context,
                    memoryStream,
                    uploadedFile.FileName,
                    uploadedFile.ContentType
                );

                if (uploadedImageId.HasValue)
                {
                    // Optionally delete previous image from S3 (disabled for now)
                    if (existingEquipment.ImageId.HasValue)
                    {
                        await ImageManager.DeleteImageFromDatabaseAndS3(_context, equipment.ImageId.Value);
                    }

                    existingEquipment.ImageId = uploadedImageId.Value;
                }
            }

            // Proceed if model is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Manually map updated fields (prevents overposting)
                    existingEquipment.Name = equipment.Name;
                    existingEquipment.Description = equipment.Description;
                    existingEquipment.RentalPricePerDay = equipment.RentalPricePerDay;
                    existingEquipment.CategoryId = equipment.CategoryId;
                    existingEquipment.ConditionStatusId = equipment.ConditionStatusId;
                    existingEquipment.AvailabilityStatusId = equipment.AvailabilityStatusId;
                    existingEquipment.UpdatedAt = DateTime.UtcNow;

                    _context.Update(existingEquipment);
                    await _context.SaveChangesAsync();

                    TempData["MessageText"] = "Equipment was saved successfully!";
                    TempData["MessageType"] = "success";

                    return RedirectToAction(nameof(Details), new { id = equipment.Id });
                }
                catch (Exception)
                {
                    TempData["MessageText"] = "An error occurred while saving the equipment.";
                    TempData["MessageType"] = "error";

                    return RedirectToAction(nameof(Details), new { id = equipment.Id });
                }
            }

            // Repopulate dropdowns if validation fails
            ViewData["AvailabilityStatusId"] = new SelectList(_context.EquipmentAvailabilityStatuses, "Id", "StatusName", equipment.AvailabilityStatusId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", equipment.CategoryId);
            ViewData["ConditionStatusId"] = new SelectList(_context.EquipmentConditionStatuses, "Id", "ConditionName", equipment.ConditionStatusId);

            return View(equipment);
        }

        /// <summary>
        /// Checks if an equipment item can be safely deleted.
        /// - If the equipment is referenced in any rental requests, it cannot be deleted directly.
        /// - Instead, a prompt is returned offering to mark it as inactive.
        /// - If unreferenced, the equipment (and associated image, if any) is permanently deleted.
        /// 
        /// Access Control:
        /// - Restricted to Admins and Managers only.
        /// 
        /// Returns:
        /// - JSON with either:
        ///     a) `success = true` and redirect URL if deletion is completed
        ///     b) `requiresInactive = true` if the item is in use
        ///     c) `success = false` if the equipment is not found
        /// </summary>
        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.Manager}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCheck(int id)
        {
            // Role fallback check
            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager)) return View("Forbidden"); // HTTP 403 Forbidden

            // Find the equipment record
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null) return Json(new { success = false, message = "Not found" });

            // Check for references in rental requests
            bool isReferenced = await _context.RentalRequests.AnyAsync(r => r.EquipmentId == id);

            if (isReferenced)
            {
                // Return a prompt to mark as inactive instead
                return Json(new
                {
                    requiresInactive = true,
                    message = "This equipment is in use. Do you want to mark it as inactive instead?",
                    setInactiveUrl = Url.Action("SetInactive", "Equipment", new { id })
                });
            }
            else
            {
                // Remove associated image from S3 if available
                if (equipment.ImageId.HasValue)
                {
                    await ImageManager.DeleteImageFromDatabaseAndS3(_context, equipment.ImageId.Value);
                }

                // Delete equipment from database
                _context.Equipment.Remove(equipment);
                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Equipment deleted successfully.",
                    redirectUrl = Url.Action("Index"),
                    type = "success"
                });
            }
        }

        /// <summary>
        /// Marks a specific equipment item as inactive (IsActive = false).
        /// - Used when the equipment is referenced in rental requests and cannot be deleted.
        /// - Intended as a fallback to preserve data integrity while hiding the item from customers.
        /// 
        /// Access Control:
        /// - Restricted to Admins and Managers only.
        /// 
        /// Returns:
        /// - JSON result indicating success or failure:
        ///     a) success = true → item successfully marked as inactive
        ///     b) success = false → item not found
        /// </summary>
        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.Manager}")]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> SetInactive(int id)
        {
            // Role validation fallback
            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager)) return View("Forbidden"); // HTTP 403 Forbidden

            // Locate the equipment by ID
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null) return Json(new { success = false, message = "Equipment not found.", type = "error" });

            // Mark as inactive and save
            equipment.IsActive = false;
            _context.Equipment.Update(equipment);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Equipment marked as inactive.", type = "success" });
        }

        /// <summary>
        /// Marks a specific equipment item as active (IsActive = true).
        /// - Used when an equipment item was previously marked as inactive and needs to be re-enabled.
        /// - Intended to restore availability without creating a new record.
        /// 
        /// Access Control:
        /// - Restricted to Admins and Managers only.
        /// 
        /// Returns:
        /// - JSON result indicating success or failure:
        ///     a) success = true → item successfully marked as active
        ///     b) success = false → item not found or update failed
        /// </summary>
        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.Manager}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetActive(int id)
        {
            // Role validation fallback
            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager)) return View("Forbidden"); // HTTP 403 Forbidden

            // Locate the equipment by ID
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null) return Json(new { success = false, message = "Equipment not found.", type = "error" });

            // Mark as active and save
            equipment.IsActive = true;
            _context.Equipment.Update(equipment);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Equipment marked as active.", type = "success" });
        }
    }
}
