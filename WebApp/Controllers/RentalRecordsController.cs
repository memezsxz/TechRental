using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database.Core.Domain;
using Database.Persistence;
using Helper;
using Identity;
using Microsoft.AspNetCore.Authorization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApp.Controllers
{
    [Authorize]
    public class RentalRecordsController : Controller
    {
        private readonly RentalDBContext _context;

        public RentalRecordsController(RentalDBContext context)
        {
            _context = context;
        }

        // GET: RentalRecords
        public async Task<IActionResult> Index(string? userEmail, string search, string sortBy, string status, string conditionFilter, int page = 1, int pageSize = 10)
        {

            //do not allow the Unuthenticated users to enter this
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }



            var query = _context.RentalRecords
                .Include(r => r.RentalRequest).ThenInclude(r => r.Customer)
                .Include(r => r.ReturnCondition)
                .AsQueryable();

            if (User.IsInRole(RoleConstants.Customer))
            {
                var loggedInEmail = User.Identity.Name;

                // If the passed email doesn't match the logged-in user's email, deny access
                if (!string.Equals(userEmail, loggedInEmail, StringComparison.OrdinalIgnoreCase))
                {
                    return Forbid(); //Authenticated but not allowed
                }

                // Filter the data to only show this user's records
                query = query.Where(u => u.RentalRequest.Customer.Email == loggedInEmail);

            }


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
            //do not allow the Unuthenticated users to enter this
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            if (id == null || _context.RentalRecords == null)
            {
                return NotFound();
            }

            var rentalRecord = await _context.RentalRecords
                .Include(r => r.RentalRequest)
                .ThenInclude(r => r.Customer)
                .Include(r => r.ReturnCondition)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rentalRecord == null)
            {
                return NotFound();
            }


            if (User.IsInRole(RoleConstants.Customer))
            {

                var loggedInEmail = User.Identity.Name;
                if (rentalRecord.RentalRequest.Customer.Email != loggedInEmail)
                {
                    return Forbid(); //Authenticated but not allowed

                }


            }

            // Retrieve the uploaded agreement file from Document table (if available)
            var document = await _context.Documents
                .FirstOrDefaultAsync(d => d.RentalId == rentalRecord.RentalRequestId);

            ViewBag.DocumentId = document?.Id;
            ViewBag.Mode = rentalRecord.ActualReturnDate == null ? "transaction" : "return";
            ViewBag.RentalRequest = rentalRecord.RentalRequest;

            return View(rentalRecord);
        }


        // GET: RentalRecords/CreateTransaction
        public IActionResult CreateTransaction(int rentalRequestId)
        {

            //do not allow the Unuthenticated users to enter this
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var request = _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .Include(r => r.Status)
                .FirstOrDefault(r => r.Id == rentalRequestId);

            if (request == null) return NotFound();

            var days = (request.ReturnDate.Date - request.StartDate.Date).Days + 1;
            var dailyRate = request.RentalPerDay ?? 0;
            var rentalFee = dailyRate * days;
            var deposit = Math.Round(dailyRate * 0.7M, 2);
            var total = rentalFee + deposit;

            ViewBag.Mode = "transaction";
            ViewBag.RentalRequest = request;

            var record = new RentalRecord
            {
                RentalRequestId = rentalRequestId,
                EquipmentName = request.Equipment?.Name,
                PickupDate = DateTime.Now,
                RentalFee = rentalFee,
                Deposit = deposit,
                TotalCost = total
            };

            return View("Create", record);
        }


        // POST: RentalRecords/CreateTransaction
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTransaction(RentalRecord rentalRecord)
        {

            //do not allow the Unuthenticated users to enter this
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }


            var request = await _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .FirstOrDefaultAsync(r => r.Id == rentalRecord.RentalRequestId);

            if (request == null) return NotFound();

            if (ModelState.IsValid)
            {
                // Step 1: Calculate fees
                var days = (request.ReturnDate.Date - request.StartDate.Date).Days + 1;
                var dailyRate = request.RentalPerDay ?? 0;
                var rentalFee = dailyRate * days;
                var deposit = Math.Round(dailyRate * 0.7M, 2);
                var total = rentalFee + deposit;

                rentalRecord.EquipmentName = request.Equipment?.Name;
                rentalRecord.PickupDate = DateTime.Now;
                rentalRecord.RentalFee = rentalFee;
                rentalRecord.Deposit = deposit;
                rentalRecord.TotalCost = total;
                rentalRecord.CreatedAt = DateTime.Now;

                // Step 2: Save rental record first to get its ID
                _context.RentalRecords.Add(rentalRecord);
                await _context.SaveChangesAsync();

                var Agreement = Request.Form.Files["Agreement"];

                // Step 3: Upload PDF agreement and store metadata
                if (Agreement != null && Agreement.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await Agreement.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    var uploadSuccess = await PdfManager.UploadPdfAndSaveToDatabase(
                        _context,
                        memoryStream,
                        Agreement.FileName,
                        Agreement.ContentType,
                        rentalRecord.RentalRequestId.Value
                    );

                    if (!uploadSuccess)
                    {
                        TempData["MessageText"] = "Rental created, but PDF upload failed.";
                        TempData["MessageType"] = "warning";
                    }
                }

                TempData["MessageText"] = "Rental transaction created successfully.";
                TempData["MessageType"] = "success";
                return RedirectToAction(nameof(Index));
            }

            // Repopulate form
            ViewBag.RentalRequest = request;
            ViewBag.ReturnConditionId = new SelectList(_context.ReturnConditionStatuses, "Id", "ConditionName", rentalRecord.ReturnConditionId);
            return View("Create", rentalRecord);
        }

        // GET: RentalRecords/CompleteReturn/5
        public async Task<IActionResult> CompleteReturn(int id)
        {

            //do not allow the Unuthenticated users to enter this
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var record = await _context.RentalRecords
                 .Include(r => r.RentalRequest)
                    .ThenInclude(r => r.Customer)
                 .Include(r => r.RentalRequest)
                    .ThenInclude(r => r.Equipment)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (record == null) return NotFound();

            ViewBag.Mode = "return"; // Important for conditional rendering in the shared view
            ViewBag.RentalRequest = record.RentalRequest;
            ViewBag.ReturnConditionId = new SelectList(_context.ReturnConditionStatuses, "Id", "ConditionName");

            return View("Create", record); // Use the same Create view
        }


        // POST: RentalRecords/CompleteReturn/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteReturn(int id, RentalRecord updated)
        {

            //do not allow the Unuthenticated users to enter this
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var record = await _context.RentalRecords.FindAsync(id);
            if (record == null) return NotFound();

            // Only update return-related fields
            record.ActualReturnDate = updated.ActualReturnDate;
            record.ReturnConditionId = updated.ReturnConditionId;
            record.LateReturnFees = updated.LateReturnFees;
            record.ExtraCharges = updated.ExtraCharges;
            record.ExtraChargeDescription = updated.ExtraChargeDescription;
            record.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: RentalRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {


            //do not allow the Unuthenticated users to enter this
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }


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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RentalRecord rentalRecord)
        {


            //do not allow the Unuthenticated users to enter this
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

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

        private bool RentalRecordExists(int id)
        {
            return (_context.RentalRecords?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
