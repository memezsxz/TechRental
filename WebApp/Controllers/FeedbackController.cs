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
    public class FeedbackController : Controller
    {
        private readonly RentalDBContext _context;

        public FeedbackController(RentalDBContext context)
        {
            _context = context;
        }

        // Fix for CS0266, CS1662, and CS8619: Ensure nullability and type conversion are handled properly.
        public async Task<IActionResult> Index(int id, string status = "Unhidden")
        {
            ViewBag.EquipmentId = id;
            ViewBag.Status = status;

            var feedbacks = _context.Feedbacks
                .Include(f => f.User)
                .Include(f => f.Equipment)
                .Where(f => f.EquipmentId == id && (status == "Hidden" ? f.IsHidden == true : f.IsHidden == false));

            return View(await feedbacks.ToListAsync());
        }

        // GET: Feedback/Create
        public IActionResult Create()
        {
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Feedback/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Note,Rate,TimeDate,UserId,EquipmentId,IsHidden,CreatedAt,UpdatedAt")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feedback);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EquipmentId"] = new SelectList(_context.Equipment, "Id", "Name", feedback.EquipmentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", feedback.UserId);
            return View(feedback);
        }

        // POST: Feedback/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                TempData["MessageType"] = "error";
                TempData["MessageText"] = "Feedback not found.";
                return RedirectToAction("Index", new { id = feedback?.EquipmentId, status = feedback?.IsHidden == true ? "Hidden" : "Unhidden" });
            }

            // Toggle the hidden status
            feedback.IsHidden = !feedback.IsHidden;
            feedback.UpdatedAt = DateTime.UtcNow;

            _context.Update(feedback);
            await _context.SaveChangesAsync();

            TempData["MessageType"] = "success";
            TempData["MessageText"] = feedback.IsHidden == true ? "Feedback successfully hidden." : "Feedback successfully unhidden.";

            return RedirectToAction("Index", new { id = feedback.EquipmentId, status = feedback.IsHidden == true ? "Hidden" : "Unhidden" });
        }
    }
}
