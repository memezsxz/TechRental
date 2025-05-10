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
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace FormsApp
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            DisableAllErrors();
        }

        private void lblLogin_Click(object sender, EventArgs e)
        {
            DisableAllErrors();

            string email = tbEmail.Text.Trim();
            string password = tbPassword.Text.Trim();

            if (!ValidateFields(email, password)) return;

            bool isValid = false;

            isValid = true;

            if (isValid)
            {
                Form home = new Home();
                home.Owner = this;
                home.Show();
                this.Hide();
            }
            else
            {
                lblLoginError.Visible = true;
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
