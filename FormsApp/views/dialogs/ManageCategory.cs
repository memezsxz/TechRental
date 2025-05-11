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

namespace FormsApp.views.dialogs
{
    //public partial class ManageCategory : Form
    public partial class ManageCategory : BaseViewEditDeleteForm
    {
        #region Fields

        private Category item;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the ManageCategory form with the specified view type and optional equipment ID.
        /// </summary>
        /// <param name="viewType">The mode in which the form is opened (Add, Edit, or View).</param>
        /// <param name="id">Optional ID of the category to load in Edit mode.</param>
        public ManageCategory(BaseViewEditDeleteForm.ViewType viewType, int? id, bool canDelete = false) : base(viewType, id, canDelete) { }

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
            lblSave.Text = "Add";
            lblDelete.Visible = false;
            cbIsActive.Checked = false;
            item = new Category();
        }
        protected override void PrepareForEdit()
        {
            LoadItemInfo();
        }

        /// <summary>
        /// Loads data from the item into the form controls.
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
        protected override bool FetchItem()
        {
            item = context.Categories.Get(id.Value);

            if (item != null) return true;

            MessageBox.Show($"Equipment with the id {id.Value} not found");
            return false;
        }
        #endregion


        #region Save/Delete Logic
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
        protected override async Task SaveItem()
        {
            await StandardSave<Category>(
                ValidateInput,
                MapFormToEntity,
                context.Categories.Add,
                context.Categories.Update,
                item,
                "Category"
            );
        }

        protected override void MapFormToEntity()
        {
            item.Name = lblName.Text.Trim();
            item.Description = tbDescription.Text.Trim();
        }
        #endregion



        #region Validation and Image Upload

        /// <summary>
        /// Validates form input and uploads the image if applicable.
        /// </summary>
        /// <returns>True if validation passed and image uploaded successfully; false otherwise.</returns>
        private bool ValidateInput()
        {
            DisableAllErrors();

            bool isValidInput = true;

            isValidInput &= ValidateTextLength(lblName.Text, lblNameError, "Name", true, 3, 100);

            isValidInput &= ValidateTextLength(tbDescription.Text, lblDescreptionError, "Description", true, 3, 255);

            if (!isValidInput) return false;

            item.Name = lblName.Text.Trim();
            item.Description = tbDescription.Text.Trim();
            item.IsActive = cbIsActive.Checked;

            return true;
        }

        /// <summary>
        /// Disables all visible validation error labels on the form.
        /// </summary>
        private void DisableAllErrors()
        {
            lblNameError.Visible = false;
            lblDescreptionError.Visible = false;
        }
        #endregion
    }
}
