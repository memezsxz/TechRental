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

        RentalRecord record;
        RentalRequest request;
        private Payment payment;

        ItemType itemType;
        public ManageRental(ItemType itemType, ViewType viewType, int? id) : base(viewType, id)
        {
            this.itemType = itemType;
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
            MapActionButtons();
            if (itemType == ItemType.Record) request = record.RentalRequest;
            if (itemType == ItemType.Request) record = context.RentalRecords.GetWithDetailsByRentalRequest(id.Value);

            if (record != null)
            {
                LoadPaymentMethodDropDownList();
                LoadPaymentStatusDropDownList();
                LoadReturnConditionDropDownList();

                if (record.ActualReturnDate == null)
                {
                    gbReturn.Visible = false;
                }
            }
            else
            {
                gbFee.Visible = false;
                gbReturn.Visible = false;
                gbPayment.Visible = false;
            }

            LoadRequestStatusDropDownList();
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
            MessageBox.Show($"You cannot create a rental {(itemType == ItemType.Record ? "request" : "record")}  ");
            Close();
            return;

            lblSave.Text = "Add";
            lblDelete.Visible = false;
            lblStartTransaction.Visible = false;
        }
        protected override void PrepareForEdit()
        {
            LoadItemInfo();
            lblStartTransaction.Visible = false;

            if (request.Status.StatusName.ToLower() != "pending") //TODO Maryam: make a method to retrive the status
            {
                ddlReqStatus.Enabled = false;
            }

            if (request.Status.StatusName.ToLower() == "approved" && record == null)
            {
                lblStartTransaction.Visible = true;
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
                    lblStartTransaction.Visible = true;
                    lblStartTransaction.Text = "Process Return";

                }
            }


        }

        /// <summary>
        /// Loads data from the item into the form controls.
        /// </summary>
        private void LoadItemInfo()
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
                gbPayment.Visible = true;

                // record data
                tbRecId.Text = record.Id.ToString();
                dtpRecPickupDate.Value = record.PickupDate;
                tbRecPrice.Text = request.RentalPerDay.Value.ToString("C");
                tbRecDeposit.Text = record.Deposit.Value.ToString("C");
                tbRecExtraCharge.Text = record.ExtraCharges.Value.ToString("C");
                tbRecExtraChargeDescreption.Text = record.ExtraChargeDescription;

                // fee data

                var payment = record.Payments.FirstOrDefault();

                if (payment != null)
                {
                    tbPatyId.Text = payment.Id.ToString();
                    tbPayTotal.Text = payment.Amount.ToString("C");
                    ddlPayMethod.SelectedValue = payment.PaymentMethodId;
                    ddlPayStatus.SelectedValue = payment.PaymentStatusId;

                    var paidStatusId = context.PaymentStatuses.GetAllByName().Where(kv => kv.Value.ToLower() == "paid").First().Key;

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
                }

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
                    if (record != null) payment = record.Payments.FirstOrDefault();

                    return true;
                }
                MessageBox.Show($"Rental request with the id {id.Value} not found");
            }
            else if (itemType == ItemType.Record)
            {
                record = context.RentalRecords.GetWithDetails(id.Value);
                if (record != null)
                {
                    request = context.RentalRequests.GetWithRecordDetails(record.RentalRequestId.Value);
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
            //if (id == null)
            //{
            //    MessageBox.Show("Cannot delete id null");
            //Dispose();

            //    return;
            //}

            //item = context.Equipment.GetEquipmentWithImage(id.Value);

            //if (item == null)
            //{
            //    MessageBox.Show($"Equipment with the id {id.Value} not found");
            //                    Dispose();
            //return;
            //}

            //if (context.Equipment.IsReferenced(id.Value))
            //{
            //    var result = MessageBox.Show($"This equipment is in use. Do you want to mark it as inactive instead?", "Equipment in use", MessageBoxButtons.YesNo);

            //    if (result == DialogResult.Yes)
            //    {
            //        try
            //        {
            //            item.IsActive = false;
            //            context.Equipment.Update(item);
            //            context.SaveChanges();
            //            RaiseSuccessfulComplete();
            //        }
            //        catch (Exception e)
            //        {
            //            Global.DisplayReportErrorDialog(e);
            //            RaiseFailedComplete();
            //        }
            //    }
            //                    Dispose();

            //    return;
            //}

            //try
            //{
            //    context.Equipment.Remove(item);
            //    context.SaveChanges();
            //    RaiseSuccessfulComplete();
            //Dispose();

            //}
            //catch (Exception e)
            //{
            //    Global.DisplayReportErrorDialog(e);
            //    RaiseFailedComplete();
            //Dispose();

            //}

        }
        protected override async Task SaveItem()
        {

            if (!await ValidateInput()) return;

            Console.WriteLine("1");
            try
            {
                //request.UpdatedAt = DateTime.UtcNow;

                //record.UpdatedAt = DateTime.UtcNow;

                if (FormViewType == ViewType.EDIT)
                {
                    context.RentalRequests.Update(request);
                    Console.WriteLine("2");

                }
                else if (FormViewType == ViewType.ADD)
                {
                    context.RentalRequests.Add(request);
                    Console.WriteLine("3");
                }

                int rows = await context.SaveChangesAsync();

                if (rows > 0)
                {
                    MessageBox.Show($"Request {(FormViewType == ViewType.ADD ? "added" : "updated")} successfully");
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

            Console.WriteLine(record == null ? "record is null" : $"record.Id = {record.Id}");

            if (record != null && record.Id == 0)
            {
                isValidInput &= ValidateExtraCharge();
            }

            Console.WriteLine(record == null ? "record is null" : $"record.Id = {record.Id}");


            Console.WriteLine($"Final validation result: {isValidInput}");


            if (!isValidInput) return false;

            request.StatusId = (int)ddlReqStatus.SelectedValue;
            Console.WriteLine(record == null ? "record is null" : $"record.Id = {record.Id}");
            Console.WriteLine(record.TotalCost.Value);


            if (record != null && record.Id == 0)
            {
                record.ExtraCharges = decimal.Parse(tbRecExtraCharge.Text.Trim(), NumberStyles.Currency, CultureInfo.CurrentCulture);
                record.ExtraChargeDescription = tbRecExtraChargeDescreption.Text.Trim();
                Console.WriteLine(record == null ? "record is null" : $"record.Id = {record.Id}");

                payment.Amount = record.TotalCost.Value;
                Console.WriteLine("here");
            }

            return true;

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
        }
        #endregion

        #region Event Handlers
        private void groupBox_Paint(object sender, PaintEventArgs e)
        {
            // no border
            //GroupBox box = (GroupBox)sender;
            //e.Graphics.Clear(SystemColors.Control);
            //e.Graphics.DrawString(box.Text, box.Font, new SolidBrush(Global.DarkGreen), 0, 0);


            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Global.DarkGreen, Global.Green);
        }


        private void lblStartTransaction_Click(object sender, EventArgs e)
        {
            if (record == null)
            {
                InstansiatTransaction();

            }
            else if (record.ActualReturnDate == null)
            {
                ProcessReturn();
            }
        }

        private void tbRecExtraCharge_TextChanged(object sender, EventArgs e)
        {
            ValidateExtraCharge();
        }
        private void tbRecExtraChargeDescreption_TextChanged(object sender, EventArgs e)
        {
            ValidateExtraCharge();

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
                ExtraCharges = 0,
                PickupDate = DateTime.Now.Date,
                RentalRequestId = request.Id,
                RentalFee = rentalFee,
                TotalCost = total
            };

            record = rec;
            request.RentalRecords.Add(record);

            Payment pay = new Payment()
            {
                PaymentDate = DateTime.Now,
                PaymentMethodId = 3, // TODO Maryam: create a get method for the status id
                PaymentStatusId = 2,
                RentalRecord = record,
                Amount = record.TotalCost.Value
            };

            payment = pay;
            record.Payments.Add(payment);

            ddlReqStatus.Enabled = false;
            lblStartTransaction.Visible = false;

            tbRecExtraCharge.Enabled = true;
            tbRecExtraChargeDescreption.Enabled = true;

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

            RecalculateCharges();

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
            payment.Amount = record.TotalCost.Value;

            tbPayTotal.Text = payment.Amount.ToString("C");

        }

        private void ProcessReturn()
        {
            lblStartTransaction.Visible = false;
            gbReturn.Visible = true;
            ddlRetCondetion.Enabled = true;

            record.ActualReturnDate = DateTime.Now.Date;
            record.LateReturnFees = (record.ActualReturnDate.Value -  request.ReturnDate).Days * request.RentalPerDay;
            if (record.LateReturnFees < 0) record.LateReturnFees = 0;
            record.ReturnConditionId = 1; // TODO Maryam: create a method to retrive the status

            LoadItemInfo();
        }
    }
}
