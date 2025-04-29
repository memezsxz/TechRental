using Database.Persistence;
using Database.Core.Domain;

namespace Helper
{
    public static class ImageManager
    {
        public static async Task<int?> UploadImageAndSaveToDatabase(RentalDBContext context, Stream fileStream, string fileName, string contentType)
        {
            var extension = "";
            var parts = contentType.Split('/');
            if (parts.Length == 2)
            {
                extension = "." + parts[1].ToLower();
            }

            var guid = Guid.NewGuid();
            var status = await S3Uploader.UploadFileAsync(fileStream, guid, extension);

            if (status == null)
                return null; // Upload failed

            var image = new Image
            {
                ImageName = fileName,
                ImageType = contentType,
                Guid = guid,
                CreatedAt = DateTime.UtcNow
            };

            context.Images.Add(image);
            await context.SaveChangesAsync();

            return image.ImageId;
        }

        public static async Task<bool> DeleteImageFromDatabaseAndS3(RentalDBContext context, int imageId)
        {
            var image = await context.Images.FindAsync(imageId);
            if (image == null)
                return false; // Image not found

            var deletedFromS3 = await S3Uploader.DeleteFileAsync((Guid)image.Guid);
            if (!deletedFromS3)
                return false; // S3 deletion failed

            context.Images.Remove(image);
            await context.SaveChangesAsync();
            return true;
        }
    }

}
