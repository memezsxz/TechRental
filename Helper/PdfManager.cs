using Database.Core.Domain;
using Database.Persistence;

namespace Helper
{
    /// <summary>
    /// Provides functionality for uploading and deleting PDF documents to/from S3 and the database.
    /// </summary>
    public static class PdfManager
    {
        /// <summary>
        /// Uploads a PDF file to S3 and saves its metadata to the database.
        /// </summary>
        /// <param name="context">The RentalDBContext used to access the database.</param>
        /// <param name="fileStream">The stream of the uploaded PDF file.</param>
        /// <param name="fileName">The original file name of the PDF.</param>
        /// <param name="contentType">The MIME type of the file (must be "application/pdf").</param>
        /// <param name="rentalRequestId">The ID of the related RentalRequest.</param>
        /// <returns>True if the upload and save were successful; otherwise, false.</returns>
        /// <remarks>
        /// This method validates that the uploaded file is a PDF,
        /// uploads it to the configured S3 bucket, and stores metadata
        /// including a generated GUID and timestamp in the Documents table.
        /// </remarks>
        public static async Task<bool> UploadPdfAndSaveToDatabase(RentalDBContext context, Stream fileStream, string fileName, string contentType, int rentalRequestId)
        {
            if (contentType.ToLower() != "application/pdf")
                return false;

            var guid = Guid.NewGuid();
            var extension = ".pdf";

            var status = await S3Uploader.UploadFileAsync(fileStream, guid, extension);
            if (status == null)
                return false;

            var document = new Document
            {
                RentalId = rentalRequestId,
                FileName = fileName,
                FileType = contentType,
                Guid = guid,
                CreatedAt = DateTime.UtcNow
            };

            context.Documents.Add(document);
            await context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Deletes a PDF document from both the database and S3 storage.
        /// </summary>
        /// <param name="context">The RentalDBContext used to access the database.</param>
        /// <param name="docId">The ID of the document to delete.</param>
        /// <returns>True if the document was successfully deleted; otherwise, false.</returns>
        /// <remarks>
        /// This method attempts to find the document by ID,
        /// delete the associated file from S3 using the stored GUID,
        /// and remove the database record if successful.
        /// </remarks>
        public static async Task<bool> DeletePdfFromDatabaseAndS3(RentalDBContext context, int docId)
        {
            var doc = await context.Documents.FindAsync(docId);
            if (doc == null || !doc.Guid.HasValue)
                return false;

            var deleted = await S3Uploader.DeleteFileAsync(doc.Guid.Value);
            if (!deleted)
                return false;

            context.Documents.Remove(doc);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
