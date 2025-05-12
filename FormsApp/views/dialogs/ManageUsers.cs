using Database.Core.Domain;
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
            MapActionButtons(lblClose, lblSave, lblDelete);
            LoadRoleDropDownList();
        }



        #endregion

        #region View Preparation

        protected override void PrepareForView()
        {
            base.PrepareForView();
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

            //public override void Delete()
            //{
            //    StandardDelete<User>(
            //        context.Users.Get,
            //        context.Users.IsReferenced,
            //        item => item.IsActive = false,
            //        context.Users.Remove,
            //        "User"
            //    );
            //}

        }

        protected override async Task SaveItem()
        {
            MessageBox.Show("Cannot edit user.");
            Dispose();
        }

        protected override void MapFormToEntity()
        {
            item.FirstName = tbFirstName.Text.Trim();
            item.LastName = tbLastName.Text.Trim();
            item.Email = tbEmail.Text.Trim();
            item.PhoneNumber = tbPhoneNumber.Text.Trim();
            item.RoleId = (int)ddlRole.SelectedValue;
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

            return false;
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
