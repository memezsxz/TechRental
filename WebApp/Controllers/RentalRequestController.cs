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
        public async Task<IActionResult> Index(string search, string statusFilter, string sortBy, int page = 1, int pageSize = 10)
        {
            var query = _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .Include(r => r.Status)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(r =>
                    r.Equipment.Name.Contains(search) ||
                    r.Customer.Email.Contains(search) ||
                    r.Id.ToString().Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                query = query.Where(r => r.Status.StatusName == statusFilter);
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy)
                {
                    case "date_asc":
                        query = query.OrderBy(r => r.StartDate);
                        break;
                    case "date_desc":
                        query = query.OrderByDescending(r => r.StartDate);
                        break;
                    default:
                        query = query.OrderByDescending(r => r.CreatedAt);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(r => r.CreatedAt);
            }

            var total = await query.CountAsync();

            var requests = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.SortBy = sortBy;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize);
            ViewBag.Search = search;
            ViewBag.StatusFilter = statusFilter;
            ViewBag.StatusOptions = await _context.RentalRequestStatuses
                .Select(s => s.StatusName)
                .Distinct()
                .ToListAsync();

            return View(requests);
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
                    .Include(e => e.Feedbacks)
                    .Include(e => e.ConditionStatus)
                    .FirstOrDefault(e => e.Id == equipmentId);

            if (equipment == null)
            {
                return NotFound();
            }

            var reservedDates = _context.RentalRequests
                .Include(r => r.Status)
                .Where(r => r.EquipmentId == equipmentId && r.Status.StatusName == "Approved")
                .Select(r => new { r.StartDate, r.ReturnDate })
                .ToList();

            var unavailableDates = new List<string>();

            foreach (var range in reservedDates)
            {
                for (DateTime date = range.StartDate.Date; date <= range.ReturnDate.Date; date = date.AddDays(1))
                {
                    if (date >= DateTime.Today)
                        unavailableDates.Add(date.ToString("yyyy-MM-dd"));
                }
            }

            ViewBag.Equipment = equipment;
            ViewBag.UnavailableDates = unavailableDates;

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
                TempData["MessageText"] = "Please select a start date.";
                TempData["MessageType"] = "error";
            }

            if (rentalRequest.ReturnDate == default)
            {
                TempData["MessageText"] = "Please select a return date.";
                TempData["MessageType"] = "error";
            }

            if (!ModelState.IsValid || rentalRequest.ReturnDate == default || rentalRequest.StartDate == default)
            {
                // Re-fetch equipment for redisplay
                var equipment = _context.Equipment
                    .Include(e => e.ConditionStatus)
                    .Include(e => e.Feedbacks)
                    .FirstOrDefault(e => e.Id == rentalRequest.EquipmentId);

                ViewBag.Equipment = equipment;

                // You must also re-pass reserved dates if you're using ViewBag.UnavailableDates
                var reservedDates = _context.RentalRequests
                    .Include(r => r.Status)
                    .Where(r => r.EquipmentId == rentalRequest.EquipmentId && r.Status.StatusName == "Approved")
                    .Select(r => new { r.StartDate, r.ReturnDate })
                    .ToList();

                var unavailableDates = new List<string>();
                foreach (var range in reservedDates)
                {
                    for (DateTime date = range.StartDate.Date; date <= range.ReturnDate.Date; date = date.AddDays(1))
                    {
                        if (date >= DateTime.Today)
                            unavailableDates.Add(date.ToString("yyyy-MM-dd"));
                    }
                }

                ViewBag.UnavailableDates = unavailableDates;

                return View(rentalRequest);
            }

            // Simulate logged-in user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "9";
            rentalRequest.CustomerId = int.Parse(userId);

            _context.Add(rentalRequest);
            await _context.SaveChangesAsync();

            TempData["MessageText"] = "Rental request submitted successfully.";
            TempData["MessageType"] = "success";

            return RedirectToAction(nameof(Index));
        }


        // GET: RentalRequest/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RentalRequests == null)
                return NotFound();

            var rentalRequest = await _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rentalRequest == null)
                return NotFound();

            // Load dropdowns
            ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Email", rentalRequest.CustomerId);
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "Id", "Name", rentalRequest.EquipmentId);
            ViewData["StatusId"] = new SelectList(_context.RentalRequestStatuses, "Id", "StatusName", rentalRequest.StatusId);

            // Build unavailable dates (exclude this rental request's range)
            var reservedRanges = await _context.RentalRequests
                .Include(r => r.Status)
                .Where(r =>
                    r.EquipmentId == rentalRequest.EquipmentId &&
                    r.Status.StatusName == "Approved" &&
                    r.Id != rentalRequest.Id) // Exclude current request
                .Select(r => new { r.StartDate, r.ReturnDate })
                .ToListAsync();

            var unavailableDates = new List<string>();
            foreach (var range in reservedRanges)
            {
                for (DateTime date = range.StartDate.Date; date <= range.ReturnDate.Date; date = date.AddDays(1))
                {
                    if (date >= DateTime.Today)
                        unavailableDates.Add(date.ToString("yyyy-MM-dd"));
                }
            }

            ViewBag.UnavailableDates = unavailableDates;

            return View(rentalRequest);
        }


        // POST: RentalRequest/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RentalRequest rentalRequest)
        {
            if (id != rentalRequest.Id)
            {
                return NotFound();
            }

            if (rentalRequest.StartDate == default)
            {
                TempData["MessageText"] = "Please select a start date.";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Edit), new { id = rentalRequest.Id });
            }

            if (rentalRequest.ReturnDate == default)
            {
                TempData["MessageText"] = "Please select a return date.";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Edit), new { id = rentalRequest.Id });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.RentalRequests.FindAsync(id);
                    if (existing == null)
                        return NotFound();

                    // Update only editable fields
                    existing.StartDate = rentalRequest.StartDate;
                    existing.ReturnDate = rentalRequest.ReturnDate;
                    existing.StatusId = rentalRequest.StatusId;
                    existing.Notes = rentalRequest.Notes;
                    existing.UpdatedAt = DateTime.Now;

                    await _context.SaveChangesAsync();

                    TempData["MessageText"] = "Rental edited successfully.";
                    TempData["MessageType"] = "success";

                    return RedirectToAction("Details", new { id = rentalRequest.Id });
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
            }

            ViewData["CustomerId"] = new SelectList(_context.Users, "Id", "Email", rentalRequest.CustomerId);
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "Id", "Name", rentalRequest.EquipmentId);
            ViewData["StatusId"] = new SelectList(_context.RentalRequestStatuses, "Id", "StatusName", rentalRequest.StatusId);

            return View(rentalRequest);
        }

        private bool RentalRequestExists(int id)
        {
          return (_context.RentalRequests?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
