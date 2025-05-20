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

        // GET: Categories
        public  IActionResult Index()
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

        private async Task<bool> CategoryExists(int id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }
    }
}
