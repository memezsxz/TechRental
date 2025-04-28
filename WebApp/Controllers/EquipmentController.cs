using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database.Core.Domain;
using Database.Persistence;
using Helper;

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
        public async Task<IActionResult> Index()
        {
            var rentalDBContext = _context.Equipment
                .Include(e => e.AvailabilityStatus)
                .Include(e => e.Category)
                .Include(e => e.ConditionStatus)
                .Include(e => e.Image);

            var equipmentList = await rentalDBContext.ToListAsync();

            return View(equipmentList);
        }


        // GET: Equipment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Equipment == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment
                .Include(e => e.AvailabilityStatus)
                .Include(e => e.Category)
                .Include(e => e.ConditionStatus)
                .Include(e => e.Image)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // GET: Equipment/Create
        public IActionResult Create()
        {
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
        public async Task<IActionResult> Create(Equipment equipment)
        {
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
                if (uploadedImageId.HasValue)
                {
                    equipment.ImageId = uploadedImageId.Value;
                }

                _context.Add(equipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns
            ViewData["AvailabilityStatusId"] = new SelectList(_context.EquipmentAvailabilityStatuses, "Id", "StatusName", equipment.AvailabilityStatusId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", equipment.CategoryId);
            ViewData["ConditionStatusId"] = new SelectList(_context.EquipmentConditionStatuses, "Id", "ConditionName", equipment.ConditionStatusId);

            return View(equipment);
        }

        // GET: Equipment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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
        public async Task<IActionResult> Edit(int id, Equipment equipment)
        {
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
                        //await ImageManager.DeleteImageFromDatabaseAndS3(_context, existingEquipment.ImageId.Value);
                    }

                    existingEquipment.ImageId = uploadedImageId.Value;
                }
            }

            if (ModelState.IsValid)
            {
                // Update properties manually (safe updating)
                existingEquipment.Name = equipment.Name;
                existingEquipment.Description = equipment.Description;
                existingEquipment.RentalPricePerDay = equipment.RentalPricePerDay;
                existingEquipment.CategoryId = equipment.CategoryId;
                existingEquipment.ConditionStatusId = equipment.ConditionStatusId;
                existingEquipment.AvailabilityStatusId = equipment.AvailabilityStatusId;
                existingEquipment.UpdatedAt = DateTime.UtcNow;

                _context.Update(existingEquipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AvailabilityStatusId"] = new SelectList(_context.EquipmentAvailabilityStatuses, "Id", "StatusName", equipment.AvailabilityStatusId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", equipment.CategoryId);
            ViewData["ConditionStatusId"] = new SelectList(_context.EquipmentConditionStatuses, "Id", "ConditionName", equipment.ConditionStatusId);

            return View(equipment);
        }

        // POST: Equipment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment != null)
            {
                _context.Equipment.Remove(equipment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EquipmentExists(int id)
        {
          return (_context.Equipment?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
