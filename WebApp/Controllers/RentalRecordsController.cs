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
    public class RentalRecordsController : Controller
    {
        private readonly RentalDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RentalRecordsController(RentalDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: RentalRecords
        /// <summary>
        /// Displays a list of rental records with support for:
        /// - Role-based filtering (customer vs admin/manager)
        /// - Transaction vs Return toggle
        /// - Search by equipment name or request ID
        /// - Filtering by return condition (for returns)
        /// - Filtering by due status (for transactions)
        /// - Sorting and pagination
        /// </summary>
        public async Task<IActionResult> Index(string search, string sortBy, string status, string conditionFilter, string dueFilter, int page = 1, int pageSize = 10)
        {

            // Block unauthenticated users
            if (!User.Identity.IsAuthenticated) return View("Unauthorized"); // HTTP 401

            // Base query with related entities
            var query = _context.RentalRecords
                .Include(r => r.RentalRequest).ThenInclude(r => r.Customer)
                .Include(r => r.ReturnCondition)
                .AsQueryable();

            // Role-based filter: Customers only see their own records
            if (User.IsInRole(RoleConstants.Customer))
            {
                var loggedInEmail = User.Identity.Name;
                // Filter the data to only show this user's records
                query = query.Where(u => u.RentalRequest.Customer.Email == loggedInEmail);
            }

            // Search by Equipment Name or Request ID
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(r =>
                    r.EquipmentName.Contains(search) ||
                    r.RentalRequest.Id.ToString().Contains(search));
            }

            // Transaction / Return toggle
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (status.ToLower() == "transaction")
                {
                    query = query.Where(r => r.ActualReturnDate == null);

                    // Due Filter only applies to ongoing transactions
                    if (!string.IsNullOrWhiteSpace(dueFilter))
                    {
                        var today = DateTime.Today;

                        query = dueFilter switch
                        {
                            "overdue" => query.Where(r => r.RentalRequest.ReturnDate < today),
                            "today" => query.Where(r => r.RentalRequest.ReturnDate == today),
                            "upcoming" => query.Where(r => r.RentalRequest.ReturnDate > today),
                            _ => query
                        };
                    }
                }
                else if (status.ToLower() == "return")
                {
                    query = query.Where(r => r.ActualReturnDate != null);
                }
            }

            // Return Condition filter (for returns)
            if (!string.IsNullOrWhiteSpace(conditionFilter))
            {
                query = query.Where(r => r.ReturnCondition.ConditionName == conditionFilter);
            }

            // Sorting
            query = !string.IsNullOrWhiteSpace(sortBy) switch
            {
                true when sortBy == "date_asc" => query.OrderBy(r => r.PickupDate),
                true when sortBy == "date_desc" => query.OrderByDescending(r => r.PickupDate),
                _ => query.OrderByDescending(r => r.CreatedAt)
            };

            // Pagination
            var total = await query.CountAsync();
            var records = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // ViewBag values for UI state
            ViewBag.Search = search;
            ViewBag.SortBy = sortBy;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize);
            ViewBag.RecordStatus = status;
            ViewBag.ConditionFilter = conditionFilter;
            ViewBag.DueFilter = dueFilter;
            ViewBag.Conditions = await _context.ReturnConditionStatuses
                .Select(c => c.ConditionName)
                .Distinct()
                .ToListAsync();

            return View(records);
        }

        // GET: RentalRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!User.Identity.IsAuthenticated) return View("Unauthorized"); // HTTP 401
            if (id == null || _context.RentalRecords == null) return View("NotFound"); // HTTP 404

            var rentalRecord = await _context.RentalRecords
                .Include(r => r.RentalRequest)
                    .ThenInclude(r => r.Customer)
                .Include(r => r.RentalRequest)
                    .ThenInclude(r => r.Equipment)
                .Include(r => r.ReturnCondition)
                .Include(r => r.Payments)
                    .ThenInclude(p => p.PaymentMethod)
                .Include(r => r.Payments)
                    .ThenInclude(p => p.PaymentStatus)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rentalRecord == null) return View("NotFound"); // HTTP 404

            if (User.IsInRole(RoleConstants.Customer))
            {
                var loggedInEmail = User.Identity.Name;
                if (rentalRecord.RentalRequest.Customer.Email != loggedInEmail) return View("Forbidden"); // HTTP 403 Forbidden
            }

            var document = await _context.Documents
                .FirstOrDefaultAsync(d => d.RentalId == rentalRecord.RentalRequestId);

            ViewBag.DocumentId = document?.Id;
            ViewBag.Mode = rentalRecord.ActualReturnDate == null ? "transaction" : "return";

            return View(rentalRecord);
        }


        // GET: RentalRecords/CreateTransaction
        public IActionResult CreateTransaction(int rentalRequestId)
        {


            //do not allow the Unuthenticated users to enter this
            if (!User.Identity.IsAuthenticated) return View("Unauthorized"); // HTTP 401

            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager)) {
                return View("Forbidden");
            }

            var request = _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .Include(r => r.Status)
                .FirstOrDefault(r => r.Id == rentalRequestId);

            if (request == null) return View("NotFound"); // HTTP 404

            var days = (request.ReturnDate.Date - request.StartDate.Date).Days + 1;
            var dailyRate = request.RentalPerDay;
            var rentalFee = dailyRate * days;
            var deposit = Math.Round(dailyRate * 0.7M, 2);
            var total = rentalFee + deposit;

            ViewBag.Mode = "transaction";
            ViewBag.RentalRequest = request;
            ViewBag.PaymentMethodId = new SelectList(_context.PaymentMethods, "Id", "MethodName", 3); // default to Cash
            ViewBag.PaymentStatusId = new SelectList(_context.PaymentStatuses, "Id", "StatusName", 2); // default to Paid

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
            if (!User.Identity.IsAuthenticated) return View("Unauthorized"); // HTTP 401


            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager))
            {
                return View("Forbidden");
            }

            var request = await _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .FirstOrDefaultAsync(r => r.Id == rentalRecord.RentalRequestId);

            if (request == null) return View("NotFound"); // HTTP 404

            if (ModelState.IsValid)
            {
                // Step 1: Calculate fees
                var days = (request.ReturnDate.Date - request.StartDate.Date).Days + 1;
                var dailyRate = request.RentalPerDay;
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

                var payment = new Payment
                {
                    RentalRecordId = rentalRecord.Id,
                    Amount = rentalRecord.TotalCost,
                    PaymentMethodId = int.Parse(Request.Form["PaymentMethodId"]),
                    PaymentStatusId = 2, // Paid
                    PaymentDate = DateTime.Now
                };

                _context.Payments.Add(payment);
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
                        rentalRecord.RentalRequestId
                    );

                    if (!uploadSuccess)
                    {
                        TempData["MessageText"] = "Rental created, but PDF upload failed.";
                        TempData["MessageType"] = "warning";
                    }
                }

                TempData["MessageText"] = "Rental transaction created successfully.";
                TempData["MessageType"] = "success";
                return RedirectToAction(nameof(Index), new { status = "transaction" });
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
            if (!User.Identity.IsAuthenticated) return View("Unauthorized"); // HTTP 401
            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager))
            {
                return View("Forbidden");
            }

            var record = await _context.RentalRecords
                .Include(r => r.RentalRequest)
                    .ThenInclude(r => r.Customer)
                .Include(r => r.RentalRequest)
                    .ThenInclude(r => r.Equipment)
                .Include(r => r.Payments)
                    .ThenInclude(p => p.PaymentMethod)
                .Include(r => r.Payments)
                    .ThenInclude(p => p.PaymentStatus)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (record == null) return View("NotFound"); // HTTP 404

            ViewBag.Mode = "return"; // Important for conditional rendering in the shared view
            ViewBag.RentalRequest = record.RentalRequest;
            ViewBag.ReturnConditionId = new SelectList(_context.ReturnConditionStatuses, "Id", "ConditionName");
            ViewBag.PaymentMethodId = new SelectList(_context.PaymentMethods, "Id", "MethodName", 3); // default to Cash
            ViewBag.PaymentStatusId = new SelectList(_context.PaymentStatuses, "Id", "StatusName", 2); // default to Paid

            // Retrieve the uploaded agreement file from Document table (if available)
            var document = await _context.Documents
                .FirstOrDefaultAsync(d => d.RentalId == record.RentalRequestId);

            ViewBag.DocumentId = document?.Id;
            return View("Create", record); // Use the same Create view
        }


        // POST: RentalRecords/CompleteReturn/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteReturn(int id, RentalRecord updated)
        {
            //do not allow the Unuthenticated users to enter this
            if (!User.Identity.IsAuthenticated) return View("Unauthorized"); // HTTP 401

            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager))
            {
                return View("Forbidden");
            }

            var record = await _context.RentalRecords.FindAsync(id);
            if (record == null) return View("NotFound");

            // Only update return-related fields
            record.ActualReturnDate = updated.ActualReturnDate;
            record.ReturnConditionId = updated.ReturnConditionId;
            record.LateReturnFees = updated.LateReturnFees;
            record.ExtraCharges = updated.ExtraCharges;
            record.ExtraChargeDescription = updated.ExtraChargeDescription;
            record.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            await NotificationManager.CreateAsync(_context, record.RentalRequest.CustomerId, 4, "return", record.Id);
            return RedirectToAction(nameof(Index), new { status ="return" });
        }


        // GET: RentalRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //do not allow the Unuthenticated users to enter this
            if (!User.Identity.IsAuthenticated) return View("Unauthorized"); // HTTP 401
            if (id == null) return View("NotFound"); // HTTP 404

            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager))
            {
                return View("Forbidden");
            }

            var rentalRecord = await _context.RentalRecords
                .Include(r => r.RentalRequest)
                    .ThenInclude(r => r.Customer)
                .Include(r => r.RentalRequest)
                    .ThenInclude(r => r.Equipment)
                .Include(r => r.ReturnCondition)
                .Include(r => r.Payments)
                    .ThenInclude(p => p.PaymentMethod)
                .Include(r => r.Payments)
                    .ThenInclude(p => p.PaymentStatus)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (rentalRecord == null) return View("NotFound"); // HTTP 404

            var document = await _context.Documents
                .FirstOrDefaultAsync(d => d.RentalId == rentalRecord.RentalRequestId);

            ViewBag.DocumentId = document?.Id;

            return View(rentalRecord);
        }

        // POST: RentalRecords/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RentalRecord rentalRecord)
        {
            if (!User.Identity.IsAuthenticated) return View("Unauthorized"); // HTTP 401

            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager))
            {
                return View("Forbidden");
            }

            if (id != rentalRecord.Id) return View("NotFound"); // HTTP 404

            if (ModelState.IsValid)
            {
                try
                {
                    var Agreement = Request.Form.Files["Agreement"];

                    // Handle uploaded agreement
                    if (Agreement != null && Agreement.Length > 0)
                    {
                        // Delete existing agreement (if exists)
                        var existingDoc = await _context.Documents.FirstOrDefaultAsync(d => d.RentalId == rentalRecord.RentalRequestId);
                        if (existingDoc != null)
                        {
                            await PdfManager.DeletePdfFromDatabaseAndS3(_context, existingDoc.Id);
                        }

                        // Save new one
                        using var stream = Agreement.OpenReadStream();
                        var uploadSuccess = await PdfManager.UploadPdfAndSaveToDatabase(_context, stream, Agreement.FileName, Agreement.ContentType, rentalRecord.RentalRequestId);

                        if (!uploadSuccess)
                        {
                            TempData["MessageText"] = "PDF upload failed.";
                            TempData["MessageType"] = "warning";
                        }

                    }

                    TempData["MessageText"] = "Transaction Record updated successfully.";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("Details", new { id = rentalRecord.Id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    return View("InternalServerError"); // HTTP 500
                }
            }

            return RedirectToAction("Details", new { id = rentalRecord.Id });
        }

        public async Task<IActionResult> DeleteAgreement(int rentalId)
        {
            if (!User.Identity.IsAuthenticated) return View("Unauthorized"); // HTTP 401

            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager))
            {
                return View("Forbidden");
            }

            var document = await _context.Documents.FirstOrDefaultAsync(d => d.RentalId == rentalId);
            if (document == null)
            {
                TempData["MessageText"] = "No agreement found to delete.";
                TempData["MessageType"] = "warning";
                return RedirectToAction("Details", new { id = rentalId });
            }

            var result = await PdfManager.DeletePdfFromDatabaseAndS3(_context, document.Id);

            if (result)
            {
                TempData["MessageText"] = "Agreement deleted successfully.";
                TempData["MessageType"] = "success";
            }
            else
            {
                TempData["MessageText"] = "Failed to delete agreement.";
                TempData["MessageType"] = "error";
            }

            var record = await _context.RentalRecords.FirstOrDefaultAsync(r => r.RentalRequestId == rentalId);

            return RedirectToAction("Details", new { id = record.Id });
        }
    }
}
