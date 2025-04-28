using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

public static class S3Uploader
{
    // Set up the AmazonS3Client with AWS credentials and region  
    private static readonly AmazonS3Client _s3Client;
    private const string _bucketName = "tech-rent-files";  // S3 Bucket name  

    // Static constructor to initialize the static fields  
    static S3Uploader()
    {
        var credentials = new Amazon.Runtime.BasicAWSCredentials("AKIAVRUVVGCYKW2MUG4V", "OJSpVFOB1rjqiHqPqmaHoa4KVaZNa5Ac7lk58wM3");
        _s3Client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.MESouth1);
    }

    // Method to upload any file to S3  
    public static async Task<string> UploadFileAsync(Stream fileStream, string fileName, string fileExtension)
    {
        string guid = Guid.NewGuid().ToString(); // Generate GUID for the file  
        string key = $"{guid}{fileExtension}"; // Use the GUID as the filename on S3  

        try
        {
            // Create a TransferUtility object  
            var transferUtility = new TransferUtility(_s3Client);

            // Create a request for uploading the file  
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = fileStream,
                Key = key,
                BucketName = _bucketName,
                ContentType = GetContentType(fileExtension) // Set content type based on the file extension  
            };

            // Upload the file to S3  
            await transferUtility.UploadAsync(uploadRequest);

            Console.WriteLine($"✅ File uploaded successfully with GUID: {guid}");
            return guid; // Return the GUID for saving in your database  
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error uploading file: {ex.Message}");
            return null;
        }
    }

    // Method to retrieve a file from S3 using its GUID  
    public async static Task<Stream> GetFileByGuidAsync(string guid)
    {
        string key = $"{guid}";  // Assuming files are saved with the GUID as the name  

        try
        {
            // Create a request to get the object (file) from S3  
            var getRequest = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            // Retrieve the object (file) from S3  
            var response = await _s3Client.GetObjectAsync(getRequest);
            var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;  // Reset the stream position to the beginning  

            Console.WriteLine($"✅ File retrieved successfully for GUID: {guid}");
            return memoryStream;  // Return the file as a stream  
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error retrieving file: {ex.Message}");
            return null;
        }
    }

    public static async Task<string> GeneratePreSignedURL(string guid)
    {
        string key = guid;  // Key is the GUID filename

        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = key,
            Expires = DateTime.UtcNow.AddMinutes(60) // URL valid for 60 minutes
        };

        string url = _s3Client.GetPreSignedURL(request);
        return url;
    }

    // Helper method to return the appropriate MIME type based on file extension  
    private static string GetContentType(string extension)
    {
        switch (extension.ToLower())
        {
            case ".png":
                return "image/png";
            case ".jpg":
            case ".jpeg":
                return "image/jpeg";
            case ".pdf":
                return "application/pdf";
            case ".docx":
                return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            case ".txt":
                return "text/plain";
            default:
                return "application/octet-stream";  // Default content type for unknown files  
        }
    }
}
