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