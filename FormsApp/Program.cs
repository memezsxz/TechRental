using Database.Persistence;
using System.Windows.Forms;
using System.Threading.Tasks;
using Amazon.S3.Model;
using System.Windows.Forms;

namespace FormsApp
{
    internal static class Program
    {

        #region Main Run

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            ApplicationConfiguration.Initialize(); 
            Application.Run(new Login());
        }


        #endregion

        #region S3 Testing
        ///// <summary>
        /////  The main entry point for the application.
        ///// </summary>
        //[STAThread]
        //static async Task Main()
        //{
        //    // To customize application configuration such as set high DPI settings or default font,
        //    // see https://aka.ms/applicationconfiguration.

        //    using (var context = new RentalDBContext())
        //    {
        //        try
        //        {
        //            //var bb = new List<byte> { 1, 2, 3 };
        //            //var x = S3Manager.DownloadBinaryAsync("bush.jpg").Result;
        //            //var y = S3Manager.UploadBinaryAsync(bb.ToArray(), "test").Result;

        //            //uplad image to S3
        //            Console.WriteLine("🚀 Starting S3 Upload...");
        //            await S3Uploader.UploadImagesAsync();
        //            Console.WriteLine("✅ Upload process completed.");

        //            //Retrive image from S3
        //            //Image img = await S3Uploader.GetImageByGuidAsync("0e78757b-8ad9-4837-8a29-645f7706f0c3");

        //            //if (img != null)
        //            //{
        //            //    img.Save(@"C:\Users\Ruqay\Downloads\fetched.png", System.Drawing.Imaging.ImageFormat.Png);
        //            //    Console.WriteLine("✅ Image saved as fetched.png");
        //            //}

        //            context.Database.EnsureCreated();
        //            Console.WriteLine("Database connection successful!");

        //            ApplicationConfiguration.Initialize();
        //            Application.Run(new Home());
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Error: {ex.Message}");
        //        }
        //    }
        //}


        #endregion

    }
}