using Database.Core.Domain;
using Database.Persistence;

namespace Helper
{
    public static class PdfManager
    {
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
                Guid = guid, // Store GUID as byte[]
                CreatedAt = DateTime.UtcNow
            };

            context.Documents.Add(document);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
