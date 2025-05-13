using Database.Persistence;
using Database.Core.Domain;

namespace Helper
{
    public static class ImageManager
    {
        /// <summary>
        /// Uploads an image to S3 and saves metadata to the database using DbContext.
        /// </summary>
        /// <param name="context">The RentalDBContext for database access.</param>
        /// <param name="fileStream">The image file stream.</param>
        /// <param name="fileName">The original name of the file.</param>
        /// <param name="contentType">The MIME type of the file (e.g., "image/png").</param>
        /// <returns>The ID of the saved image if successful; otherwise, null.</returns>
        public static Task<int?> UploadImageAndSaveToDatabase(RentalDBContext context, Stream fileStream, string fileName, string contentType) =>
            UploadAsync(
                image => { context.Images.Add(image); return Task.CompletedTask; },
                () => context.SaveChangesAsync(),
                fileStream,
                fileName,
                contentType
            );

        /// <summary>
        /// Uploads an image to S3 and saves metadata to the database using a UnitOfWork abstraction.
        /// </summary>
        /// <param name="unitOfWork">The UnitOfWork instance for repository access.</param>
        /// <param name="fileStream">The image file stream.</param>
        /// <param name="fileName">The original name of the file.</param>
        /// <param name="contentType">The MIME type of the file (e.g., "image/jpeg").</param>
        /// <returns>The ID of the saved image if successful; otherwise, null.</returns>
        public static Task<int?> UploadImageAndSaveToDatabase(UnitOfWork unitOfWork, Stream fileStream, string fileName, string contentType) =>
            UploadAsync(
                image => { unitOfWork.Images.Add(image); return Task.CompletedTask; },
                () => unitOfWork.SaveChangesAsync(),
                fileStream,
                fileName,
                contentType
            );

        /// <summary>
        /// Deletes an image from both the database and S3 storage.
        /// </summary>
        /// <param name="context">The RentalDBContext for database access.</param>
        /// <param name="imageId">The ID of the image to delete.</param>
        /// <returns>True if deletion was successful; false if the image was not found or S3 deletion failed.</returns>
        public static async Task<bool> DeleteImageFromDatabaseAndS3(RentalDBContext context, int imageId)
        {
            var image = await context.Images.FindAsync(imageId);
            if (image == null) return false;

            var deletedFromS3 = await S3Uploader.DeleteFileAsync((Guid)image.Guid);
            if (!deletedFromS3) return false;

            context.Images.Remove(image);
            await context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Handles shared logic for uploading an image to S3 and saving it to the database.
        /// </summary>
        /// <param name="addImageFunc">A delegate to add the image to the desired repository or context.</param>
        /// <param name="saveChangesFunc">A delegate to save changes to the database.</param>
        /// <param name="fileStream">The image file stream.</param>
        /// <param name="fileName">The original file name.</param>
        /// <param name="contentType">The MIME content type of the file.</param>
        /// <returns>The ID of the image if the operation is successful; otherwise, null.</returns>
        private static async Task<int?> UploadAsync(Func<Image, Task> addImageFunc, Func<Task> saveChangesFunc, Stream fileStream, string fileName, string contentType)
        {
            var extension = GetExtensionFromContentType(contentType);
            var guid = Guid.NewGuid();
            var status = await S3Uploader.UploadFileAsync(fileStream, guid, extension);

            if (status == null) return null;

            var image = new Image
            {
                ImageName = fileName,
                ImageType = contentType,
                Guid = guid,
                CreatedAt = DateTime.UtcNow
            };

            await addImageFunc(image);
            await saveChangesFunc();

            return image.ImageId;
        }

        /// <summary>
        /// Extracts the file extension from a MIME content type.
        /// </summary>
        /// <param name="contentType">The content type string (e.g., "image/png").</param>
        /// <returns>The corresponding file extension with a dot prefix (e.g., ".png"), or an empty string if parsing fails.</returns>
        private static string GetExtensionFromContentType(string contentType)
        {
            var parts = contentType.Split('/');
            return parts.Length == 2 ? "." + parts[1].ToLower() : "";
        }
    }
}
