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

namespace WebApp.Controllers
{
    public class CategoriesController : Controller
    {

        private readonly RentalDBContext _context;

        public CategoriesController(RentalDBContext context)
        {
            _context = context;
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

        // GET: Categories/Details/5
        public IActionResult Details(int id)
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
                try
                {
                    _context.Categories.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CategoryExists(category.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int id)
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

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            if (!User.Identity.IsAuthenticated)
            {
                return View("Unauthorized");
            }

            if (!User.IsInRole(RoleConstants.Admin))
            {
                return View("Forbidden");
            }

            if (_context.Categories == null)
            {
                return Problem("Entity set 'RentalDBContext.Categories'  is null.");
            }
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CategoryExists(int id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }
    }
}
