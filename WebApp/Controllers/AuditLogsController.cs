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
    public class AuditLogsController : Controller
    {
        private readonly RentalDBContext _context;

        public AuditLogsController(RentalDBContext context)
        {
            _context = context;
        }

        // GET: AuditLogs
        public async Task<IActionResult> Index()
        {
            var rentalDBContext = _context.AuditLogs.Include(a => a.User);
            return View(await rentalDBContext.ToListAsync());
        }
    }
}
