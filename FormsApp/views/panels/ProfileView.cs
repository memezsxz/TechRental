using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Database.Core.Domain;
using Database.Persistence;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FormsApp.views.panels
{
    /// <summary>
    /// User control for displaying and managing user profile information
    /// </summary>
    public partial class ProfileView : UserControl
    {
        #region Fields
        /// <summary>
        /// Current user being displayed
        /// </summary>
        private User user;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the ProfileView control
        /// </summary>
        public ProfileView()
        {
            InitializeComponent();

            // Add red border to logout label
            lblLogout.Paint += (s, e) => Global.SetBorderColor(lblLogout, e, Color.Red);

            // Load user data on initialization
            var isUserLoaded = FetchUser();
            if (isUserLoaded)
            {
                LoadUserInfo();
            }
            else
            {
                LogOut();
            }
        }
        #endregion

        #region User Data Methods
        /// <summary>
        /// Fetches user data from the database
        /// </summary>
        /// <returns>True if user was found and loaded, false otherwise</returns>
        private bool FetchUser()
        {
            var context = new UnitOfWork();
            user = context.Users.GetUserWithProfile(Global.userID);

            if (user != null) return true;

            // Show error and logout if user not found
            MessageBox.Show($"User with the id {Global.userID} not found, will logout.");
            return false;
        }

        /// <summary>
        /// Loads user information into the UI controls
        /// </summary>
        private void LoadUserInfo()
        {
            // Set basic user information
            lblId.Text = user.Id.ToString();
            lblFirstName.Text = user.FirstName;
            lblLastName.Text = user.LastName;
            lblEmail.Text = user.Email;
            lblPhoneNumber.Text = user.PhoneNumber;
            lblRole.Text = user.Role?.RoleName;

            // Load profile image if available
            if (user.Image != null)
            {
                Global.LoadImage(user.Image.Guid.Value, user.Image.ImageType, pnlImage, lblImage);
            }
            else
            {
                lblImage.Text = "No Image";
            }
        }
        #endregion

        #region Logout Methods
        /// <summary>
        /// Handles the logout process
        /// </summary>
        private void LogOut()
        {
            // Get reference to parent forms
            Home home = ((Home)this.TopLevelControl);
            Form login = home.Owner;

            // Set logout flag and close home form
            home.isLoggingOut = true;
            home.Close();
            home.Dispose();

            // Show login form again
            login.Show();
        }

        /// <summary>
        /// Logout label click event handler
        /// </summary>
        private void lblLogout_Click(object sender, EventArgs e)
        {
            LogOut();
        }
        #endregion
    }
}