using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Database.Persistence;
using FormsApp.views.panels;
using FormsApp.views;

namespace FormsApp
{
    public partial class Home : Form
    {
        public bool isLoggingOut = false;

        public Home()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            Global.SizeAndCenterForm(this, 0.8f);
            CenterToScreen(); 
            LoadNavigation();
        }

        void LoadNavigation()
        {

            BaseNavigationPanel navigationPanelPanel = new BaseNavigationPanel(pnlMainView);

            pnlNavigation.Controls.Clear();
            pnlNavigation.Controls.Add(navigationPanelPanel);
        }

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isLoggingOut) Application.Exit();
        }
    }
}
