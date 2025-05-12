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
    /// <summary>
    /// Login form for authenticating users with email and password credentials
    /// </summary>
    public partial class Login : Form
    {
        #region Fields and Properties
        private IServiceProvider serviceProvider;
        private IdentityContext IdentityContext = new IdentityContext();

        /// <summary>
        /// Flag to prevent multiple concurrent login attempts
        /// </summary>
        private bool didClick = false;
        #endregion

        #region Constructor and Initialization
        /// <summary>
        /// Initializes a new instance of the Login form
        /// </summary>
        public Login()
        {
            InitializeComponent();
            var services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Form load event handler - resets the form state
        /// </summary>
        private void Login_Load(object sender, EventArgs e)
        {
            Refresh();
        }
        #endregion

        #region UI Event Handlers
        /// <summary>
        /// Login button click event handler
        /// </summary>
        private void lblLogin_Click(object sender, EventArgs e)
        {
            ProcessLogin();
        }

        /// <summary>
        /// Visible changed event handler - refreshes form when made visible
        /// </summary>
        private void Login_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                Refresh();
            }
        }
        #endregion

        #region Login Process
        /// <summary>
        /// Processes the login attempt asynchronously
        /// </summary>
        private async void ProcessLogin()
        {
            // Prevent multiple concurrent login attempts
            if (didClick) return;
            DisableAllErrors();

            // Get user input
            string email = tbEmail.Text.Trim();
            string password = tbPassword.Text.Trim();

            // Validate input fields
            if (!ValidateFields(email, password)) return;

            // Show loading indicator
            pbLoading.Visible = true;
            await Task.Delay(5); // Small delay to ensure UI updates

            didClick = true;

            try
            {
                // Attempt authentication
                bool signInResults = await VerifyUserNamePassword(email, password, lblLoginError);
                if (signInResults)
                {
                    // Login successful - open main form
                    Home home = new Home();
                    home.Owner = this;
                    this.Hide();
                    home.Show();
                }
                else
                {
                    // Show generic login error
                    lblLoginError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Global.DisplayReportErrorDialog(ex);
            }
            finally
            {
                // Reset UI state
                pbLoading.Visible = false;
                didClick = false;
            }
        }

        /// <summary>
        /// Verifies user credentials against the identity database
        /// </summary>
        /// <param name="userName">Email address to authenticate</param>
        /// <param name="password">Password to verify</param>
        /// <param name="errorLabel">Label to display error messages</param>
        /// <returns>True if authentication succeeds, false otherwise</returns>
        public async Task<bool> VerifyUserNamePassword(string userName, string password, Label errorLabel)
        {
            try
            {
                lblLoginError.Text = "Invalid email or password, please try again.";
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                // Find user by email
                var founduser = await userManager.FindByEmailAsync(userName);

                if (founduser != null)
                {
                    // Verify password
                    var passCheck = await userManager.CheckPasswordAsync(founduser, password) == true;

                    if (passCheck)
                    {
                        // Get user roles
                        var roles = await userManager.GetRolesAsync(founduser);

                        // Get additional user data from application database
                        UnitOfWork context = new UnitOfWork();
                        User user = context.Users.GetUserByEmail(userName);

                        if (user != null)
                        {
                            // Set global user information
                            Global.userID = user.Id;
                            Global.userType = user.Role.RoleName.ToLower();
                            Console.WriteLine($"role is {Global.userType}");

                            // Validate user has appropriate role
                            if (Global.userType.ToLower() is not "admin" and not "manager")
                            {
                                lblLoginError.Text = "Invalid user role, this app is only for admins and managers.";
                                return false;
                            }
                            else if ((user.IsActive ?? true) == false)
                            {
                                // User account is inactive
                                return false;
                            }

                            return passCheck;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Global.DisplayReportErrorDialog(ex);
                lblLoginError.Text = "An error occurred, try again later.";
                return false;
            }
        }
        #endregion

        #region Validation Methods
        /// <summary>
        /// Validates both email and password fields
        /// </summary>
        /// <param name="email">Email address to validate</param>
        /// <param name="password">Password to validate</param>
        /// <returns>True if both fields are valid, false otherwise</returns>
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

        /// <summary>
        /// Validates an email address format using regular expression
        /// </summary>
        /// <param name="email">Email address to validate</param>
        /// <returns>True if email is valid, false otherwise</returns>
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            // Regular expression pattern for basic email validation:
            // 1. Characters before @
            // 2. @ symbol
            // 3. Characters after @
            // 4. Dot
            // 5. Characters after dot
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Validates password meets minimum requirements
        /// </summary>
        /// <param name="password">Password to validate</param>
        /// <returns>True if password is valid, false otherwise</returns>
        public bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;
            if (password.Length < 6) return false; // Minimum 6 characters
            return true;
        }
        #endregion

        #region UI Helper Methods
        /// <summary>
        /// Hides all error labels on the form
        /// </summary>
        private void DisableAllErrors()
        {
            lblEmailError.Visible = false;
            lblPasswordError.Visible = false;
            lblLoginError.Visible = false;
        }

        /// <summary>
        /// Resets the form to its initial state
        /// </summary>
        private void Refresh()
        {
            DisableAllErrors();
            tbEmail.Text = "";
            tbPassword.Text = "";
            pbLoading.Visible = false;
            tbEmail.Focus(); // Set focus to email field
            Global.userID = -1;
            Global.userType = null;
        }
        #endregion

        #region Service Configuration
        /// <summary>
        /// Configures dependency injection services for identity management
        /// </summary>
        /// <param name="services">Service collection to configure</param>
        private void ConfigureServices(IServiceCollection services)
        {
            try
            {
                string connectionString = "Server=reboot08.com,1433;Database=RentalIdentity;User Id=sa;Password=caliber,willpower,enjoyably,ending,giggling,P5;Encrypt=False;TrustServerCertificate=True;";

                // Configure database context
                services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlServer(connectionString));

                services.AddEntityFrameworkSqlServer()
                    .AddDbContext<IdentityContext>();

                // Configure identity services
                services.AddIdentity<ApplicationUser, IdentityRole>()
                   .AddEntityFrameworkStores<IdentityContext>()
                   .AddDefaultTokenProviders();

                // Add required supporting services
                services.AddLogging();
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }
            catch (Exception ex)
            {
                Global.DisplayReportErrorDialog(ex);
            }
        }
        #endregion
    }
}