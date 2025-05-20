using Database.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers
{
    public class ImageController : Controller
    {
        private readonly RentalDBContext _context;

        public ImageController(RentalDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        /// <summary>
        /// Retrieves and returns an image file from the S3 storage by its ID.
        /// 
        /// Features:
        /// - Fetches image metadata from the database
        /// - Retrieves the corresponding file stream from S3 storage using the stored GUID
        /// - Returns the file with the appropriate content type
        /// 
        /// Returns a "NotFound" view if the image metadata or the file stream does not exist.
        /// </summary>
        /// <param name="id">The unique identifier (ImageId) of the image to retrieve</param>
        /// <returns>The image file as a stream with appropriate MIME type, or a NotFound view</returns>
        public async Task<IActionResult> GetImage(int id)
        {
            // Attempt to retrieve the image metadata from the database using the provided ID
            var imageEntity = await _context.Images.FirstOrDefaultAsync(i => i.ImageId == id);
            if (imageEntity == null)
            {
                // Return a "NotFound" view if no matching record is found
                return View("NotFound");
            }

            // Retrieve the file stream from the S3 storage using the stored GUID
            var stream = await S3Uploader.GetFileByGuidAsync(imageEntity.Guid.ToString());
            if (stream == null)
            {
                // Return a "NotFound" view if the file doesn't exist in S3
                return View("NotFound");
            }

            // Use the stored MIME type if available, otherwise default to binary stream
            var contentType = imageEntity.ImageType ?? "application/octet-stream";

            // Return the image file as a FileStreamResult with the appropriate content type
            return File(stream, contentType);
        }
    }
}
