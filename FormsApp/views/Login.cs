using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Database.Core.Domain;
using Database.Persistence;
using Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FormsApp
{
    public partial class Login : Form
    {
        private IServiceProvider serviceProvider;

        IdentityContext IdentityContext = new IdentityContext();

        private bool didClick = false;
        public Login()
        {
            InitializeComponent();
            var services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            DisableAllErrors();
            pbLoading.Visible = false;
        }

        private void lblLogin_Click(object sender, EventArgs e)
        {
            ProcessLogin();
        }

        private async void ProcessLogin()
        {
            if (didClick) return;
            DisableAllErrors();

            string email = tbEmail.Text.Trim();
            string password = tbPassword.Text.Trim();

            if (!ValidateFields(email, password)) return;

            pbLoading.Visible = true;
            await Task.Delay(5);

            didClick = true;

            bool signInResults = await VerifyUserNamePassword(email, password, lblLoginError);
            if (signInResults) //if user is verified
            {
                //do something.. i.e. navigate to next forms
                Home home = new Home();
                home.Owner = this;
                this.Hide();
                home.Show();
            }
            else
            {
                lblLoginError.Visible = true;
            }
            pbLoading.Visible = false;
            didClick = false;
        }

        public async Task<bool> VerifyUserNamePassword(string userName, string password, Label errorLabel)
        {
            try
            {
                lblLoginError.Text = "Invalid email or password, please try again.";
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var founduser = await userManager.FindByEmailAsync(userName);

                if (founduser != null)
                {
                    var passCheck = await userManager.CheckPasswordAsync(founduser, password) == true;

                    if (passCheck)
                    {
                        var roles = await userManager.GetRolesAsync(founduser);

                        UnitOfWork context = new UnitOfWork();

                        User user = context.Users.GetUserByEmail(userName);

                        if (user != null)
                        {
                            Global.userID = user.Id;
                            Global.userType = user.Role.RoleName;

                            if (Global.userType.ToLower() is not "admin" or not "manager")
                            {
                                lblLoginError.Text = "Invalid user role, this app is only for admins and managers.";
                                return false;
                            }

                            return passCheck;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                lblLoginError.Text = "An error occurred, try again later.";
                return false;
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            try
            {
                string connectionString = "Server=reboot08.com,1433;Database=RentalIdentity;User Id=sa;Password=caliber,willpower,enjoyably,ending,giggling,P5;Encrypt=False;TrustServerCertificate=True;";

                services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlServer(connectionString));

                services.AddEntityFrameworkSqlServer()
                    .AddDbContext<IdentityContext>();

                // Register UserManager & RoleManager
                services.AddIdentity<ApplicationUser, IdentityRole>()
                   .AddEntityFrameworkStores<IdentityContext>()
                   .AddDefaultTokenProviders();

                // UserManager & RoleManager require logging and HttpContext dependencies
                services.AddLogging();
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }

        private bool ValidateFields(string email, string password)
        {
            bool isValidInput = true;

            if (!IsValidEmail(email))
            {
                lblEmailError.Visible = true;
                isValidInput = false;
            }

            if (!IsValidPassword(password))
            {
                lblPasswordError.Visible = true;
                isValidInput = false;
            }

            return isValidInput;
        }
        private void DisableAllErrors()
        {
            lblEmailError.Visible = false;
            lblPasswordError.Visible = false;
            lblLoginError.Visible = false;
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            // Regular expression for basic email validation
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }
        public bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;
            if (password.Length < 6) return false;
            return true;
        }

    }
}
