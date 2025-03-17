using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Database.Persistence;
using FormsApp.views.panels;
using FormsApp.views;

namespace FormsApp
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {

            //pnlMainView.Paint += Panel_Paint;

            Global.SizeAndCenterForm(this, 0.8f);
            CenterToScreen();
            LoadNavigation();

        }

        void LoadNavigation()
        {
            new UnitOfWork(new RentalDBContext()).Equipment.GetAll();
            // check user type and load the proper navigation user control
            admin_navigation navigationPanel = new admin_navigation(pnlMainView);

            pnlNavigation.Controls.Clear();
            pnlNavigation.Controls.Add(navigationPanel);
        }
    }
}
