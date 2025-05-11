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
    public partial class ProfileView : UserControl
    {
        private User user;
        public ProfileView()
        {
            InitializeComponent();
            lblLogout.Paint += (s, e) => Global.SetBorderColor(lblLogout, e, Color.Red);
            var isUserLoaded = FetchUser();
            if (isUserLoaded) LoadUserInfo();
            else LogOut();
        }



        private bool FetchUser()
        {
            var context = new UnitOfWork();
            user = context.Users.GetUserWithProfile(Global.userID);

            if (user != null) return true;

            MessageBox.Show($"User with the id {Global.userID} not found, will logout.");
            return false;
        }


        private void LoadUserInfo()
        {
            lblId.Text = user.Id.ToString();
            lblFirstName.Text = user.FirstName;
            lblLastName.Text = user.LastName;
            lblEmail.Text = user.Email;
            lblPhoneNumber.Text = user.PhoneNumber;
            lblRole.Text = user.Role?.RoleName;
            if (user.Image != null) Global.LoadImage(user.Image.Guid.Value, user.Image.ImageType, pnlImage, lblImage);
            else lblImage.Text = "No Image";
        }

        private void LogOut()
        {
            Home home = ((Home)this.TopLevelControl);
            Form login = home.Owner;

            home.isLoggingOut = true;
            home.Close(); 
            home.Dispose();    

            login.Show();  
        }

        private void lblLogout_Click(object sender, EventArgs e)
        {
            LogOut();
        }
    }
}
