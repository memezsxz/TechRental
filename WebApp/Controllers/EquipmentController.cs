using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database.Core.Domain;
using Database.Persistence;
using Helper;
using Microsoft.AspNetCore.Authorization;
using WebApp.Helpers;

namespace WebApp.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly RentalDBContext _context;

        public EquipmentController(RentalDBContext context)
        {
            _context = context;
        }

        // GET: Equipment
        public async Task<IActionResult> Index(string search, string category, string sortBy, int page = 1, int pageSize = 9)
        {

            var query = _context.Equipment
                .Include(e => e.Category)
                .Include(e => e.ConditionStatus)
                .Include(e => e.Image)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(e => e.Name.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(e => e.Category.Name == category);
            }

            switch (sortBy)
            {
                case "name_asc":
                    query = query.OrderBy(e => e.Name);
                    break;
                case "name_desc":
                    query = query.OrderByDescending(e => e.Name);
                    break;
                case "price_asc":
                    query = query.OrderBy(e => e.RentalPricePerDay);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(e => e.RentalPricePerDay);
                    break;
                default:
                    query = query.OrderBy(e => e.Name); // Default sort
                    break;
            }

            var totalItems = await query.CountAsync();

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            page = Math.Clamp(page, 1, Math.Max(1, totalPages));

            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            

            ViewBag.Categories = await _context.Categories
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Category = category;
            ViewBag.SortBy = sortBy;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return View(items);
        }


        // GET: Equipment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Equipment == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment
                    .Include(e => e.Image)
                    .Include(e => e.AvailabilityStatus)
                    .Include(e => e.ConditionStatus)
                    .Include(e => e.Feedbacks)
                        .ThenInclude(f => f.User)
                    .FirstOrDefaultAsync(e => e.Id == id);

            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // GET: Equipment/Create
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.Manager}")]
        public IActionResult Create()
        {
            if (!User.IsInRole(RoleConstants.Manager) && !User.IsInRole(RoleConstants.Admin))
            {
                return Forbid();
            }

                ViewData["AvailabilityStatusId"] = new SelectList(_context.EquipmentAvailabilityStatuses, "Id", "StatusName");
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
                ViewData["ConditionStatusId"] = new SelectList(_context.EquipmentConditionStatuses, "Id", "ConditionName");
                ViewData["ImageId"] = new SelectList(_context.Images, "ImageId", "ImageName");
                return View();
            
        }

        // POST: Equipment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.Manager}")]
        public async Task<IActionResult> Create(Equipment equipment)
        {
            if (!User.IsInRole(RoleConstants.Manager) && !User.IsInRole(RoleConstants.Admin))
            {
                return Forbid();
            }


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

                    //return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    TempData["MessageText"] = "An error occurred while saving the equipment.";
                    TempData["MessageType"] = "error";
                    return RedirectToAction(nameof(Details), new { id = equipment.Id });
                }
            }

            // Repopulate dropdowns
            ViewData["AvailabilityStatusId"] = new SelectList(_context.EquipmentAvailabilityStatuses, "Id", "StatusName", equipment.AvailabilityStatusId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", equipment.CategoryId);
            ViewData["ConditionStatusId"] = new SelectList(_context.EquipmentConditionStatuses, "Id", "ConditionName", equipment.ConditionStatusId);

            return View(equipment);
        }

        // GET: Equipment/Edit/5
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.Manager}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager))
            {
                return Forbid();
            }

            if (id == null || _context.Equipment == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }
            ViewData["AvailabilityStatusId"] = new SelectList(_context.EquipmentAvailabilityStatuses, "Id", "StatusName", equipment.AvailabilityStatusId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", equipment.CategoryId);
            ViewData["ConditionStatusId"] = new SelectList(_context.EquipmentConditionStatuses, "Id", "ConditionName", equipment.ConditionStatusId);
            ViewData["ImageId"] = new SelectList(_context.Images, "ImageId", "ImageName", equipment.ImageId);
            return View(equipment);
        }

        // POST: Equipment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.Manager}")]
        public async Task<IActionResult> Edit(int id, Equipment equipment)
        {

            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager))
            {
                return Forbid();
            }

            if (id != equipment.Id)
            {
                return NotFound();
            }

            var existingEquipment = await _context.Equipment.FindAsync(id);
            if (existingEquipment == null)
            {
                return NotFound();
            }

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
                    // Delete old image from S3 if needed
                    if (existingEquipment.ImageId.HasValue)
                    {
                        // await ImageManager.DeleteImageFromDatabaseAndS3(_context, existingEquipment.ImageId.Value);
                    }

                    existingEquipment.ImageId = uploadedImageId.Value;
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update properties manually
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

            // If ModelState is not valid
            ViewData["AvailabilityStatusId"] = new SelectList(_context.EquipmentAvailabilityStatuses, "Id", "StatusName", equipment.AvailabilityStatusId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", equipment.CategoryId);
            ViewData["ConditionStatusId"] = new SelectList(_context.EquipmentConditionStatuses, "Id", "ConditionName", equipment.ConditionStatusId);

            return View(equipment);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.Manager}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCheck(int id)
        {
            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager))
            {
                return Forbid();
            }

            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
                return Json(new { success = false, message = "Not found" });

            bool isReferenced = await _context.RentalRequests.AnyAsync(r => r.EquipmentId == id);

            if (isReferenced)
            {
                return Json(new
                {
                    requiresInactive = true,
                    message = "This equipment is in use. Do you want to mark it as inactive instead?",
                    setInactiveUrl = Url.Action("SetInactive", "Equipment", new { id })
                });
            }
            else
            {
                if (equipment.ImageId.HasValue)
                    await ImageManager.DeleteImageFromDatabaseAndS3(_context, equipment.ImageId.Value);

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


        [HttpPost]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.Manager}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> SetInactive(int id)
        {
            if (!User.IsInRole(RoleConstants.Admin) && !User.IsInRole(RoleConstants.Manager))
            {
                return Forbid();
            }

            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                return Json(new { success = false, message = "Equipment not found.", type = "error" });
            }

            equipment.IsActive = false;
            _context.Equipment.Update(equipment);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Equipment marked as inactive.", type = "success" });
        }


        //[HttpGet]
        //public async Task ConvertImageToWebpAndUpdateAsync(int id = 1)
        //{
        //    var imageRecord = await _context.Images.FirstOrDefaultAsync(i => i.ImageId == id);
        //    if (imageRecord == null) return;

        //    using var getResponse = await S3Uploader.GetFileByGuidAsync(imageRecord.Guid.ToString()!);
        //    using var originalImage = await SixLabors.ImageSharp.Image.LoadAsync(getResponse);

        //    // Convert to WebP
        //    using var webpStream = new MemoryStream();
        //    await originalImage.SaveAsync(webpStream, new WebpEncoder { Quality = 80 });
        //    webpStream.Position = 0;

        //    // Generate new filename and GUID
        //    var newGuid = Guid.NewGuid();
        //    var newFileName = Path.GetFileNameWithoutExtension(imageRecord.ImageName) + ".webp";

        //    // Upload to S3
        //    await S3Uploader.UploadFileAsync(webpStream, newGuid, ".webp");

        //    // Update DB record
        //    imageRecord.ImageName = newFileName;
        //    imageRecord.ImageType = "image/webp";
        //    imageRecord.Guid = newGuid;
        //    imageRecord.CreatedAt = DateTime.UtcNow;

        //    await _context.SaveChangesAsync();
        //}

    }
}
