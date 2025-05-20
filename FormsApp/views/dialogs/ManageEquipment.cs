using Database.Core.Domain;
using System.Globalization;
using Image = System.Drawing.Image;
using Helper;
using System.Drawing.Imaging;


namespace FormsApp.views.dialogs
{
    /*
     * WARNING:
     * To view this form in the Windows Forms Designer,
     * temporarily change the base class from 'BaseViewEditDeleteForm' to 'Form'.
     *
     * Example:
     *     public partial class ManageEquipment : Form
     *
     * After making design changes, revert the base class back to 'BaseViewEditDeleteForm'
     * to preserve functionality and application behavior.
     */

    /// <summary>
    /// Manages equipment data for Add, Edit, and Delete operations in the Equipment Rental Management System.
    /// </summary>
    //public partial class ManageEquipment : Form
    public partial class ManageEquipment : BaseViewEditDeleteForm
    {
        #region Fields

        /// <summary>
        /// The current equipment item loaded or being created.
        /// </summary>
        private Equipment item;

        /// <summary>
        /// Holds the selected image from the user (if uploading).
        /// </summary>
        private Image uploadedImage;

        /// <summary>
        /// MIME type of the uploaded image (e.g., "image/png").
        /// </summary>
        private string imageFormat;

        /// <summary>
        /// File extension of the uploaded image (e.g., ".png").
        /// </summary>
        private string imageExtention;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the ManageEquipment form.
        /// </summary>
        /// <param name="viewType">Mode of interaction (Add, Edit, or View).</param>
        /// <param name="id">Optional ID of the equipment to load for edit/view.</param>
        /// <param name="canDelete">If true, enables the delete button for this form instance.</param>
        public ManageEquipment(ViewType viewType, int? id, bool canDelete = false)
            : base(viewType, id, canDelete) { }

        #endregion

        #region Form Initialization

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        protected override void PrepareForView()
        {
            base.PrepareForView();

            // Disable all input fields to prevent changes
            foreach (Control control in this.Controls)
            {
                if (control is TextBox or ComboBox or CheckBox)
                    control.Enabled = false;
            }

            // Disable image selection in view mode
            lblImage.Click -= lblImage_Click;
            LoadItemInfo();
        }

        /// <inheritdoc/>
        protected override void PrepareForAdd()
        {
            lblSave.Text = "Add";
            lblDelete.Visible = false;
            item = new Equipment();
        }

        /// <inheritdoc/>
        protected override void PrepareForEdit()
        {
            LoadItemInfo();
            if (canDelete) deleteLabel.Visible = true;
        }

        /// <summary>
        /// Loads the current equipment item data into form controls.
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
        /// Populates the Availability dropdown list with all availability statuses from the database.
        /// </summary>
        private void LoadAvailabilityDropDownList()
        {
            ddlAvalability.DisplayMember = "Value";
            ddlAvalability.ValueMember = "Key";
            ddlAvalability.DataSource = new BindingSource(context.EquipmentAvailabilityStatuses.GetAllByName(), null);
        }

        /// <summary>
        /// Populates the Condition dropdown list with all condition statuses from the database.
        /// </summary>
        private void LoadConditionDropDownList()
        {
            ddlCondition.DisplayMember = "Value";
            ddlCondition.ValueMember = "Key";
            ddlCondition.DataSource = new BindingSource(context.EquipmentConditionStatuses.GetAllByName(), null);
        }

        /// <summary>
        /// Populates the Category dropdown list with all equipment categories from the database.
        /// </summary>
        private void LoadCategoryDropDownList()
        {
            ddlCategory.DisplayMember = "Value";
            ddlCategory.ValueMember = "Key";
            ddlCategory.DataSource = new BindingSource(context.Categories.GetAllByName(), null);
        }

        #endregion

        #region Data Loaders

        /// <inheritdoc/>
        protected override bool FetchItem()
        {
            item = context.Equipment.GetEquipmentWithImage(id.Value);

            if (item != null) return true;

            MessageBox.Show($"Equipment with the ID {id.Value} was not found.");
            return false;
        }

        #endregion

        #region Save/Delete Logic

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        protected override async Task SaveItem()
        {
            await StandardSaveAsync<Equipment>(
                ValidateInput,
                MapFormToEntity,
                context.Equipment.Add,
                context.Equipment.Update,
                item,
                item.Id,
                "Equipment",
                Global.userID
            );
        }

        /// <inheritdoc/>
        protected override void MapFormToEntity()
        {
            item.Name = lblName.Text.Trim();
            item.RentalPricePerDay = decimal.Parse(lblPrice.Text.Trim(), NumberStyles.Currency, CultureInfo.CurrentCulture);
            item.AvailabilityStatusId = (int)ddlAvalability.SelectedValue;
            item.ConditionStatusId = (int)ddlCondition.SelectedValue;
            item.CategoryId = (int)ddlCategory.SelectedValue;
            item.Description = tbDescription.Text.Trim();
            item.IsActive = cbIsActive.Checked;
            item.UpdatedAt = DateTime.Now;

            // Note: image assignment handled in validation if uploaded
        }

        #endregion

        #region Validation and Image Upload

        /// <summary>
        /// Validates all form fields including name, description, price, and image.
        /// If an image is selected, uploads it to storage and database.
        /// </summary>
        /// <returns>True if validation passes and image upload (if applicable) succeeds; false otherwise.</returns>
        private async Task<bool> ValidateInput()
        {
            DisableAllErrors();
            bool isValidInput = true;

            // Validate Name field
            isValidInput &= ValidateTextLength(
                lblName.Text, lblNameError, "Name", required: true, minLength: 3, maxLength: 100);

            // Validate Description field
            isValidInput &= ValidateTextLength(
                tbDescription.Text, lblDescreptionError, "Description", required: true, minLength: 3, maxLength: 255);

            // Validate numeric price field
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

            // Ensure at least one image exists (uploaded or already assigned)
            if (uploadedImage == null && item.Image == null)
            {
                isValidInput &= ActivateError(lblImageError, "Please Upload an Image");
            }

            // If new image selected, attempt upload
            if (uploadedImage != null)
            {
                bool didUpload = await UploadImage();
                if (!didUpload) isValidInput &= ActivateError(lblImageError, "Unable to upload image, try again");
            }

            return isValidInput;
        }

        /// <summary>
        /// Uploads the selected image to cloud storage and saves the metadata in the database.
        /// </summary>
        /// <returns>True if both cloud and database upload succeed; otherwise, false.</returns>
        private async Task<bool> UploadImage()
        {
            try
            {
                using var uploadStream = new MemoryStream();
                uploadedImage.Save(uploadStream, ImageFormat.Png);

                string fileExtension = imageExtention;
                string contentType = imageFormat;
                Guid imageGuid = Guid.NewGuid();

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
                        // Replace old image if present
                        // await ImageManager.DeleteImageFromDatabaseAndS3(...);
                        item.ImageId = imageRecordId.Value;
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Image upload error: " + e.Message);
            }

            return false;
        }

        /// <summary>
        /// Hides all error labels related to validation.
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
        /// Allows the user to select and preview an image for the equipment item.
        /// </summary>
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

                Image selectedImage = Image.FromFile(filePath);
                uploadedImage = selectedImage;
                imageExtention = fileExtension;
                imageFormat = contentType;

                // Display image in panel
                pnlImage.BackgroundImage = selectedImage;
            }
            catch
            {
                MessageBox.Show("Could not load the file. Please try again.", "Error loading image");
            }
        }

        #endregion
    }
}
