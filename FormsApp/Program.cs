using Database.Persistence;
using System.Windows.Forms;

namespace FormsApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            using (var context = new RentalDBContext())
            {
                try
                {
                    //var bb = new List<byte> { 1, 2, 3 };
                    //var x = S3Manager.DownloadBinaryAsync("bush.jpg").Result;
                    //var y = S3Manager.UploadBinaryAsync(bb.ToArray(), "test").Result;

                    context.Database.EnsureCreated();
                    Console.WriteLine("Database connection successful!");

                    ApplicationConfiguration.Initialize();
                    Application.Run(new Home());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}