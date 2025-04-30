using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database.Core.Domain;
using Database.Persistence;
using System.Security.Claims;

namespace WebApp.Controllers
{
    public class RentalRequestController : Controller
    {
        private readonly RentalDBContext _context;

        public RentalRequestController(RentalDBContext context)
        {
            _context = context;
        }

        // GET: RentalRequest
        public async Task<IActionResult> Index()
        {
            var rentalDBContext = _context.RentalRequests.Include(r => r.Customer).Include(r => r.Equipment).Include(r => r.Status);
            return View(await rentalDBContext.ToListAsync());
        }

        // GET: RentalRequest/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RentalRequests == null)
            {
                return NotFound();
            }

            var rentalRequest = await _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentalRequest == null)
            {
                return NotFound();
            }

            return View(rentalRequest);
        }

        // GET: RentalRequest/Create
        public IActionResult Create(int equipmentId)
        {
            var equipment = _context.Equipment
                    .Include(e => e.Feedbacks) // Include feedbacks for rating
                    .Include(e => e.ConditionStatus) // accessing ConditionStatus.ConditionName
                    .FirstOrDefault(e => e.Id == equipmentId);

            if (equipment == null)
            {
                return NotFound();
            }

            ViewBag.Equipment = equipment;

            return View();
        }

        // POST: RentalRequest/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentalRequest rentalRequest)
        {
            if (rentalRequest.StartDate == default)
            {
                ModelState.AddModelError("StartDate", "Please select a start date.");
            }

            if (rentalRequest.ReturnDate == default)
            {
                ModelState.AddModelError("ReturnDate", "Please select a return date.");
            }

            if (ModelState.IsValid)
            {
                // Simulated logged-in user
                var userId = "9";

                rentalRequest.CustomerId = int.Parse(userId);
                _context.Add(rentalRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // ✅ Fix: Re-fetch equipment
            var equipment = _context.Equipment
                .Include(e => e.ConditionStatus)
                .Include(e => e.Feedbacks)
                .FirstOrDefault(e => e.Id == rentalRequest.EquipmentId);

            ViewBag.Equipment = equipment;

            return View(rentalRequest);
        }

        // GET: RentalRequest/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RentalRequests == null)
            {
                return NotFound();
            }

            var rentalRequest = await _context.RentalRequests.FindAsync(id);
            if (rentalRequest == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Email", rentalRequest.CustomerId);
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "Id", "Name", rentalRequest.EquipmentId);
            ViewData["StatusId"] = new SelectList(_context.RentalRequestStatuses, "Id", "StatusName", rentalRequest.StatusId);
            return View(rentalRequest);
        }

        // POST: RentalRequest/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EquipmentId,CustomerId,StartDate,ReturnDate,RentalPerDay,StatusId,Notes,CreatedAt,UpdatedAt")] RentalRequest rentalRequest)
        {
            if (id != rentalRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rentalRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalRequestExists(rentalRequest.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Email", rentalRequest.CustomerId);
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "Id", "Name", rentalRequest.EquipmentId);
            ViewData["StatusId"] = new SelectList(_context.RentalRequestStatuses, "Id", "StatusName", rentalRequest.StatusId);
            return View(rentalRequest);
        }

        // GET: RentalRequest/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RentalRequests == null)
            {
                return NotFound();
            }

            var rentalRequest = await _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentalRequest == null)
            {
                return NotFound();
            }

            return View(rentalRequest);
        }

        // POST: RentalRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RentalRequests == null)
            {
                return Problem("Entity set 'RentalDBContext.RentalRequests'  is null.");
            }
            var rentalRequest = await _context.RentalRequests.FindAsync(id);
            if (rentalRequest != null)
            {
                _context.RentalRequests.Remove(rentalRequest);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalRequestExists(int id)
        {
          return (_context.RentalRequests?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
