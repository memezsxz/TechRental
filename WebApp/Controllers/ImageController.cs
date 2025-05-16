using Database.Persistence;
using Microsoft.AspNetCore.Mvc;
using Database.Core.Domain;
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
        public async Task<IActionResult> GetImage(int id)
        {
            var imageEntity = await _context.Images
                .FirstOrDefaultAsync(i => i.ImageId == id);

            if (imageEntity == null)
                return NotFound();

            string extension = "";
            if (!string.IsNullOrEmpty(imageEntity.ImageType))
            {
                var parts = imageEntity.ImageType.Split('/');
                if (parts.Length == 2)
                {
                    extension = "." + parts[1].ToLower();
                }
            }

            var fullKey = imageEntity.Guid.ToString();

            var stream = await S3Uploader.GetFileByGuidAsync(fullKey);

            if (stream == null)
                return NotFound();

            var contentType = imageEntity.ImageType ?? "application/octet-stream";
            return File(stream, contentType);
        }
    }
}
