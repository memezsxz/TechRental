using Database.Core.Domain;
using Helper;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
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
     *     public partial class ManageRental : Form
     *
     * After making design changes, revert the base class back to 'BaseViewEditDeleteForm'
     * to preserve functionality and application behavior.
     */

    /// <summary>
    /// Form for managing rental requests and records, including return processing, document uploads,
    /// payment and status updates, and conditional form logic based on current item type.
    /// </summary>
    //public partial class ManageRental : Form
    public partial class ManageRental : BaseViewEditDeleteForm
    {
        /// <summary>
        /// Enum indicating whether the context is a rental request or a record.
        /// </summary>
        public enum ItemType
        {
            Request,
            Record
        }

        private RentalRecord record;
        private RentalRequest request;
        private Payment payment;
        private ItemType itemType;

        /// <summary>
        /// Initializes the ManageRental form in either request or record mode.
        /// </summary>
        /// <param name="itemType">Whether the item being managed is a request or record.</param>
        /// <param name="viewType">The operation type (Add, Edit, View).</param>
        /// <param name="id">ID of the rental request or record to load.</param>
        /// <param name="canDelete">Flag indicating if deletion is allowed.</param>
        public ManageRental(ItemType itemType, ViewType viewType, int? id, bool canDelete = false)
            : base(viewType, id, canDelete, switchType: false)
        {
            this.itemType = itemType;
            SwitchType(); // Manually call base setup
        }

        #region Form Initialization

        /// <inheritdoc/>
        protected override void InitializeForm()
        {
            InitializeComponent();
            DisableAllErrors();
            MapActionButtons(lblClose, lblSave, lblDelete);

            if (record != null)
            {
                LoadPaymentMethodDropDownList();
                LoadPaymentStatusDropDownList();
                LoadReturnConditionDropDownList();

                if (record.ActualReturnDate == null)
                {
                    gbReturn.Visible = false;
                }
                else
                {
                    pnlUploadDoc.Visible = false;
                    pnlDeleteDoc.Visible = false;
                }
            }
            else
            {
                gbFee.Visible = false;
                gbDocument.Visible = false;
                gbReturn.Visible = false;
                gbPayment.Visible = false;
            }

            LoadRequestStatusDropDownList();
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
            MessageBox.Show($"You cannot create a rental {(itemType == ItemType.Record ? "record" : "request")}.",
                "Operation Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        /// <inheritdoc/>
        protected override void PrepareForEdit()
        {
            LoadItemInfo();
            lblAction.Visible = false;

            // Disable request status dropdown if not pending
            if (request.Status?.StatusName.ToLower() != "pending")
            {
                ddlReqStatus.Enabled = false;
            }

            if (request.Status?.StatusName.ToLower() != "approved")
            {
                lblClose.Location = lblSave.Location;
                lblSave.Visible = false;
            }
            // Show "Start Transaction" or "Process Return" button based on context
            else if (request.Status?.StatusName.ToLower() == "approved" && record == null)
            {
                lblAction.Visible = true;
            }
            else if (record != null)
            {
                if (record.ActualReturnDate != null)
                {
                    lblClose.Location = lblSave.Location;
                    lblSave.Visible = false;
                }
                else
                {
                    lblAction.Visible = true;
                    lblAction.Text = "Process Return";
                }
            }
        }

        /// <summary>
        /// Populates the form fields with data from the current rental request and record.
        /// </summary>
        private void LoadItemInfo()
        {
            try
            {
                // Populate request data
                tbReqId.Text = request.Id.ToString();
                dtpReqStartDate.Value = request.StartDate;
                dtpReqEndDate.Value = request.ReturnDate;
                ddlReqStatus.SelectedValue = request.StatusId;
                tbReqNotes.Text = request.Notes;

                // Populate customer data
                var customer = request.Customer;
                tbCustId.Text = customer.Id.ToString();
                tbCustName.Text = $"{customer.FirstName} {customer.LastName}";
                tbCustEmail.Text = customer.Email;
                tbCustPhoneNumber.Text = customer.PhoneNumber;
                cbCustIsActive.Checked = customer.IsActive ?? false;

                // Populate equipment data
                var equipment = request.Equipment;
                tbEqId.Text = equipment.Id.ToString();
                tbEqName.Text = equipment.Name;
                cbEqIsActive.Checked = equipment.IsActive ?? false;

                if (record != null)
                {
                    gbFee.Visible = true;
                    gbDocument.Visible = true;
                    gbPayment.Visible = true;

                    // Populate record details
                    tbRecId.Text = record.Id.ToString();
                    dtpRecPickupDate.Value = record.PickupDate;
                    tbRecPrice.Text = request.RentalPerDay.ToString("C");
                    tbRecDeposit.Text = record.Deposit?.ToString("C") ?? "$0.00";

                    var payment = record.Payments.FirstOrDefault();

                    if (payment != null)
                    {
                        tbPatyId.Text = payment.Id.ToString();
                        tbPayTotal.Text = payment.Amount.ToString("C");
                        ddlPayMethod.SelectedValue = payment.PaymentMethodId;
                        ddlPayStatus.SelectedValue = payment.PaymentStatusId;
                    }
                    else
                    {
                        gbPayment.Visible = false;
                    }

                    // Populate return details
                    if (record.ActualReturnDate != null)
                    {
                        dtpRetDate.Value = record.ActualReturnDate.Value;
                        tbRetLateFee.Text = record.LateReturnFees?.ToString("C") ?? "$0.00";
                        ddlRetCondetion.SelectedValue = record.ReturnConditionId.Value;
                        tbRecExtraCharge.Text = record.ExtraCharges?.ToString("C") ?? "$0.00";
                        tbRecExtraChargeDescreption.Text = record.ExtraChargeDescription;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion
        #region Dropdown Loaders

        /// <summary>
        /// Loads available payment methods into the payment method dropdown.
        /// </summary>
        private void LoadPaymentMethodDropDownList()
        {
            ddlPayMethod.DisplayMember = "Value";
            ddlPayMethod.ValueMember = "Key";
            ddlPayMethod.DataSource = new BindingSource(context.PaymentMethods.GetAllByName(), null);
        }

        /// <summary>
        /// Loads return condition options into the return condition dropdown.
        /// </summary>
        private void LoadReturnConditionDropDownList()
        {
            ddlRetCondetion.DisplayMember = "Value";
            ddlRetCondetion.ValueMember = "Key";
            ddlRetCondetion.DataSource = new BindingSource(context.ReturnConditionStatuses.GetAllByName(), null);
        }

        /// <summary>
        /// Loads all payment statuses into the payment status dropdown.
        /// </summary>
        private void LoadPaymentStatusDropDownList()
        {
            ddlPayStatus.DisplayMember = "Value";
            ddlPayStatus.ValueMember = "Key";
            ddlPayStatus.DataSource = new BindingSource(context.PaymentStatuses.GetAllByName(), null);
        }

        /// <summary>
        /// Loads rental request status options into the request status dropdown.
        /// </summary>
        private void LoadRequestStatusDropDownList()
        {
            ddlReqStatus.DisplayMember = "Value";
            ddlReqStatus.ValueMember = "Key";
            ddlReqStatus.DataSource = new BindingSource(context.RentalRequestStatuses.GetAllByName(), null);
        }

        #endregion

        #region Data Loaders

        /// <inheritdoc/>
        protected override bool FetchItem()
        {
            if (itemType == ItemType.Request)
            {
                // Load request first, then attempt to load associated record and payment
                request = context.RentalRequests.GetWithRecordDetails(id.Value);
                if (request != null)
                {
                    record = context.RentalRecords.GetWithDetailsByRentalRequest(id.Value);
                    if (record != null)
                    {
                        payment = record.Payments.FirstOrDefault();
                    }
                    return true;
                }

                MessageBox.Show($"Rental request with the ID {id.Value} was not found.");
            }
            else if (itemType == ItemType.Record)
            {
                // Load record first, then load associated request and payment
                record = context.RentalRecords.GetWithDetails(id.Value);
                if (record != null)
                {
                    request = context.RentalRequests.GetWithRecordDetails(record.RentalRequestId);
                    payment = record.Payments.FirstOrDefault();
                    return true;
                }

                MessageBox.Show($"Rental record with the ID {id.Value} was not found.");
            }

            return false;
        }

        #endregion
        #region Save/Delete Logic

        /// <inheritdoc/>
        public override void Delete()
        {
            MessageBox.Show("Cannot delete a rental.");
            Dispose();
        }

        /// <inheritdoc/>
        protected override async Task SaveItem()
        {
            // Update timestamps and save either the record or the request
            if (record != null)
            {
                record.UpdatedAt = DateTime.Now;
                await StandardSave(
                    ValidateInput,
                    MapFormToEntity,
                    context.RentalRecords.Add,
                    context.RentalRecords.Update,
                    record,
                    record.Id,
                    itemType == ItemType.Request ? "Rental request" : "Rental record"
                );
            }
            else
            {
                request.UpdatedAt = DateTime.Now;
                await StandardSave(
                    ValidateInput,
                    MapFormToEntity,
                    context.RentalRequests.Add,
                    context.RentalRequests.Update,
                    request,
                    request.Id,
                    "Rental request"
                );
            }
        }

        /// <inheritdoc/>
        protected override void MapFormToEntity()
        {
            request.StatusId = (int)ddlReqStatus.SelectedValue;

            if (record != null)
            {
                if (record.Id == 0)
                {
                    // Set default payment amount for new record
                    payment.Amount = record.TotalCost;
                }

                if (record.ActualReturnDate != null)
                {
                    // Set return condition and extra charges if item was returned
                    record.ReturnConditionId = (int)ddlRetCondetion.SelectedValue;
                    record.ExtraCharges = decimal.Parse(tbRecExtraCharge.Text.Trim(), NumberStyles.Currency, CultureInfo.CurrentCulture);
                    record.ExtraChargeDescription = tbRecExtraChargeDescreption.Text.Trim();
                }
            }
        }

        #endregion

        #region Validation

        /// <summary>
        /// Validates form input including status changes, document existence, and return charge inputs.
        /// </summary>
        /// <returns>True if all inputs are valid; false otherwise.</returns>
        private bool ValidateInput()
        {
            DisableAllErrors();
            bool isValidInput = true;

            // Check for overlapping approved request
            if (request.StatusId != 2 && ((int)ddlReqStatus.SelectedValue) == 2)
            {
                isValidInput &= !context.RentalRequests.IsConflicted(
                    request.Id,
                    request.EquipmentId,
                    request.StartDate,
                    request.ReturnDate);

                if (!isValidInput)
                {
                    MessageBox.Show(
                        "This request conflicts with another approved rental for the same equipment.",
                        "Overlap Detected",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }

            // Document must be uploaded if request is being finalized
            if (record != null && request.Documents.FirstOrDefault() == null)
            {
                isValidInput = false;
                lblDocError.Visible = true;
            }

            // Validate extra charges if item was returned
            if (record is { ActualReturnDate: not null })
            {
                isValidInput &= ValidateExtraCharge();
            }
            return isValidInput;
        }

        /// <summary>
        /// Hides all validation error labels.
        /// </summary>
        private void DisableAllErrors()
        {
            lblRecExtraChargeDescreptionError.Visible = false;
            lblRecExtraChargeError.Visible = false;
            lblReqStatusError.Visible = false;
            lblDocError.Visible = false;
        }

        #endregion
        #region Transaction Logic

        /// <summary>
        /// Handles the logic for either starting a rental or processing its return.
        /// </summary>
        private void SwitchAction()
        {
            if (record == null)
            {
                if (DateTime.Now < request.StartDate)
                {
                    MessageBox.Show("Transaction cannot start before the start date.", "Transaction", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (DateTime.Now >= request.ReturnDate)
                {
                    MessageBox.Show("Transaction cannot start after or on the return date.", "Transaction", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (context.Equipment.IsInUse(request.EquipmentId))
                {
                    MessageBox.Show("The equipment is already in use and cannot be rented until it is returned.", "Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                InstansiatTransaction();
            }
            else if (record.ActualReturnDate == null)
            {
                ProcessReturn();
            }
        }

        /// <summary>
        /// Converts an approved rental request into an active rental record with initial values.
        /// </summary>
        private void InstansiatTransaction()
        {
            request.StatusId = 2;

            decimal rentalFee = (request.ReturnDate.Date - DateTime.Now.Date).Days * request.Equipment.RentalPricePerDay;
            decimal deposit = rentalFee * 0.1m;
            decimal total = rentalFee + deposit;

            record = new RentalRecord
            {
                Deposit = deposit,
                EquipmentName = request.Equipment.Name,
                PickupDate = DateTime.Now.Date,
                RentalRequestId = request.Id,
                RentalFee = rentalFee,
                TotalCost = total
            };

            payment = new Payment
            {
                PaymentDate = DateTime.Now,
                PaymentMethodId = 3, // TODO: make dynamic
                PaymentStatusId = 2,
                RentalRecord = record,
                Amount = record.TotalCost
            };

            record.Payments.Add(payment);

            ddlReqStatus.Enabled = false;
            lblAction.Visible = false;

            LoadPaymentMethodDropDownList();
            LoadPaymentStatusDropDownList();
            LoadItemInfo();
        }

        /// <summary>
        /// Processes the return of the equipment by enabling return fields and assigning late fees.
        /// </summary>
        private void ProcessReturn()
        {
            lblAction.Visible = false;
            gbReturn.Visible = true;

            ddlRetCondetion.Enabled = true;
            tbRecExtraCharge.Enabled = true;
            tbRecExtraChargeDescreption.Enabled = true;

            record.ActualReturnDate = DateTime.Now.Date;
            record.LateReturnFees = (record.ActualReturnDate.Value - request.ReturnDate).Days * request.RentalPerDay;
            if (record.LateReturnFees < 0) record.LateReturnFees = 0;
            record.ReturnConditionId = 1; // TODO: make dynamic

            LoadItemInfo();
        }

        #endregion

        #region Charges & Validation

        /// <summary>
        /// Validates the extra charge value and its corresponding description if applicable.
        /// </summary>
        /// <returns>True if the extra charge section is valid; otherwise false.</returns>
        private bool ValidateExtraCharge()
        {
            lblRecExtraChargeError.Visible = false;
            lblRecExtraChargeDescreptionError.Visible = false;

            bool isValid = ValidateNumericField<decimal>(
                tbRecExtraCharge.Text,
                lblRecExtraChargeError,
                "Extra charges",
                required: true,
                allowZero: true,
                allowNegative: false,
                parser: s => decimal.Parse(s, NumberStyles.Currency, CultureInfo.CurrentCulture),
                out var extra,
                minValue: 0m,
                maxValue: 9999
            );

            if (extra > 0)
            {
                isValid &= ValidateTextLength(
                    tbRecExtraChargeDescreption.Text,
                    lblRecExtraChargeDescreptionError,
                    "Description",
                    required: true,
                    minLength: 3,
                    maxLength: 255
                );
            }

            record.ExtraCharges = extra;
            return isValid;
        }

        /// <summary>
        /// Recalculates the total rental cost based on fees and extra charges.
        /// </summary>
        private void RecalculateCharges()
        {
            decimal rentalFee = (request.ReturnDate.Date - DateTime.Now.Date).Days * request.Equipment.RentalPricePerDay;
            decimal deposit = rentalFee * 0.1m;
            decimal extra = record.ExtraCharges ?? 0;
            decimal total = rentalFee + deposit + extra;

            record.TotalCost = total;
            payment.Amount = record.TotalCost;
            tbPayTotal.Text = payment.Amount.ToString("C");
        }

        #endregion
        #region Document Management

        /// <summary>
        /// Opens a dialog for the user to download the existing rental agreement or a new template if not found.
        /// </summary>
        public async void DownloadAndSaveAgreement()
        {
            try
            {
                Stream stream;
                var doc = request.Documents.FirstOrDefault();

                if (doc == null)
                {
                    stream = AgreementGenerator.GeneratePdf(request);
                }
                else
                {
                    stream = await S3Uploader.GetFileByGuidAsync(doc.Guid.ToString());

                    if (stream == null)
                    {
                        DialogResult result = MessageBox.Show(
                            "Saved agreement could not be found. Would you like to download the agreement template?",
                            "Download Failed",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question
                        );

                        if (result == DialogResult.Yes)
                        {
                            stream = AgreementGenerator.GeneratePdf(request);
                        }
                        else return;
                    }
                }

                using SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Title = "Save Rental Agreement",
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = $"Transaction_Record#{request.Id}_{request.CustomerId}.pdf"
                };

                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

                using var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                File.WriteAllBytes(saveFileDialog.FileName, memoryStream.ToArray());

                MessageBox.Show("Agreement saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Agreement could not be saved locally.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Opens a dialog to let the user upload a new PDF rental agreement and saves it to the cloud and database.
        /// </summary>
        public async void UploadPdfForRentalRequest()
        {
            try
            {
                using var openFileDialog = new OpenFileDialog
                {
                    Title = "Select PDF Agreement",
                    Filter = "PDF Files (*.pdf)|*.pdf"
                };

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                string filePath = openFileDialog.FileName;
                string fileName = Path.GetFileName(filePath);
                string contentType = "application/pdf";

                if (!DeleteDocs(showMessages: false))
                {
                    MessageBox.Show("Failed to upload document. Please try again.", "Upload Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                await using var fileStream = File.OpenRead(filePath);
                bool success = await PdfManager.UploadPdfAndSaveToDatabase(
                    context,
                    fileStream,
                    fileName,
                    contentType,
                    request.Id
                );

                if (success)
                {
                    MessageBox.Show("Document uploaded successfully.", "Upload Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblDocError.Visible = false;
                    request = context.RentalRequests.GetWithRecordDetails(request.Id);
                }
                else
                {
                    MessageBox.Show("Failed to upload document. Please ensure it's a valid PDF.", "Upload Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Global.DisplayReportErrorDialog(ex);
            }
        }

        /// <summary>
        /// Deletes all documents associated with the current rental request from both the cloud and database.
        /// </summary>
        /// <param name="showMessages">Whether to show result messages to the user.</param>
        /// <returns>True if all documents were deleted successfully; otherwise false.</returns>
        private bool DeleteDocs(bool showMessages)
        {
            List<Document> docs = request.Documents.ToList();

            if (!docs.Any())
            {
                if (showMessages) MessageBox.Show("No document uploaded to delete.");
                return true;
            }

            try
            {
                bool success = true;

                docs.ForEach(async d =>
                {
                    success &= await PdfManager.DeletePdfFromDatabaseAndS3(context, d.Id);
                });

                if (success)
                {
                    if (showMessages) MessageBox.Show("Document deleted successfully.", "Delete Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblDocError.Visible = false;
                    request = context.RentalRequests.GetWithRecordDetails(request.Id);
                    return true;
                }

                if (showMessages) MessageBox.Show("Failed to delete the document.", "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Global.DisplayReportErrorDialog(ex);
            }

            return false;
        }

        #endregion
        #region Event Handlers

        /// <summary>
        /// Paints a custom border for group boxes using specified colors.
        /// </summary>
        private void groupBox_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Global.DarkGreen, Global.Green);
        }

        /// <summary>
        /// Handles clicks on the action label to either start a transaction or process a return.
        /// </summary>
        private void lblStartTransaction_Click(object sender, EventArgs e)
        {
            SwitchAction();
        }

        /// <summary>
        /// Triggered when the return extra charge value changes — triggers live validation.
        /// </summary>
        private void tbRecExtraCharge_TextChanged(object sender, EventArgs e)
        {
            if (tbRecExtraCharge.Enabled) ValidateExtraCharge();
        }

        /// <summary>
        /// Triggered when the extra charge description changes — triggers live validation.
        /// </summary>
        private void tbRecExtraChargeDescreption_TextChanged(object sender, EventArgs e)
        {
            if (tbRecExtraChargeDescreption.Enabled) ValidateExtraCharge();
        }

        /// <summary>
        /// Opens download logic for rental agreement.
        /// </summary>
        private void pnlDownloadDoc_Click(object sender, EventArgs e)
        {
            DownloadAndSaveAgreement();
        }

        /// <summary>
        /// Opens file dialog and uploads a selected rental agreement PDF.
        /// </summary>
        private void pnlUploadDoc_Click(object sender, EventArgs e)
        {
            UploadPdfForRentalRequest();
        }

        /// <summary>
        /// Deletes all documents associated with the current rental request.
        /// </summary>
        private void pnlDeleteDoc_Click(object sender, EventArgs e)
        {
            DeleteDocs(true);
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Draws a custom-styled group box border and header with the specified colors.
        /// </summary>
        private void DrawGroupBox(GroupBox box, Graphics g, Color textColor, Color borderColor)
        {
            if (box == null) return;

            Brush textBrush = new SolidBrush(textColor);
            Brush borderBrush = new SolidBrush(borderColor);
            Pen borderPen = new Pen(borderBrush);
            SizeF strSize = g.MeasureString(box.Text, box.Font);
            Rectangle rect = new Rectangle(
                box.ClientRectangle.X,
                box.ClientRectangle.Y + (int)(strSize.Height / 2),
                box.ClientRectangle.Width - 1,
                box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1
            );

            // Clear background and draw text
            g.Clear(this.BackColor);
            g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);

            // Draw borders
            g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height)); // Left
            g.DrawLine(borderPen, new Point(rect.Right, rect.Y), new Point(rect.Right, rect.Bottom)); // Right
            g.DrawLine(borderPen, new Point(rect.X, rect.Bottom), new Point(rect.Right, rect.Bottom)); // Bottom
            g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y)); // Top 1
            g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)strSize.Width, rect.Y), new Point(rect.Right, rect.Y)); // Top 2
        }

        #endregion
    }
}
