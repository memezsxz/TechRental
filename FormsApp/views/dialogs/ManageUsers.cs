using Database.Core.Domain;
using Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FormsApp.views.dialogs.BaseViewEditDeleteForm;
using Image = Database.Core.Domain.Image;

namespace FormsApp.views.dialogs
{
    //public partial class ManageUser : Form
    public partial class ManageUser : BaseViewEditDeleteForm
    {
        #region Fields
        private User item;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the ManageUser form with the specified view type and optional equipment ID.
        /// </summary>
        /// <param name="viewType">The mode in which the form is opened (Add, Edit, or View).</param>
        /// <param name="id">Optional ID of the category to load in Edit mode.</param>
        public ManageUser(BaseViewEditDeleteForm.ViewType viewType, int? id = null) : base(viewType, id) { }

        #endregion

        #region Form Initialization

        /// <summary>
        /// Initializes the form components, disables validation errors,
        /// maps action buttons, and loads dropdown lists.
        /// </summary>
        protected override void InitializeForm()
        {
            InitializeComponent();

            DisableAllErrors();
            MapActionButtons();
            LoadRoleDropDownList();
        }

        /// <summary>
        /// Maps form action buttons (Save, Close, Delete) to corresponding UI labels in the base class to attach listeners on them.
        /// </summary>
        private void MapActionButtons()
        {
            closeLabel = lblClose;
            saveLabel = lblSave;
            deleteLabel = lblDelete;
            PrepareActionButtons();
        }
        #endregion

        #region View Preparation

        protected override void PrepareForView()
        {
            saveLabel.Visible = false;
            closeLabel.Location = saveLabel.Location;
            lblDelete.Visible = false;
            LoadItemInfo();
        }

        protected override void PrepareForAdd()
        {
            MessageBox.Show("Cannot add a user");
            return;

            lblSave.Text = "Add";
            lblDelete.Visible = false;
            cbIsActive.Checked = true;
            item = new User();
        }
        protected override void PrepareForEdit()
        {
            saveLabel.Visible = false;
            closeLabel.Location = saveLabel.Location;
            LoadItemInfo();
        }

        /// <summary>
        /// Loads data from the item into the form controls.
        /// </summary>
        private void LoadItemInfo()
        {
            tbId.Text = item.Id.ToString();
            tbFirstName.Text = item.FirstName;
            tbLastName.Text = item.LastName;
            tbEmail.Text = item.Email;
            tbPhoneNumber.Text = item.PhoneNumber;
            ddlRole.SelectedValue = item.RoleId;
            cbIsActive.Checked = item.IsActive ?? false;
            if (item.Image != null) LoadImage(item.Image.Guid, item.Image.ImageType, pnlImage, lblImage);
            else lblImage.Text = "No Image";
        }

        #endregion



        #region Data Loaders
        protected override bool FetchItem()
        {
            item = context.Users.GetUserWithProfile(id.Value);

            if (item != null) return true;

            MessageBox.Show($"User with the id {id.Value} not found");
            return false;
        }
        /// <summary>
        /// Populates the Role dropdown list with values from the database.
        /// </summary>
        private void LoadRoleDropDownList()
        {
            ddlRole.DisplayMember = "Value";
            ddlRole.ValueMember = "Key";

            ddlRole.DataSource = new BindingSource(context.UserRoles.GetAllByName(), null);
        }

        #endregion

        #region Save/Delete Logic
        public override void Delete()
        {
            MessageBox.Show("Cannot Delete user.");
            Dispose();

            //if (id == null)
            //{
            //    MessageBox.Show("Cannot delete id null");
            //    Dispose();
            //    return;
            //}

            //item = context.Users.Get(id.Value);

            //if (item == null)
            //{
            //    MessageBox.Show($"User with the id {id.Value} not found");
            //    Dispose();
            //}

            //if (context.Categories.IsReferenced(id.Value))
            //{
            //    var result = MessageBox.Show($"This user refrenced elsewhare. Do you want to mark it as inactive instead?", "User in use", MessageBoxButtons.YesNo);

            //    if (result == DialogResult.Yes)
            //    {
            //        try
            //        {
            //            item.IsActive = false;
            //            context.Users.Update(item);
            //            context.SaveChanges();
            //            RaiseSuccessfulComplete();
            //        }
            //        catch (Exception e)
            //        {
            //            Global.DisplayReportErrorDialog(e);
            //            RaiseFailedComplete();
            //        }
            //    }
            //    Dispose();

            //    return;
            //}

            //try
            //{
            //    context.Users.Remove(item);
            //    context.SaveChanges();
            //    RaiseSuccessfulComplete();
            //    Dispose();

            //}
            //catch (Exception e)
            //{
            //    Global.DisplayReportErrorDialog(e);
            //    RaiseFailedComplete();
            //    Dispose();
            //}
        }
        protected override async Task SaveItem()
        {
            MessageBox.Show("Cannot edit user.");
            //if (!await ValidateInput()) return;
        }
        #endregion



        #region Validation and Image Upload

        /// <summary>
        /// Validates form input and uploads the image if applicable.
        /// </summary>
        /// <returns>True if validation passed and image uploaded successfully; false otherwise.</returns>
        private async Task<bool> ValidateInput()
        {
            DisableAllErrors();

            //bool isValidInput = true;

            //isValidInput &= ValidateTextLength(tbFirstName.Text, lblFirstNameError, "First name", true, 3, 50);
            //isValidInput &= ValidateTextLength(tbLastName.Text, lblLastNameError, "Last name", true, 3, 50);
            //bool isLengthValid = ValidateTextLength(tbEmail.Text, lblEmailError, "Email", true, 3, 100);

            //if (isLengthValid)
            //{
            //    bool isFormatValid = IsValidEmail(tbEmail.Text);
            //    if (!isFormatValid)
            //    {
            //        isValidInput &= ActivateError(lblEmailError, "Please enter a valid email");
            //    }
            //}
            //else
            //{
            //    isValidInput = false;
            //}
            //bool isPhoneLengthValid = ValidateTextLength(tbPhoneNumber.Text, lblPhoneNumberError, "Phone", true, 7, 15);

            //if (isPhoneLengthValid)
            //{
            //    bool isPhoneFormatValid = IsValidPhone(tbPhoneNumber.Text);
            //    if (!isPhoneFormatValid)
            //    {
            //        isValidInput &= ActivateError(lblPhoneNumberError, "Please enter a valid phone number");
            //    }
            //}
            //else
            //{
            //    isValidInput = false;
            //}


            // // validate image 


            //if (!isValidInput) return false;

            //item.FirstName = tbFirstName.Text.Trim();
            //item.LastName = tbLastName.Text.Trim();
            //item.Email = tbEmail.Text.Trim();
            //item.PhoneNumber = tbPhoneNumber.Text.Trim();
            //item.RoleId = (int)ddlRole.SelectedValue;

            return true;
        }

        /// <summary>
        /// Disables all visible validation error labels on the form.
        /// </summary>
        private void DisableAllErrors()
        {
            lblFirstNameError.Visible = false;
            lblLastNameError.Visible = false;
            lblEmailError.Visible = false;
            lblPhoneNumberError.Visible = false;
            lblImageError.Visible = false;
        }


        #endregion
    }
}
