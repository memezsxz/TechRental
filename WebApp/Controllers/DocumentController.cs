using Database.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers
{
    public class DocumentController : Controller
    {
        private readonly RentalDBContext _context;

        public DocumentController(RentalDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Generates and downloads a rental agreement PDF for a given Rental Request.
        /// 
        /// Features:
        /// - Fetches the full Rental Request including Customer and Equipment
        /// - Generates a MemoryStream PDF from request data
        /// - Returns the file as a downloadable PDF
        /// 
        /// Used when printing or sharing the rental transaction summary.
        /// </summary>
        /// <param name="rentalRequestId">The ID of the Rental Request to generate the agreement for</param>
        /// <returns>A downloadable PDF file containing rental transaction details, or NotFound if the request doesn't exist</returns>
        [HttpGet]
        public async Task<IActionResult> DownloadAgreement(int rentalRequestId)
        {
            // Retrieve the rental request and include customer and equipment info
            var request = await _context.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .FirstOrDefaultAsync(r => r.Id == rentalRequestId);

            if (request == null) return View("NotFound"); // Return 404 if rental request is missing

            // Generate the rental agreement as a MemoryStream
            var stream = AgreementGenerator.GeneratePdf(request);

            // Generate a descriptive file name
            var fileName = $"Transaction_Record#{rentalRequestId}_{request.CustomerId}.pdf";

            // Return the generated PDF as a downloadable file
            return File(stream.ToArray(), "application/pdf", fileName);
        }

        /// <summary>
        /// Streams a stored PDF document directly to the browser for viewing.
        /// 
        /// Features:
        /// - Finds the document by ID in the database
        /// - Retrieves the file from S3 using its GUID
        /// - Displays the file in the browser
        /// 
        /// Used when previewing agreements or attachments without download.
        /// </summary>
        /// <param name="id">The ID of the stored Document record</param>
        /// <returns>PDF file streamed inline for browser view, or NotFound if missing</returns>
        [HttpGet]
        public async Task<IActionResult> GetPdf(int id)
        {
            // Lookup the document in the database
            var doc = await _context.Documents.FindAsync(id);
            if (doc == null) return View("NotFound"); // Return 404 if not found

            // Retrieve the file stream from S3 using its GUID
            var stream = await S3Uploader.GetFileByGuidAsync(doc.Guid.ToString());
            if (stream == null) return View("NotFound"); // Return 404 if file missing in S3

            // Return the PDF file to be displayed in browser
            return File(stream, "application/pdf");
        }

        /// <summary>
        /// Downloads a stored PDF document from S3.
        /// 
        /// Features:
        /// - Finds the document in the database
        /// - Retrieves the file stream from S3
        /// - Returns the PDF file with download prompt
        /// 
        /// Used when the user explicitly requests to save a file.
        /// </summary>
        /// <param name="id">The ID of the stored Document record</param>
        /// <returns>PDF file as a forced download, or NotFound if missing</returns>
        [HttpGet]
        public async Task<IActionResult> DownloadPdf(int id)
        {
            // Lookup the document in the database
            var doc = await _context.Documents.FindAsync(id);
            if (doc == null) return View("NotFound"); // Return 404 if document not found

            // Retrieve the file stream from S3 using its GUID
            var stream = await S3Uploader.GetFileByGuidAsync(doc.Guid.ToString());
            if (stream == null) return View("NotFound"); // Return 404 if file missing in S3

            // Return the PDF file with a download prompt
            return File(stream, "application/pdf", "Agreement.pdf");
        }
    }
}
