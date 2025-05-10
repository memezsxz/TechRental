using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Database.Core.Domain;
using Database.Persistence;
using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using Image = System.Drawing.Image;
using Helper;
using System.Drawing.Imaging;
using System.Windows.Forms.DataVisualization.Charting;
using PdfSharpCore.Pdf.Content.Objects;
using System.Diagnostics;

namespace FormsApp.views.dialogs
{
    /// <summary>
    /// Manages equipment data for Add, Edit, and Delete operations in the Equipment Rental Management System.
    /// </summary>
    //public partial class ManageEquipment : Form
    public partial class ManageEquipment : BaseViewEditDeleteForm
    {
        #region Fields
        private Equipment item;
        private Image uploadedImage;
        private string imageFormat;
        private string imageExtention;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the ManageEquipment form with the specified view type and optional equipment ID.
        /// </summary>
        /// <param name="viewType">The mode in which the form is opened (Add, Edit, or View).</param>
        /// <param name="id">Optional ID of the equipment to load in Edit mode.</param>
        public ManageEquipment(BaseViewEditDeleteForm.ViewType viewType, int? id, bool canDelete = false) : base(viewType, id, canDelete) { }

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
            LoadAvailabilityDropDownList();
            LoadConditionDropDownList();
            LoadCategoryDropDownList();
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
            item = new Equipment();
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
            lblPrice.Text = item.RentalPricePerDay.ToString("C");
            ddlAvalability.SelectedValue = item.AvailabilityStatusId;
            ddlCondition.SelectedValue = item.ConditionStatusId;
            ddlCategory.SelectedValue = item.CategoryId;
            tbDescription.Text = item.Description;
            cbIsActive.Checked = item.IsActive ?? false;
            if (item.Image != null) LoadImage(item.Image.Guid, item.Image.ImageType, pnlImage, lblImage);
            else lblImage.Text = "No Image";
        }

        #endregion


        #region Dropdown Loaders
        /// <summary>
        /// Populates the Availability dropdown list with values from the database.
        /// </summary>

        private void LoadAvailabilityDropDownList()
        {
            ddlAvalability.DisplayMember = "Value";
            ddlAvalability.ValueMember = "Key";

            ddlAvalability.DataSource = new BindingSource(context.EquipmentAvailabilityStatuses.GetAllByName(), null);
        }
        /// <summary>
        /// Populates the Condition dropdown list with values from the database.
        /// </summary>

        private void LoadConditionDropDownList()
        {
            ddlCondition.DisplayMember = "Value";
            ddlCondition.ValueMember = "Key";

            ddlCondition.DataSource = new BindingSource(context.EquipmentConditionStatuses.GetAllByName(), null);
        }
        /// <summary>
        /// Populates the Category dropdown list with values from the database.
        /// </summary>

        private void LoadCategoryDropDownList()
        {
            ddlCategory.DisplayMember = "Value";
            ddlCategory.ValueMember = "Key";

            ddlCategory.DataSource = new BindingSource(context.Categories.GetAllByName(), null);
        }

        #endregion

        #region Data Loaders
        protected override bool FetchItem()
        {
            item = context.Equipment.GetEquipmentWithImage(id.Value);

            if (item != null) return true;

            MessageBox.Show($"Equipment with the id {id.Value} not found");
            return false;
        }
        /// <summary>
        /// Loads the image associated with the equipment from storage and displays it in the panel.
        /// </summary>

        #endregion


        #region Save/Delete Logic
        public override void Delete()
        {
            StandardDelete<Equipment>(
                context.Equipment.GetEquipmentWithImage,
                context.Equipment.IsReferenced,
                item => item.IsActive = false,
                context.Equipment.Remove,
                "Equipment"
            );
        }
  
        protected override async Task SaveItem()
        {
            await StandardSaveAsync<Equipment>(
                ValidateInput,
                MapFormToEntity,
                context.Equipment.Add,
                context.Equipment.Update,
                item,
                "Equipment"
            );
        }

        protected override void MapFormToEntity()
        {
            item.Name = lblName.Text.Trim();

            item.RentalPricePerDay = decimal.Parse(lblPrice.Text.Trim(), NumberStyles.Currency, CultureInfo.CurrentCulture);
            item.AvailabilityStatusId = (int)ddlAvalability.SelectedValue;
            item.ConditionStatusId = (int)ddlCondition.SelectedValue;
            item.CategoryId = (int)ddlCategory.SelectedValue;

            item.Description = tbDescription.Text.Trim();
            item.IsActive = cbIsActive.Checked;

            // image is uploaded and verified in validation when calling UploadImage
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

            isValidInput &= ValidateNumericField<decimal>(
                lblPrice.Text,
                lblPriceError,
                "Price",
                required: true,
                allowZero: false,
                allowNegative: false,
                parser: s => decimal.Parse(s, NumberStyles.Currency, CultureInfo.CurrentCulture),
                out var price,
                minValue: 0.01m,
                maxValue: 9999
            );

            if (uploadedImage == null && item.Image == null)
            {
                isValidInput &= ActivateError(lblImageError, "Please Upload an Image");
            }


            if (uploadedImage != null)
            {
                bool didUpload = await UploadImage();
                if (!didUpload) isValidInput &= ActivateError(lblImageError, "Unable to upload image, try again");
            }

            Console.WriteLine($"Final validation result: {isValidInput}");

            return isValidInput;
        }

        /// <summary>
        /// Uploads the selected image to cloud storage and the database.
        /// </summary>
        /// <returns>True if upload succeeded; false otherwise.</returns>

        private async Task<bool> UploadImage()
        {
            try
            {
                using var uploadStream = new MemoryStream();
                uploadedImage.Save(uploadStream, System.Drawing.Imaging.ImageFormat.Png);

                string fileExtension = imageExtention;
                string contentType = imageFormat;
                Guid imageGuid = Guid.NewGuid();

                // Upload to S3
                uploadStream.Position = 0;
                var uploadedImageId = await S3Uploader.UploadFileAsync(uploadStream, imageGuid, fileExtension);

                if (uploadedImageId.HasValue)
                {
                    using var dbStream = new MemoryStream();
                    uploadedImage.Save(dbStream, contentType == "image/jpeg" ? ImageFormat.Jpeg : ImageFormat.Png);
                    dbStream.Position = 0;

                    var imageRecordId = await ImageManager.UploadImageAndSaveToDatabase(
                        context,
                        dbStream,
                        $"{imageGuid}{fileExtension}",
                        contentType
                    );

                    if (imageRecordId.HasValue)
                    {
                        if (item.ImageId.HasValue)
                        {
                            // await ImageManager.DeleteImageFromDatabaseAndS3(...);
                        }

                        item.ImageId = imageRecordId.Value;
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("image error " + e.Message);
            }

            return false;
        }

        /// <summary>
        /// Disables all visible validation error labels on the form.
        /// </summary>
        private void DisableAllErrors()
        {
            lblNameError.Visible = false;
            lblPriceError.Visible = false;
            lblDescreptionError.Visible = false;
            lblImageError.Visible = false;
        }
        #endregion



        #region Event Handlers
        /// <summary>
        /// Handles the image label click event to allow the user to select and preview an image.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">Event arguments.</param>
        private void lblImage_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Select an Image",
                Filter = "Image files (*.jpg, *.jpeg, *.png) |*.jpg; *.jpeg; *.png",
                Multiselect = false,
            };


            if (ofd.ShowDialog() != DialogResult.OK) return;
            try
            {
                string filePath = ofd.FileName;
                string fileExtension = Path.GetExtension(filePath).ToLower();
                string contentType = S3Uploader.GetContentType(fileExtension);

                //Console.WriteLine($"Extension: {fileExtension}");
                //Console.WriteLine($"Content-Type: {contentType}");

                Image selectedImage = Image.FromFile(filePath);
                uploadedImage = selectedImage;
                imageExtention = fileExtension;
                imageFormat = contentType;
                pnlImage.BackgroundImage = selectedImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load the file, please try again", "Error loading image");
            }
        }

        #endregion
    }
}
