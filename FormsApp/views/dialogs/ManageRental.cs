using Database.Core.Domain;
using Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetEnv;
using Database.Persistence;

namespace FormsApp.views.dialogs
{
    //public partial class ManageRental : Form
    public partial class ManageRental : BaseViewEditDeleteForm
    {
        public enum ItemType
        {
            Request,
            Record
        }

        private RentalRecord record;
        private RentalRequest request;
        private Payment payment;

        ItemType itemType;


        public ManageRental(ItemType itemType, ViewType viewType, int? id, bool canDelete = false) : base(viewType, id, canDelete, false)
        {
            this.itemType = itemType;
            SwitchType();
        }

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

        protected override void PrepareForView()
        {
            base.PrepareForView();
            LoadItemInfo();
        }

        protected override void PrepareForAdd()
        {
            MessageBox.Show($"You cannot create a rental {(itemType == ItemType.Record ? "request" : "record")}  ");
            Close();
            return;

            lblSave.Text = "Add";
            lblDelete.Visible = false;
            lblAction.Visible = false;
        }
        protected override void PrepareForEdit()
        {
            LoadItemInfo();
            lblAction.Visible = false;

            if (request.Status?.StatusName.ToLower() != "pending") //TODO Maryam: make a method to retrive the status
            {
                ddlReqStatus.Enabled = false;
            }

            if (request.Status?.StatusName.ToLower() == "approved" && record == null)
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
        /// Loads data from the item into the form controls.
        /// </summary>
        private void LoadItemInfo()
        {
            try
            {
                // request data
                tbReqId.Text = request.Id.ToString();
                dtpReqStartDate.Value = request.StartDate;
                dtpReqEndDate.Value = request.ReturnDate;
                ddlReqStatus.SelectedValue = request.StatusId;
                tbReqNotes.Text = request.Notes;

                // customer data
                var customer = request.Customer;
                tbCustId.Text = customer.Id.ToString();
                tbCustName.Text = $"{customer.FirstName} {customer.LastName}";
                tbCustEmail.Text = customer.Email;
                tbCustPhoneNumber.Text = customer.PhoneNumber;
                cbCustIsActive.Checked = customer.IsActive ?? false;

                // equipment data
                var equipment = request.Equipment;
                tbEqId.Text = equipment.Id.ToString();
                tbEqName.Text = equipment.Name;
                cbEqIsActive.Checked = equipment.IsActive ?? false;

                if (record != null)
                {
                    gbFee.Visible = true;
                    gbDocument.Visible = true;
                    gbPayment.Visible = true;

                    // record data
                    tbRecId.Text = record.Id.ToString();
                    dtpRecPickupDate.Value = record.PickupDate;
                    tbRecPrice.Text = request.RentalPerDay.ToString("C");
                    tbRecDeposit.Text = record.Deposit?.ToString("C") ?? "$0.00";
                    // fee data

                    var payment = record.Payments.FirstOrDefault();

                    if (payment != null)
                    {
                        tbPatyId.Text = payment.Id.ToString();
                        tbPayTotal.Text = payment.Amount.ToString("C");
                        ddlPayMethod.SelectedValue = payment.PaymentMethodId;
                        ddlPayStatus.SelectedValue = payment.PaymentStatusId;

                        var paidStatusId = context.PaymentStatuses.GetAllByName().First(kv => kv.Value.ToLower() == "paid").Key;

                        //if (payment.PaymentStatusId == paidStatusId)
                        //{
                        //    dtpRecPickupDate.Enabled = true;
                        //}
                        //else
                        //{
                        //}

                    }
                    else
                    {
                        gbPayment.Visible = false;
                    }

                    // return data

                    if (record.ActualReturnDate != null)
                    {
                        dtpRetDate.Value = record.ActualReturnDate.Value;
                        tbRetLateFee.Text = record.LateReturnFees.Value.ToString("C");
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
        /// Populates the Availability dropdown list with values from the database.
        /// </summary>

        private void LoadPaymentMethodDropDownList()
        {
            ddlPayMethod.DisplayMember = "Value";
            ddlPayMethod.ValueMember = "Key";

            ddlPayMethod.DataSource = new BindingSource(context.PaymentMethods.GetAllByName(), null);
        }
        /// <summary>
        /// Populates the Return Condition dropdown list with values from the database.
        /// </summary>

        private void LoadReturnConditionDropDownList()
        {
            ddlRetCondetion.DisplayMember = "Value";
            ddlRetCondetion.ValueMember = "Key";

            ddlRetCondetion.DataSource = new BindingSource(context.ReturnConditionStatuses.GetAllByName(), null);
        }
        /// <summary>
        /// Populates the Payment Status dropdown list with values from the database.
        /// </summary>

        private void LoadPaymentStatusDropDownList()
        {
            ddlPayStatus.DisplayMember = "Value";
            ddlPayStatus.ValueMember = "Key";

            ddlPayStatus.DataSource = new BindingSource(context.PaymentStatuses.GetAllByName(), null);
        }

        /// <summary>
        /// Populates the Payment Status dropdown list with values from the database.
        /// </summary>

        private void LoadRequestStatusDropDownList()
        {
            ddlReqStatus.DisplayMember = "Value";
            ddlReqStatus.ValueMember = "Key";

            ddlReqStatus.DataSource = new BindingSource(context.RentalRequestStatuses.GetAllByName(), null);
        }


        #endregion

        #region Data Loaders
        protected override bool FetchItem()
        {
            if (itemType == ItemType.Request)
            {
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
                MessageBox.Show($"Rental request with the id {id.Value} not found");
            }
            else if (itemType == ItemType.Record)
            {
                record = context.RentalRecords.GetWithDetails(id.Value);
                if (record != null)
                {
                    request = context.RentalRequests.GetWithRecordDetails(record.RentalRequestId);
                    payment = record.Payments.FirstOrDefault();
                    return true;
                }
                MessageBox.Show($"Rental record with the id {id.Value} not found");
            }

            return false;
        }

        #endregion


        #region Save/Delete Logic
        public override void Delete()
        {
            MessageBox.Show("Cannot Delete a rental.");
            Dispose();
        }


        protected override async Task SaveItem()
        {
            //Console.WriteLine($"record {record.Id}           request {request.Id}");
            if (record != null)
            {
                //Console.WriteLine("record");
                record.UpdatedAt = DateTime.Now;
                await StandardSave<RentalRecord>(
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
                //Console.WriteLine("request");
                await StandardSave<RentalRequest>(
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

        protected override void MapFormToEntity()
        {
            request.StatusId = (int)ddlReqStatus.SelectedValue;


            if (record != null)
            {
                if (record.Id == 0)
                {
                    payment.Amount = record.TotalCost;
                }

                if (record.ActualReturnDate != null)
                {
                    record.ReturnConditionId = (int)ddlRetCondetion.SelectedValue;
                    record.ExtraCharges = decimal.Parse(tbRecExtraCharge.Text.Trim(), NumberStyles.Currency,
                        CultureInfo.CurrentCulture);
                    record.ExtraChargeDescription = tbRecExtraChargeDescreption.Text.Trim();
                }

            }
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

            if (request.StatusId != 2 && ((int)ddlReqStatus.SelectedValue) == 2)
            {
                isValidInput &= !context.RentalRequests.IsConflicted(request.Id, request.EquipmentId, request.StartDate,
                    request.ReturnDate);
                if (!isValidInput)
                {
                    MessageBox.Show("This request conflicts with another approved rental for the same equipment.", "Overlap Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }


            if (record != null && request.Documents.FirstOrDefault() == null)
            {
                isValidInput = false;
                lblDocError.Visible = true;
            }

            if (record is { ActualReturnDate: not null })
            {
                isValidInput &= ValidateExtraCharge();
            }


            Console.WriteLine($"Final validation result: {isValidInput}");

            return isValidInput;
        }


        /// <summary>
        /// Disables all visible validation error labels on the form.
        /// </summary>
        private void DisableAllErrors()
        {
            lblRecExtraChargeDescreptionError.Visible = false;
            lblRecExtraChargeError.Visible = false;
            lblRecExtraChargeError.Visible = false;
            lblReqStatusError.Visible = false;
            lblDocError.Visible = false;
        }
        #endregion

        #region Event Handlers
        private void groupBox_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Global.DarkGreen, Global.Green);
        }


        private void lblStartTransaction_Click(object sender, EventArgs e)
        {
            SwitchAction();
        }

        private void SwitchAction()
        {
            if (record == null)
            {
                if (DateTime.Now < request.StartDate)
                {
                    MessageBox.Show("Transaction cannot start before the start date.", "Transation",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (DateTime.Now >= request.ReturnDate)
                {
                    MessageBox.Show("Transaction cannot start after or on the return date.", "Transation",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (context.Equipment.IsInUse(request.EquipmentId))
                {
                    MessageBox.Show("The equipment is already in use and cannot be rented until it is returned.", "Conflict",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                InstansiatTransaction();
            }
            else if (record.ActualReturnDate == null)
            {
                ProcessReturn();
            }
        }

        private void tbRecExtraCharge_TextChanged(object sender, EventArgs e)
        {
            if (tbRecExtraCharge.Enabled) ValidateExtraCharge();
        }
        private void tbRecExtraChargeDescreption_TextChanged(object sender, EventArgs e)
        {
            if (tbRecExtraChargeDescreption.Enabled) ValidateExtraCharge();
        }
        #endregion

        #region Utilities
        private void DrawGroupBox(GroupBox box, Graphics g, Color textColor, Color borderColor)
        {

            if (box != null)
            {
                Brush textBrush = new SolidBrush(textColor);
                Brush borderBrush = new SolidBrush(borderColor);
                Pen borderPen = new Pen(borderBrush);
                SizeF strSize = g.MeasureString(box.Text, box.Font);
                Rectangle rect = new Rectangle(box.ClientRectangle.X,
                    box.ClientRectangle.Y + (int)(strSize.Height / 2),
                    box.ClientRectangle.Width - 1,
                    box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);

                // Clear text and border
                g.Clear(this.BackColor);

                // Draw text
                g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);

                // Drawing Border
                //Left
                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                //Right
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Bottom
                g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Top1
                g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                //Top2
                g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }
        #endregion


        private void InstansiatTransaction()
        {
            request.StatusId = 2;

            decimal rentalFee = (request.ReturnDate.Date - DateTime.Now.Date).Days *
                                request.Equipment.RentalPricePerDay;
            decimal deposit = rentalFee * 0.1m;
            decimal total = rentalFee + deposit;

            RentalRecord rec = new RentalRecord()
            {
                Deposit = deposit,
                EquipmentName = request.Equipment.Name,
                PickupDate = DateTime.Now.Date,
                RentalRequestId = request.Id,
                RentalFee = rentalFee,
                TotalCost = total
            };

            record = rec;
            //request.RentalRecords.Add(record);

            Payment pay = new Payment()
            {
                PaymentDate = DateTime.Now,
                PaymentMethodId = 3, // TODO Maryam: create a get method for the status id
                PaymentStatusId = 2,
                RentalRecord = record,
                Amount = record.TotalCost
            };

            payment = pay;
            record.Payments.Add(payment);

            ddlReqStatus.Enabled = false;
            lblAction.Visible = false;

            LoadPaymentMethodDropDownList();
            LoadPaymentStatusDropDownList();
            LoadItemInfo();
        }


        private bool ValidateExtraCharge()
        {
            lblRecExtraChargeError.Visible = false;
            lblRecExtraChargeDescreptionError.Visible = false;

            bool isValid = true;
            isValid &= ValidateNumericField<decimal>(
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
                isValid &= ValidateTextLength(tbRecExtraChargeDescreption.Text, lblRecExtraChargeDescreptionError, "Description", true, 3, 255);
            }

            record.ExtraCharges = extra;

            //RecalculateCharges();

            return isValid;
        }

        private void RecalculateCharges()
        {
            decimal rentalFee = (request.ReturnDate.Date - DateTime.Now.Date).Days *
                                request.Equipment.RentalPricePerDay;
            decimal deposit = rentalFee * 0.1m;
            decimal extra = record.ExtraCharges ?? 0;
            decimal total = rentalFee + deposit + extra;

            record.TotalCost = total;
            payment.Amount = record.TotalCost;

            tbPayTotal.Text = payment.Amount.ToString("C");

        }

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
            record.ReturnConditionId = 1; // TODO Maryam: create a method to retrive the status

            LoadItemInfo();
        }

        private async void pnlDownloadDoc_Click(object sender, EventArgs e)
        {
            DownloadAndSaveAgreement();
        }

        private void pnlUploadDoc_Click(object sender, EventArgs e)
        {
            UploadPdfForRentalRequest();
        }


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
                        else
                        {
                            return;
                        }
                    }
                }

                using SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Title = "Save Rental Agreement",
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = $"Transaction_Record#{request.Id}_{request.CustomerId}.pdf"
                };

                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    File.WriteAllBytes(saveFileDialog.FileName, memoryStream.ToArray());
                }

                MessageBox.Show("Agreement saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Agreement could not be saved locally.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


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


                if (!DeleteDocs(false))
                {
                    MessageBox.Show("Failed to upload document. Please try again.", "Upload Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
              return;  }


                await using var fileStream = File.OpenRead(filePath);
                bool success = await PdfManager.UploadPdfAndSaveToDatabase(
                    context: context,
                    fileStream: fileStream,
                    fileName: fileName,
                    contentType: contentType,
                    rentalRequestId: request.Id
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

        private async void pnlDeleteDoc_Click(object sender, EventArgs e)
        {
            DeleteDocs(true);
        }


        private bool DeleteDocs(bool showMessages)
        {
            List<Document> docs = request.Documents.ToList();

            if (docs.Any() == false)
            {
                if (showMessages) MessageBox.Show("No documeent uploaded to delete.");
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
                else
                {
                    if (showMessages) MessageBox.Show("Failed to delete the document.", "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Global.DisplayReportErrorDialog(ex);
            }

            return false;
        }
    }
}
