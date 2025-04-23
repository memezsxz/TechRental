using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

public class S3Uploader
{
    // ✅ CONFIGURATION
    private const string bucketName = "tech-rent-files";  // S3 bucket
    private const string folderPath = @"C:\Users\Ruqay\Downloads\Mageradmin"; // local .png image path
    private const string outputSqlFile = @"C:\Users\Ruqay\Downloads\uuid_insert.sql"; // output .sql file
    private static readonly RegionEndpoint bucketRegion = RegionEndpoint.MESouth1;

    private static readonly AmazonS3Client s3Client = new AmazonS3Client(
        "AKIAVRUVVGCYKW2MUG4V", "OJSpVFOB1rjqiHqPqmaHoa4KVaZNa5Ac7lk58wM3",
        bucketRegion
    );

    public static async Task UploadImagesAsync()
    {
        var credentials = new Amazon.Runtime.BasicAWSCredentials("AKIAVRUVVGCYKW2MUG4V", "OJSpVFOB1rjqiHqPqmaHoa4KVaZNa5Ac7lk58wM3");
        using var s3Client = new AmazonS3Client(credentials, bucketRegion);
        var transferUtility = new TransferUtility(s3Client);

        var imageFiles = Directory.GetFiles(folderPath, "*.png").OrderBy(name => name).ToList();

        using var writer = new StreamWriter(outputSqlFile);

        foreach (var filePath in imageFiles)
        {
            string originalName = Path.GetFileName(filePath);
            string uuid = Guid.NewGuid().ToString();
            string s3Key = uuid + ".png";

            try
            {
                await transferUtility.UploadAsync(filePath, bucketName, s3Key);
                Console.WriteLine($"✅ Uploaded: {originalName} → {uuid}.png");

                writer.WriteLine($"INSERT INTO Image (image_name, image_type, guid) VALUES ('{originalName}', 'image/png', '{uuid}');");
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine($"❌ Failed to upload {originalName}: {e.Message}");
            }
        }

        Console.WriteLine($"\n✅ SQL insert statements saved to: {outputSqlFile}");
    }

    public static async Task<string> UploadImageAsync(Image image)
    {
        string uuid = Guid.NewGuid().ToString();
        string key = $"{uuid}.png";

        try
        {
            using var memoryStream = new MemoryStream();
            image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            memoryStream.Position = 0;

            var request = new TransferUtilityUploadRequest
            {
                InputStream = memoryStream,
                Key = key,
                BucketName = bucketName,
                ContentType = "image/png"
            };

            var transferUtility = new TransferUtility(s3Client);
            await transferUtility.UploadAsync(request);

            Console.WriteLine($"✅ Image uploaded as {key}");
            Console.WriteLine($"INSERT INTO Image (image_name, image_type, guid) VALUES ('{key}', 'image/png', '{uuid}');");

            return uuid;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Failed to upload image: {ex.Message}");
            return null;
        }
    }


    public static async Task<Image> GetImageByGuidAsync(string guid)
    {
        string key = $"{guid}.png";

        try
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = key
            };

            using var response = await s3Client.GetObjectAsync(request);
            using var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            Image image = Image.FromStream(memoryStream);
            Console.WriteLine($"✅ Image loaded for GUID: {guid}");

            return image;
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"❌ Failed to fetch image for GUID {guid}: {ex.Message}");
            return null;
        }
    }

}
