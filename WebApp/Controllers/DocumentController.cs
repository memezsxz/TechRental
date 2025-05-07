using Database.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Helper;

namespace WebApp.Controllers
{
    public class DocumentController : Controller
    {
        private readonly RentalDBContext _context;

        public DocumentController(RentalDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> DownloadAgreement(int rentalRequestId)
        {
            var request = await _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .FirstOrDefaultAsync(r => r.Id == rentalRequestId);

            if (request == null) return NotFound();

            var stream = AgreementGenerator.GeneratePdf(request); // this method returns a MemoryStream
            var fileName = $"Transaction_Record#{rentalRequestId}_{request.CustomerId}.pdf";
            return File(stream.ToArray(), "application/pdf", fileName);
        }
    }
}
