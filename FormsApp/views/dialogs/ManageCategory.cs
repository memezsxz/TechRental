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
        public ManageCategory(BaseViewEditDeleteForm.ViewType viewType, int? id = null) : base(viewType, id) { }

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
            if (id == null)
            {
                MessageBox.Show("Cannot delete id null");
                Dispose();
                return;
            }

            item = context.Categories.Get(id.Value);

            if (item == null)
            {
                MessageBox.Show($"Category with the id {id.Value} not found");
               Dispose();
                //Close();
            }

            if (context.Categories.IsReferenced(id.Value))
            {
                var result = MessageBox.Show($"This category is in use. Do you want to mark it as inactive instead?", "Category in use", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        item.IsActive = false;
                        context.Categories.Update(item);
                        context.SaveChanges();
                        RaiseSuccessfulComplete();
                    }
                    catch (Exception e)
                    {
                        Global.DisplayReportErrorDialog(e);
                        RaiseFailedComplete();
                    }
                }
                Dispose();

                return;
            }

            try
            {
                context.Categories.Remove(item);
                context.SaveChanges();
                RaiseSuccessfulComplete();
                Dispose();

            }
            catch (Exception e)
            {
                Global.DisplayReportErrorDialog(e);
                RaiseFailedComplete();
                Dispose();
            }
        }
        protected override async Task SaveItem()
        {

            if (!await ValidateInput()) return;

            Console.WriteLine("1");
            try
            {
                if (FormViewType == ViewType.EDIT)
                {
                    item.UpdatedAt = DateTime.UtcNow;
                    context.Categories.Update(item);
                    Console.WriteLine("2");

                }
                else if (FormViewType == ViewType.ADD)
                {
                    context.Categories.Add(item);
                    Console.WriteLine("3");
                }

                int rows = await context.SaveChangesAsync();

                if (rows > 0)
                {
                    MessageBox.Show($"Category {(FormViewType == ViewType.ADD ? "added" : "updated")} successfully");
                    RaiseSuccessfulComplete();
                    Close();
                }
                else
                {
                    Console.WriteLine("5");

                    MessageBox.Show($"Please Try Again");
                }
            }
            catch (Exception e)
            {
                Global.DisplayReportErrorDialog(e);
                RaiseFailedComplete();

            }
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
