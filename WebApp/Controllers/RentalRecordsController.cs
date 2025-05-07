using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database.Core.Domain;
using Database.Persistence;

namespace WebApp.Controllers
{
    public class RentalRecordsController : Controller
    {
        private readonly RentalDBContext _context;

        public RentalRecordsController(RentalDBContext context)
        {
            _context = context;
        }

        // GET: RentalRecords
        public async Task<IActionResult> Index(string search, string sortBy, string status, string conditionFilter, int page = 1, int pageSize = 10)
        {
            var query = _context.RentalRecords
                .Include(r => r.RentalRequest)
                .Include(r => r.ReturnCondition)
                .AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(r =>
                    r.EquipmentName.Contains(search) ||
                    r.RentalRequest.Id.ToString().Contains(search));
            }

            // Transaction/Return Toggle
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (status.ToLower() == "transaction")
                    query = query.Where(r => r.ActualReturnDate == null);
                else if (status.ToLower() == "return")
                    query = query.Where(r => r.ActualReturnDate != null);
            }

            // Return Condition Filter
            if (!string.IsNullOrWhiteSpace(conditionFilter))
            {
                query = query.Where(r => r.ReturnCondition.ConditionName == conditionFilter);
            }

            // Sort
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                query = sortBy switch
                {
                    "date_asc" => query.OrderBy(r => r.PickupDate),
                    "date_desc" => query.OrderByDescending(r => r.PickupDate),
                    _ => query.OrderByDescending(r => r.CreatedAt),
                };
            }
            else
            {
                query = query.OrderByDescending(r => r.CreatedAt);
            }

            // Pagination
            var total = await query.CountAsync();
            var records = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // ViewBag for dropdown & pagination
            ViewBag.Search = search;
            ViewBag.SortBy = sortBy;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize);
            ViewBag.RecordStatus = status;
            ViewBag.ConditionFilter = conditionFilter;
            ViewBag.Conditions = await _context.ReturnConditionStatuses
                .Select(c => c.ConditionName)
                .Distinct()
                .ToListAsync();

            return View(records);
        }

        // GET: RentalRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RentalRecords == null)
            {
                return NotFound();
            }

            var rentalRecord = await _context.RentalRecords
                .Include(r => r.RentalRequest)
                .Include(r => r.ReturnCondition)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentalRecord == null)
            {
                return NotFound();
            }

            return View(rentalRecord);
        }

        // GET: RentalRecords/Create
        public IActionResult Create(int rentalRequestId)
        {
            var request = _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .Include(r => r.Status)
                .FirstOrDefault(r => r.Id == rentalRequestId);

            if (request == null) return NotFound();

            var days = (request.ReturnDate.Date - request.StartDate.Date).Days + 1;
            var dailyRate = request.RentalPerDay;
            var rentalFee = dailyRate * days;
            var deposit = Math.Round(request.RentalPerDay.Value * 0.7M, 2);
            var total = rentalFee + deposit;

            ViewBag.RentalRequest = request;
            ViewBag.ReturnConditionId = new SelectList(_context.ReturnConditionStatuses, "Id", "ConditionName");

            var record = new RentalRecord
            {
                RentalRequestId = rentalRequestId,
                EquipmentName = request.Equipment?.Name,
                PickupDate = DateTime.Now,
                RentalFee = rentalFee,
                Deposit = deposit,
                TotalCost = total
            };

            return View(record);
        }



        // POST: RentalRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentalRecord rentalRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rentalRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RentalRequestId"] = new SelectList(_context.RentalRequests, "Id", "Id", rentalRecord.RentalRequestId);
            ViewData["ReturnConditionId"] = new SelectList(_context.ReturnConditionStatuses, "Id", "ConditionName", rentalRecord.ReturnConditionId);
            return View(rentalRecord);
        }

        // GET: RentalRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RentalRecords == null)
            {
                return NotFound();
            }

            var rentalRecord = await _context.RentalRecords.FindAsync(id);
            if (rentalRecord == null)
            {
                return NotFound();
            }
            ViewData["RentalRequestId"] = new SelectList(_context.RentalRequests, "Id", "Id", rentalRecord.RentalRequestId);
            ViewData["ReturnConditionId"] = new SelectList(_context.ReturnConditionStatuses, "Id", "ConditionName", rentalRecord.ReturnConditionId);
            return View(rentalRecord);
        }

        // POST: RentalRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RentalRequestId,EquipmentName,PickupDate,ActualReturnDate,ReturnConditionId,Deposit,LateReturnFees,ExtraCharges,ExtraChargeDescription,TotalCost,RentalFee,CreatedAt,UpdatedAt")] RentalRecord rentalRecord)
        {
            if (id != rentalRecord.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rentalRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalRecordExists(rentalRecord.Id))
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
            ViewData["RentalRequestId"] = new SelectList(_context.RentalRequests, "Id", "Id", rentalRecord.RentalRequestId);
            ViewData["ReturnConditionId"] = new SelectList(_context.ReturnConditionStatuses, "Id", "ConditionName", rentalRecord.ReturnConditionId);
            return View(rentalRecord);
        }

        // GET: RentalRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RentalRecords == null)
            {
                return NotFound();
            }

            var rentalRecord = await _context.RentalRecords
                .Include(r => r.RentalRequest)
                .Include(r => r.ReturnCondition)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentalRecord == null)
            {
                return NotFound();
            }

            return View(rentalRecord);
        }

        // POST: RentalRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RentalRecords == null)
            {
                return Problem("Entity set 'RentalDBContext.RentalRecords'  is null.");
            }
            var rentalRecord = await _context.RentalRecords.FindAsync(id);
            if (rentalRecord != null)
            {
                _context.RentalRecords.Remove(rentalRecord);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalRecordExists(int id)
        {
          return (_context.RentalRecords?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
