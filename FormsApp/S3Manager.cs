//using Amazon;
//using Amazon.S3;
//using Amazon.S3.Model;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading.Tasks;

//public static class S3Manager
//{
//    //use : var xg = Guid.NewGuid(); to generate a new GUID
//    // *** IMPORTANT:  Replace with your actual AWS credentials and bucket name ***
//    private const string AccessKey = "AKIAVRUVVGCYKW2MUG4V";
//    private const string SecretKey = "OJSpVFOB1rjqiHqPqmaHoa4KVaZNa5Ac7lk58wM3";
//    private const string BucketName = "tech-rent-files"; // Example: "my-unique-bucket-name"
//    private static readonly RegionEndpoint RegionEndpoint = RegionEndpoint.MESouth1;

//    private static readonly IAmazonS3 S3Client = InitializeS3Client(); // Static instance for reuse

//    private static IAmazonS3 InitializeS3Client()
//    {
//        // Configure AWS credentials and region (no environment variables or config files used here).
//        var config = new AmazonS3Config
//        {
//            RegionEndpoint = RegionEndpoint
//        };

//        return new AmazonS3Client(AccessKey, SecretKey, config);
//    }

//    // -------------------  High-Level Methods ------------------------

//    public static async Task<bool> UploadBinaryAsync(byte[] data, string key)
//    {
//        if (data == null || data.Length == 0)
//        {
//            Console.WriteLine($"Error: No data provided for upload to s3://{BucketName}/{key}");
//            return false;
//        }

//        try
//        {
//            var request = new PutObjectRequest
//            {
//                BucketName = BucketName,
//                Key = key,
//                InputStream = new MemoryStream(data),
//                ContentType = "application/octet-stream" // Generic binary content type
//            };

//            PutObjectResponse response = await S3Client.PutObjectAsync(request);

//            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
//            {
//                Console.WriteLine($"Successfully uploaded binary data to s3://{BucketName}/{key}");
//                return true;
//            }
//            else
//            {
//                Console.WriteLine($"Error uploading binary data to s3://{BucketName}/{key}: {response.HttpStatusCode}");
//                return false;
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Error uploading binary data to s3://{BucketName}/{key}: {ex.Message}");
//            return false;
//        }
//    }


//    public static async Task<byte[]> DownloadBinaryAsync(string key)
//    {
//        try
//        {
//            var request = new GetObjectRequest
//            {
//                BucketName = BucketName,
//                Key = key
//            };

//            using (GetObjectResponse response = await S3Client.GetObjectAsync(request))
//            {
//                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
//                {
//                    using (Stream responseStream = response.ResponseStream)
//                    {
//                        using (MemoryStream memoryStream = new MemoryStream())
//                        {
//                            await responseStream.CopyToAsync(memoryStream);
//                            return memoryStream.ToArray();
//                        }
//                    }
//                }
//                else
//                {
//                    Console.WriteLine($"Error downloading s3://{BucketName}/{key}: {response.HttpStatusCode}");
//                    return null;
//                }
//            }
//        }
//        catch (AmazonS3Exception ex)
//        {
//            Console.WriteLine($"Error downloading s3://{BucketName}/{key}: {ex.Message} - {ex.StatusCode}");
//            return null;
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Error downloading s3://{BucketName}/{key}: {ex.Message}");
//            return null;
//        }
//    }


//    public static async Task<bool> DeleteObjectAsync(string key)
//    {
//        try
//        {
//            var request = new DeleteObjectRequest
//            {
//                BucketName = BucketName,
//                Key = key
//            };

//            DeleteObjectResponse response = await S3Client.DeleteObjectAsync(request);

//            if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
//            {
//                Console.WriteLine($"Successfully deleted s3://{BucketName}/{key}");
//                return true;
//            }
//            else
//            {
//                Console.WriteLine($"Error deleting s3://{BucketName}/{key}: {response.HttpStatusCode}");
//                return false;
//            }
//        }
//        catch (AmazonS3Exception ex)
//        {
//            Console.WriteLine($"Error deleting s3://{BucketName}/{key}: {ex.Message} - {ex.StatusCode}");
//            return false;
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Error deleting s3://{BucketName}/{key}: {ex.Message}");
//            return false;
//        }
//    }

//}