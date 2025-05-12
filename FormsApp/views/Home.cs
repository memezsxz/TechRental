using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Database.Persistence;
using FormsApp.views.panels;
using FormsApp.views;

namespace FormsApp
{
    /// <summary>
    /// Main application form that serves as the container for navigation and content panels
    /// </summary>
    public partial class Home : Form
    {
        #region Fields and Properties
        /// <summary>
        /// Flag indicating whether the user is logging out (as opposed to closing the application)
        /// </summary>
        public bool isLoggingOut = false;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the Home form
        /// </summary>
        public Home()
        {
            InitializeComponent();
        }
        #endregion

        #region Form Event Handlers
        /// <summary>
        /// Form load event handler - sets up the initial form state
        /// </summary>
        private void Home_Load(object sender, EventArgs e)
        {
            // Size and position the form
            Global.SizeAndCenterForm(this, 0.8f);  // 80% of screen size
            CenterToScreen(); // Ensure form is centered

            // Initialize navigation
            LoadNavigation();
        }

        /// <summary>
        /// Form closing event handler - manages application exit behavior
        /// </summary>
        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Only exit application if this is not a logout action
            if (!isLoggingOut)
            {
                Application.Exit();
            }
        }
        #endregion

        #region Navigation Methods
        /// <summary>
        /// Loads and initializes the navigation panel
        /// </summary>
        private void LoadNavigation()
        {
            // Create new navigation panel instance with reference to the main content panel
            BaseNavigationPanel navigationPanelPanel = new BaseNavigationPanel(pnlMainView);

            // Clear any existing navigation controls
            pnlNavigation.Controls.Clear();

            // Add the new navigation panel to the navigation container
            pnlNavigation.Controls.Add(navigationPanelPanel);
        }
        #endregion
    }
}