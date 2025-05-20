using Database.Core.Domain;
using Helper;
using System;
using System.Drawing;
using System.Windows.Forms;
using static FormsApp.views.dialogs.BaseViewEditDeleteForm;

namespace FormsApp.views.dialogs
{
    /*
     * WARNING:
     * To view this form in the Windows Forms Designer,
     * temporarily change the base class from 'BaseViewEditDeleteForm' to 'Form'.
     *
     * Example:
     *     public partial class ManageCategory : Form
     *
     * After making design changes, revert the base class back to 'BaseViewEditDeleteForm'
     * to preserve functionality and application behavior.
     */

    /// <summary>
    /// A form used for managing Category entities, including adding, editing, and soft-deleting.
    /// Inherits shared logic from <see cref="BaseViewEditDeleteForm"/>.
    /// </summary>
    //public partial class ManageCategory : Form
    public partial class ManageCategory : BaseViewEditDeleteForm
    {
        #region Fields

        /// <summary>
        /// The category item being added, edited, or viewed.
        /// </summary>
        private Category item;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageCategory"/> form with a specified view type and ID.
        /// </summary>
        /// <param name="viewType">The mode in which the form is opened (Add, Edit, or View).</param>
        /// <param name="id">Optional ID of the category to load in Edit or View mode.</param>
        /// <param name="canDelete">Whether the delete option should be available.</param>
        public ManageCategory(ViewType viewType, int? id, bool canDelete = false)
            : base(viewType, id, canDelete) { }

        #endregion

        #region Form Initialization

        /// <inheritdoc/>
        protected override void InitializeForm()
        {
            InitializeComponent();
            DisableAllErrors();
            MapActionButtons(lblClose, lblSave, lblDelete);
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
            lblSave.Text = "Add";
            lblDelete.Visible = false;
            cbIsActive.Checked = false;
            item = new Category();
        }

        /// <inheritdoc/>
        protected override void PrepareForEdit()
        {
            LoadItemInfo();
        }

        /// <summary>
        /// Loads the category data into the form controls.
        /// </summary>
        private void LoadItemInfo()
        {
            lblId.Text = item.Id.ToString();
            lblName.Text = item.Name;
            tbDescription.Text = item.Description;
            cbIsActive.Checked = item.IsActive ?? false;
        }

        #endregion

        #region Data Loaders

        /// <inheritdoc/>
        protected override bool FetchItem()
        {
            item = context.Categories.Get(id.Value);

            if (item != null)
                return true;

            MessageBox.Show($"Category with the ID {id.Value} not found.");
            return false;
        }

        #endregion

        #region Save/Delete Logic

        /// <inheritdoc/>
        public override void Delete()
        {
            StandardDelete<Category>(
                context.Categories.Get,
                context.Categories.IsReferenced,
                item => item.IsActive = false,
                context.Categories.Remove,
                "Category"
            );
        }

        /// <inheritdoc/>
        protected override async Task SaveItem()
        {
            await StandardSave(
                ValidateInput,
                MapFormToEntity,
                context.Categories.Add,
                context.Categories.Update,
                item,
                item.Id,
                "Category"
            );
        }

        /// <inheritdoc/>
        protected override void MapFormToEntity()
        {
            item.Name = lblName.Text.Trim();
            item.Description = tbDescription.Text.Trim();
            item.UpdatedAt = DateTime.Now;
        }

        #endregion
       
        #region Validation and Utility

        /// <summary>
        /// Validates the form input fields and applies any changes to the entity object if valid.
        /// </summary>
        /// <returns>
        /// True if all fields are valid and mapped into <c>item</c>; false if validation failed.
        /// </returns>
        private bool ValidateInput()
        {
            DisableAllErrors();

            bool isValidInput = true;

            // Validate Name field (required, length 3–100)
            isValidInput &= ValidateTextLength(
                lblName.Text,
                lblNameError,
                "Name",
                required: true,
                minLength: 3,
                maxLength: 100);

            bool duplicate = context.Categories.IsDuplicateName(item.Id, lblName.Text.Trim());

            if (isValidInput && duplicate)
            {
                lblNameError.Text = "A category with the same name already exists";
                lblNameError.Visible = true;
            }

            isValidInput &= !duplicate;

            // Validate Description field (required, length 3–255)
            isValidInput &= ValidateTextLength(
                tbDescription.Text,
                lblDescreptionError,
                "Description",
                required: true,
                minLength: 3,
                maxLength: 255);

            if (!isValidInput)
                return false;

            // If valid, update values in the Category entity
            item.Name = lblName.Text.Trim();
            item.Description = tbDescription.Text.Trim();
            item.IsActive = cbIsActive.Checked;

            return true;
        }

        /// <summary>
        /// Hides all visible error labels related to form validation.
        /// This is typically called before performing field-level validation.
        /// </summary>
        private void DisableAllErrors()
        {
            lblNameError.Visible = false;
            lblDescreptionError.Visible = false;
        }

        #endregion
    }
}
