using Database.Persistence;
using System.Windows.Forms;
using System.Threading.Tasks;
using Amazon.S3.Model;
using System.Windows.Forms;

namespace FormsApp
{
    internal static class Program
    {

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

    }
}