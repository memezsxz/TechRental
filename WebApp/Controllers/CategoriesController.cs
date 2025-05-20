using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database.Core.Domain;
using Database.Persistence;
using Identity;
using System.Drawing.Text;
using Microsoft.AspNetCore.Identity;
using Sprache;

namespace WebApp.Controllers
{
    public class CategoriesController : Controller
    {

        private readonly RentalDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoriesController(RentalDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Displays a list of all categories in the system.
        /// - Restricted to authenticated Admin users only.
        /// - Returns Forbidden or Unauthorized views based on access checks.
        /// 
        /// Returns:
        /// - View of category list if successful
        /// - View("Forbidden") if not an Admin
        /// - View("Unauthorized") if not logged in
        /// </summary>

        // GET: Categories
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated) {
                return View("Unauthorized");
            }

            if (!User.IsInRole(RoleConstants.Admin)) {
                return View("Forbidden");
            }


            return _context.Categories != null ?
                        View( _context.Categories.ToList()) :
                        Problem("Entity set 'RentalDBContext.Categories'  is null.");
        }

        /// <summary>
        /// Displays the form for creating a new category.
        /// - Restricted to authenticated Admin users only.
        /// 
        /// Returns:
        /// - Create view for category
        /// - View("Forbidden") or View("Unauthorized") if access is denied
        /// </summary>

        // GET: Categories/Create
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("Unauthorized");
            }

            if (!User.IsInRole(RoleConstants.Admin))
            {
                return View("Forbidden");
            }

            return View();
        }

        /// <summary>
        /// Handles submission of the category creation form.
        /// - Binds form data to a new Category entity.
        /// - Restricted to authenticated Admin users only.
        /// - Adds the category to the database and redirects to Index on success.
        /// 
        /// Returns:
        /// - Redirects to Index on success
        /// - Returns form with validation errors otherwise
        /// </summary>

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,IsActive,CreatedAt,UpdatedAt")] Category category)
        {

            if (!User.Identity.IsAuthenticated)
            {
                return View("Unauthorized");
            }

            if (!User.IsInRole(RoleConstants.Admin))
            {
                return View("Forbidden");
            }

            if (ModelState.IsValid)
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                TempData["MessageText"] = "Category created successfully.";
                TempData["MessageType"] = "success";

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        /// <summary>
        /// Displays the edit form for a specific category by ID.
        /// - Restricted to authenticated Admin users only.
        /// - Returns 404 if category not found.
        /// 
        /// Returns:
        /// - Edit view with pre-filled category data
        /// - NotFound or Forbidden/Unauthorized view if conditions fail
        /// </summary>

        // GET: Categories/Edit/5
        public IActionResult Edit(int id)
        {

            if (!User.Identity.IsAuthenticated)
            {
                return View("Unauthorized");
            }

            if (!User.IsInRole(RoleConstants.Admin))
            {
                return View("Forbidden");
            }

            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        /// <summary>
        /// Handles form submission for editing a category.
        /// - Restricted to authenticated Admin users only.
        /// - Compares old and new data for audit logging.
        /// - Handles concurrency and logs any update exceptions.
        /// 
        /// Returns:
        /// - Redirects to Index on success
        /// - Returns form view if validation fails or error occurs
        /// </summary>
        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,IsActive,CreatedAt,UpdatedAt")] Category category)
        {

            if (!User.Identity.IsAuthenticated)
            {
                return View("Unauthorized");
            }

            if (!User.IsInRole(RoleConstants.Admin))
            {
                return View("Forbidden");
            }

            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var trackedCategory = await _context.Categories.FindAsync(id);
                if (trackedCategory == null)
                {
                    return NotFound();
                }

                var oldData = System.Text.Json.JsonSerializer.Serialize(trackedCategory);

                // Update the tracked entity instead of replacing it
                trackedCategory.Name = category.Name;
                trackedCategory.Description = category.Description;
                trackedCategory.IsActive = category.IsActive;
                trackedCategory.UpdatedAt = DateTime.Now;

                var newData = System.Text.Json.JsonSerializer.Serialize(trackedCategory);
                var userid = (int)(await _userManager.GetUserAsync(User)).UserID;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!await CategoryExists(category.Id))
                        return NotFound();
                    else
                    {
                        // log the error
                        await ErrorLogger.LogErrorAsync(
                            context: _context,
                            userId: userid,
                            errorMessage: ex.Message,
                            errorSource: ex.Source ?? "Unknown",
                            sourceProcedure: Exception.ReferenceEquals(ex.TargetSite, null) ? "Unknown" : ex.TargetSite.Name
                        );

                        throw;
                    }
                }

                TempData["MessageText"] = "Category edited successfully.";
                TempData["MessageType"] = "success";

                await AuditLogger.LogActionAsync(
                    context: _context,
                    userId: userid,
                    actionType: "Update",
                    sourceEntity: "Category",
                    dataBefore: oldData,
                    dataAfter: newData,
                    affectedRecordKey: id.ToString()
                );

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


        /// <summary>
        /// Checks if a category with a given ID exists in the database.
        /// 
        /// Returns:
        /// - True if category exists, otherwise false
        /// </summary>
        private async Task<bool> CategoryExists(int id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }
    }
}
