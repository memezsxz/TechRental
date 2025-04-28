using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database.Core.Domain;
using Database.Persistence;

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

        public async Task<IActionResult> GetImage(int id)
        {
            var equipment = await _context.Equipment
                .Include(e => e.Image)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (equipment?.Image == null || !equipment.Image.Guid.HasValue)
                return NotFound();

            string guid = equipment.Image.Guid.Value.ToString();
            string extension = "";

            if (!string.IsNullOrEmpty(equipment.Image.ImageType))
            {
                if (equipment.Image.ImageType.ToLower() == "image/png")
                    extension = ".png";
                else if (equipment.Image.ImageType.ToLower() == "image/jpeg")
                    extension = ".jpg"; // or .jpeg
            }

            var fullKey = guid + extension;

            var stream = await S3Uploader.GetFileByGuidAsync(fullKey);
            if (stream == null)
                return NotFound();

            var contentType = equipment.Image.ImageType ?? "application/octet-stream";
            return File(stream, contentType);
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
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,RentalPricePerDay,AvailabilityStatusId,ConditionStatusId,CategoryId,IsActive,CreatedAt,UpdatedAt,ImageId")] Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(equipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AvailabilityStatusId"] = new SelectList(_context.EquipmentAvailabilityStatuses, "Id", "StatusName", equipment.AvailabilityStatusId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", equipment.CategoryId);
            ViewData["ConditionStatusId"] = new SelectList(_context.EquipmentConditionStatuses, "Id", "ConditionName", equipment.ConditionStatusId);
            ViewData["ImageId"] = new SelectList(_context.Images, "ImageId", "ImageName", equipment.ImageId);
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
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,RentalPricePerDay,AvailabilityStatusId,ConditionStatusId,CategoryId,IsActive,CreatedAt,UpdatedAt,ImageId")] Equipment equipment)
        {
            if (id != equipment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipmentExists(equipment.Id))
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
            ViewData["AvailabilityStatusId"] = new SelectList(_context.EquipmentAvailabilityStatuses, "Id", "StatusName", equipment.AvailabilityStatusId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", equipment.CategoryId);
            ViewData["ConditionStatusId"] = new SelectList(_context.EquipmentConditionStatuses, "Id", "ConditionName", equipment.ConditionStatusId);
            ViewData["ImageId"] = new SelectList(_context.Images, "ImageId", "ImageName", equipment.ImageId);
            return View(equipment);
        }

        // GET: Equipment/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Equipment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Equipment == null)
            {
                return Problem("Entity set 'RentalDBContext.Equipment'  is null.");
            }
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment != null)
            {
                _context.Equipment.Remove(equipment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipmentExists(int id)
        {
          return (_context.Equipment?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
