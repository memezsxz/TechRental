using Database.Core.Domain;

namespace FormsApp.views.dialogs
{
    /*
     * WARNING:
     * To view this form in the Windows Forms Designer,
     * temporarily change the base class from 'BaseViewEditDeleteForm' to 'Form'.
     *
     * Example:
     *     public partial class ManageUser : Form
     *
     * After making design changes, revert the base class back to 'BaseViewEditDeleteForm'
     * to preserve functionality and application behavior.
     */

    /// <summary>
    /// A form used to display and (optionally) manage user accounts in view/edit modes.
    /// </summary>
    //public partial class ManageUser : Form
    public partial class ManageUser : BaseViewEditDeleteForm
    {
        #region Fields

        /// <summary>
        /// The user entity currently loaded into the form.
        /// </summary>
        private User item;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the ManageUser form with the specified view type and optional user ID.
        /// </summary>
        /// <param name="viewType">The mode in which the form is opened (View, Edit, Add).</param>
        /// <param name="id">Optional ID of the user to load.</param>
        public ManageUser(ViewType viewType, int? id = null)
            : base(viewType, id) { }

        #endregion

        #region Form Initialization

        /// <inheritdoc/>
        protected override void InitializeForm()
        {
            InitializeComponent();
            DisableAllErrors();
            MapActionButtons(lblClose, lblSave, lblDelete);
            LoadRoleDropDownList();
        }

        #endregion
        #region View Preparation

        /// <inheritdoc/>
        protected override void PrepareForView()
        {
            base.PrepareForView();
            LoadItemInfo();
        }

        /// <inheritdoc/>
        protected override void PrepareForAdd()
        {
            MessageBox.Show("Cannot add a user.", "Operation Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;

            // Code below is skipped due to return above — reserved for potential future enablement
            lblSave.Text = "Add";
            lblDelete.Visible = false;
            cbIsActive.Checked = true;
            item = new User();
        }

        /// <inheritdoc/>
        protected override void PrepareForEdit()
        {
            saveLabel.Visible = false;
            closeLabel.Location = saveLabel.Location;
            LoadItemInfo();
        }

        /// <summary>
        /// Loads user data into the form's input fields.
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

        /// <inheritdoc/>
        protected override bool FetchItem()
        {
            item = context.Users.GetUserWithProfile(id.Value);

            if (item != null) return true;

            MessageBox.Show($"User with the ID {id.Value} was not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        /// <summary>
        /// Populates the role dropdown list with available user roles from the database.
        /// </summary>
        private void LoadRoleDropDownList()
        {
            ddlRole.DisplayMember = "Value";
            ddlRole.ValueMember = "Key";
            ddlRole.DataSource = new BindingSource(context.UserRoles.GetAllByName(), null);
        }

        #endregion
        #region Save/Delete Logic

        /// <inheritdoc/>
        public override void Delete()
        {
            MessageBox.Show("Cannot delete user.", "Operation Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Dispose();

            // Uncomment below to enable soft delete logic in the future:
            /*
            StandardDelete<User>(
                context.Users.Get,
                context.Users.IsReferenced,
                item => item.IsActive = false,
                context.Users.Remove,
                "User"
            );
            */
        }

        /// <inheritdoc/>
        protected override async Task SaveItem()
        {
            MessageBox.Show("Cannot edit user.", "Operation Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Dispose();
        }

        /// <inheritdoc/>
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
        /// Validates form input including text fields, email/phone formats, and image (future enhancement).
        /// </summary>
        /// <returns>False (validation logic not implemented yet).</returns>
        private async Task<bool> ValidateInput()
        {
            DisableAllErrors();

            // Reserved for future use:
            /*
            bool isValidInput = true;

            isValidInput &= ValidateTextLength(tbFirstName.Text, lblFirstNameError, "First name", true, 3, 50);
            isValidInput &= ValidateTextLength(tbLastName.Text, lblLastNameError, "Last name", true, 3, 50);

            bool isEmailLengthValid = ValidateTextLength(tbEmail.Text, lblEmailError, "Email", true, 3, 100);
            if (isEmailLengthValid)
            {
                if (!IsValidEmail(tbEmail.Text))
                    isValidInput &= ActivateError(lblEmailError, "Please enter a valid email");
            }
            else
            {
                isValidInput = false;
            }

            bool isPhoneValid = ValidateTextLength(tbPhoneNumber.Text, lblPhoneNumberError, "Phone", true, 7, 15);
            if (isPhoneValid)
            {
                if (!IsValidPhone(tbPhoneNumber.Text))
                    isValidInput &= ActivateError(lblPhoneNumberError, "Please enter a valid phone number");
            }
            else
            {
                isValidInput = false;
            }

            if (!isValidInput) return false;

            item.FirstName = tbFirstName.Text.Trim();
            item.LastName = tbLastName.Text.Trim();
            item.Email = tbEmail.Text.Trim();
            item.PhoneNumber = tbPhoneNumber.Text.Trim();
            item.RoleId = (int)ddlRole.SelectedValue;

            return true;
            */

            return false;
        }

        /// <summary>
        /// Hides all validation error labels on the form.
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
