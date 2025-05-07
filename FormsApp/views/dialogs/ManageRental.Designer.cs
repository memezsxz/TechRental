namespace FormsApp.views.dialogs
{
    partial class ManageRental
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblClose = new Label();
            lblDelete = new Label();
            lblSave = new Label();
            tbReqNotes = new TextBox();
            ddlReqStatus = new ComboBox();
            tbRecPrice = new TextBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label10 = new Label();
            dtpReqStartDate = new DateTimePicker();
            dtpReqEndDate = new DateTimePicker();
            label2 = new Label();
            gbRequest = new GroupBox();
            lblReqStatusError = new Label();
            label12 = new Label();
            tbReqId = new TextBox();
            gbCustomer = new GroupBox();
            cbCustIsActive = new CheckBox();
            tbCustPhoneNumber = new TextBox();
            label16 = new Label();
            tbCustName = new TextBox();
            tbCustEmail = new TextBox();
            label15 = new Label();
            label13 = new Label();
            label14 = new Label();
            tbCustId = new TextBox();
            gbEquipment = new GroupBox();
            cbEqIsActive = new CheckBox();
            tbEqName = new TextBox();
            label19 = new Label();
            label20 = new Label();
            tbEqId = new TextBox();
            gbPayment = new GroupBox();
            tbPayTotal = new TextBox();
            label30 = new Label();
            ddlPayStatus = new ComboBox();
            label17 = new Label();
            ddlPayMethod = new ComboBox();
            label11 = new Label();
            label1 = new Label();
            tbPatyId = new TextBox();
            gbFee = new GroupBox();
            lblRecExtraChargeError = new Label();
            lblRecExtraChargeDescreptionError = new Label();
            tbRecExtraCharge = new TextBox();
            label28 = new Label();
            tbRecDeposit = new TextBox();
            label21 = new Label();
            label18 = new Label();
            tbRecId = new TextBox();
            dtpRecPickupDate = new DateTimePicker();
            label23 = new Label();
            tbRecExtraChargeDescreption = new TextBox();
            label24 = new Label();
            gbReturn = new GroupBox();
            tbRetLateFee = new TextBox();
            label8 = new Label();
            dtpRetDate = new DateTimePicker();
            label26 = new Label();
            ddlRetCondetion = new ComboBox();
            label33 = new Label();
            lblStartTransaction = new Label();
            gbRequest.SuspendLayout();
            gbCustomer.SuspendLayout();
            gbEquipment.SuspendLayout();
            gbPayment.SuspendLayout();
            gbFee.SuspendLayout();
            gbReturn.SuspendLayout();
            SuspendLayout();
            // 
            // lblClose
            // 
            lblClose.BackColor = Color.White;
            lblClose.Font = new Font("Cascadia Mono", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblClose.ForeColor = Color.FromArgb(60, 173, 104);
            lblClose.Location = new Point(994, 962);
            lblClose.Margin = new Padding(0);
            lblClose.Name = "lblClose";
            lblClose.Size = new Size(122, 48);
            lblClose.TabIndex = 48;
            lblClose.Text = "Close";
            lblClose.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDelete
            // 
            lblDelete.BackColor = Color.Red;
            lblDelete.Font = new Font("Cascadia Mono", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblDelete.ForeColor = Color.White;
            lblDelete.Location = new Point(21, 962);
            lblDelete.Margin = new Padding(0);
            lblDelete.Name = "lblDelete";
            lblDelete.Size = new Size(122, 48);
            lblDelete.TabIndex = 47;
            lblDelete.Text = "Delete";
            lblDelete.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSave
            // 
            lblSave.BackColor = Color.FromArgb(60, 173, 104);
            lblSave.Font = new Font("Cascadia Mono", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblSave.ForeColor = Color.White;
            lblSave.Location = new Point(1127, 962);
            lblSave.Margin = new Padding(0);
            lblSave.Name = "lblSave";
            lblSave.Size = new Size(122, 48);
            lblSave.TabIndex = 46;
            lblSave.Text = "Save";
            lblSave.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tbReqNotes
            // 
            tbReqNotes.BackColor = Color.FromArgb(247, 247, 249);
            tbReqNotes.Enabled = false;
            tbReqNotes.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbReqNotes.Location = new Point(185, 292);
            tbReqNotes.Margin = new Padding(2);
            tbReqNotes.Multiline = true;
            tbReqNotes.Name = "tbReqNotes";
            tbReqNotes.Size = new Size(402, 104);
            tbReqNotes.TabIndex = 44;
            // 
            // ddlReqStatus
            // 
            ddlReqStatus.BackColor = Color.FromArgb(247, 247, 249);
            ddlReqStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlReqStatus.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            ddlReqStatus.FormattingEnabled = true;
            ddlReqStatus.Location = new Point(229, 221);
            ddlReqStatus.Margin = new Padding(2);
            ddlReqStatus.Name = "ddlReqStatus";
            ddlReqStatus.Size = new Size(361, 39);
            ddlReqStatus.TabIndex = 41;
            // 
            // tbRecPrice
            // 
            tbRecPrice.BackColor = Color.FromArgb(247, 247, 249);
            tbRecPrice.Enabled = false;
            tbRecPrice.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbRecPrice.Location = new Point(218, 159);
            tbRecPrice.Margin = new Padding(2);
            tbRecPrice.Name = "tbRecPrice";
            tbRecPrice.Size = new Size(373, 38);
            tbRecPrice.TabIndex = 40;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(17, 221);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(169, 31);
            label5.TabIndex = 34;
            label5.Text = "Request Status:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(15, 159);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(196, 31);
            label4.TabIndex = 33;
            label4.Text = "Daily Rental Price:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(17, 292);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(79, 31);
            label3.TabIndex = 32;
            label3.Text = "Notes:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label10.Location = new Point(15, 112);
            label10.Margin = new Padding(2, 0, 2, 0);
            label10.Name = "label10";
            label10.Size = new Size(120, 31);
            label10.TabIndex = 56;
            label10.Text = "Start Date:";
            // 
            // dtpReqStartDate
            // 
            dtpReqStartDate.Enabled = false;
            dtpReqStartDate.Location = new Point(187, 109);
            dtpReqStartDate.Name = "dtpReqStartDate";
            dtpReqStartDate.Size = new Size(403, 34);
            dtpReqStartDate.TabIndex = 57;
            // 
            // dtpReqEndDate
            // 
            dtpReqEndDate.Enabled = false;
            dtpReqEndDate.Location = new Point(186, 165);
            dtpReqEndDate.Name = "dtpReqEndDate";
            dtpReqEndDate.Size = new Size(403, 34);
            dtpReqEndDate.TabIndex = 59;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(16, 168);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(112, 31);
            label2.TabIndex = 58;
            label2.Text = "End Date:";
            // 
            // gbRequest
            // 
            gbRequest.Controls.Add(lblReqStatusError);
            gbRequest.Controls.Add(label12);
            gbRequest.Controls.Add(dtpReqEndDate);
            gbRequest.Controls.Add(label2);
            gbRequest.Controls.Add(tbReqId);
            gbRequest.Controls.Add(dtpReqStartDate);
            gbRequest.Controls.Add(label10);
            gbRequest.Controls.Add(tbReqNotes);
            gbRequest.Controls.Add(label3);
            gbRequest.Controls.Add(ddlReqStatus);
            gbRequest.Controls.Add(label5);
            gbRequest.Location = new Point(21, 33);
            gbRequest.Name = "gbRequest";
            gbRequest.Size = new Size(609, 421);
            gbRequest.TabIndex = 63;
            gbRequest.TabStop = false;
            gbRequest.Text = "Request Info";
            // 
            // lblReqStatusError
            // 
            lblReqStatusError.AutoSize = true;
            lblReqStatusError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblReqStatusError.ForeColor = Color.Red;
            lblReqStatusError.Location = new Point(229, 262);
            lblReqStatusError.Name = "lblReqStatusError";
            lblReqStatusError.Size = new Size(73, 25);
            lblReqStatusError.TabIndex = 70;
            lblReqStatusError.Text = "label10";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label12.Location = new Point(15, 53);
            label12.Margin = new Padding(2, 0, 2, 0);
            label12.Name = "label12";
            label12.Size = new Size(135, 31);
            label12.TabIndex = 30;
            label12.Text = "Request ID: ";
            // 
            // tbReqId
            // 
            tbReqId.BackColor = Color.FromArgb(247, 247, 249);
            tbReqId.Enabled = false;
            tbReqId.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbReqId.Location = new Point(181, 52);
            tbReqId.Margin = new Padding(2);
            tbReqId.Name = "tbReqId";
            tbReqId.Size = new Size(126, 38);
            tbReqId.TabIndex = 38;
            // 
            // gbCustomer
            // 
            gbCustomer.Controls.Add(cbCustIsActive);
            gbCustomer.Controls.Add(tbCustPhoneNumber);
            gbCustomer.Controls.Add(label16);
            gbCustomer.Controls.Add(tbCustName);
            gbCustomer.Controls.Add(tbCustEmail);
            gbCustomer.Controls.Add(label15);
            gbCustomer.Controls.Add(label13);
            gbCustomer.Controls.Add(label14);
            gbCustomer.Controls.Add(tbCustId);
            gbCustomer.Location = new Point(652, 33);
            gbCustomer.Name = "gbCustomer";
            gbCustomer.Size = new Size(599, 252);
            gbCustomer.TabIndex = 64;
            gbCustomer.TabStop = false;
            gbCustomer.Text = "Customer Info";
            // 
            // cbCustIsActive
            // 
            cbCustIsActive.AutoSize = true;
            cbCustIsActive.Enabled = false;
            cbCustIsActive.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            cbCustIsActive.Location = new Point(344, 48);
            cbCustIsActive.Margin = new Padding(2);
            cbCustIsActive.Name = "cbCustIsActive";
            cbCustIsActive.Size = new Size(103, 35);
            cbCustIsActive.TabIndex = 68;
            cbCustIsActive.Text = "Active";
            cbCustIsActive.UseVisualStyleBackColor = true;
            // 
            // tbCustPhoneNumber
            // 
            tbCustPhoneNumber.BackColor = Color.FromArgb(247, 247, 249);
            tbCustPhoneNumber.Enabled = false;
            tbCustPhoneNumber.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbCustPhoneNumber.Location = new Point(194, 196);
            tbCustPhoneNumber.Margin = new Padding(2);
            tbCustPhoneNumber.Name = "tbCustPhoneNumber";
            tbCustPhoneNumber.Size = new Size(385, 38);
            tbCustPhoneNumber.TabIndex = 67;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label16.Location = new Point(15, 199);
            label16.Margin = new Padding(2, 0, 2, 0);
            label16.Name = "label16";
            label16.Size = new Size(179, 31);
            label16.TabIndex = 66;
            label16.Text = "Phone Number: ";
            // 
            // tbCustName
            // 
            tbCustName.BackColor = Color.FromArgb(247, 247, 249);
            tbCustName.Enabled = false;
            tbCustName.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbCustName.Location = new Point(192, 96);
            tbCustName.Margin = new Padding(2);
            tbCustName.Name = "tbCustName";
            tbCustName.Size = new Size(387, 38);
            tbCustName.TabIndex = 65;
            // 
            // tbCustEmail
            // 
            tbCustEmail.BackColor = Color.FromArgb(247, 247, 249);
            tbCustEmail.Enabled = false;
            tbCustEmail.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbCustEmail.Location = new Point(192, 145);
            tbCustEmail.Margin = new Padding(2);
            tbCustEmail.Name = "tbCustEmail";
            tbCustEmail.Size = new Size(387, 38);
            tbCustEmail.TabIndex = 64;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label15.Location = new Point(13, 148);
            label15.Margin = new Padding(2, 0, 2, 0);
            label15.Name = "label15";
            label15.Size = new Size(81, 31);
            label15.TabIndex = 63;
            label15.Text = "Email: ";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label13.Location = new Point(13, 47);
            label13.Margin = new Padding(2, 0, 2, 0);
            label13.Name = "label13";
            label13.Size = new Size(151, 31);
            label13.TabIndex = 30;
            label13.Text = "Customer ID: ";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label14.Location = new Point(11, 99);
            label14.Margin = new Padding(2, 0, 2, 0);
            label14.Name = "label14";
            label14.Size = new Size(128, 31);
            label14.TabIndex = 61;
            label14.Text = "Full Name: ";
            // 
            // tbCustId
            // 
            tbCustId.BackColor = Color.FromArgb(247, 247, 249);
            tbCustId.Enabled = false;
            tbCustId.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbCustId.Location = new Point(192, 46);
            tbCustId.Margin = new Padding(2);
            tbCustId.Name = "tbCustId";
            tbCustId.Size = new Size(126, 38);
            tbCustId.TabIndex = 38;
            // 
            // gbEquipment
            // 
            gbEquipment.Controls.Add(cbEqIsActive);
            gbEquipment.Controls.Add(tbEqName);
            gbEquipment.Controls.Add(label19);
            gbEquipment.Controls.Add(label20);
            gbEquipment.Controls.Add(tbEqId);
            gbEquipment.Location = new Point(652, 291);
            gbEquipment.Name = "gbEquipment";
            gbEquipment.Size = new Size(599, 163);
            gbEquipment.TabIndex = 69;
            gbEquipment.TabStop = false;
            gbEquipment.Text = "Equipment Info";
            gbEquipment.Paint += groupBox_Paint;
            // 
            // cbEqIsActive
            // 
            cbEqIsActive.AutoSize = true;
            cbEqIsActive.Enabled = false;
            cbEqIsActive.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            cbEqIsActive.Location = new Point(346, 62);
            cbEqIsActive.Margin = new Padding(2);
            cbEqIsActive.Name = "cbEqIsActive";
            cbEqIsActive.Size = new Size(103, 35);
            cbEqIsActive.TabIndex = 68;
            cbEqIsActive.Text = "Active";
            cbEqIsActive.UseVisualStyleBackColor = true;
            // 
            // tbEqName
            // 
            tbEqName.BackColor = Color.FromArgb(247, 247, 249);
            tbEqName.Enabled = false;
            tbEqName.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbEqName.Location = new Point(194, 110);
            tbEqName.Margin = new Padding(2);
            tbEqName.Name = "tbEqName";
            tbEqName.Size = new Size(387, 38);
            tbEqName.TabIndex = 65;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label19.Location = new Point(15, 61);
            label19.Margin = new Padding(2, 0, 2, 0);
            label19.Name = "label19";
            label19.Size = new Size(165, 31);
            label19.TabIndex = 30;
            label19.Text = "Equipment ID: ";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label20.Location = new Point(13, 113);
            label20.Margin = new Padding(2, 0, 2, 0);
            label20.Name = "label20";
            label20.Size = new Size(86, 31);
            label20.TabIndex = 61;
            label20.Text = "Name: ";
            // 
            // tbEqId
            // 
            tbEqId.BackColor = Color.FromArgb(247, 247, 249);
            tbEqId.Enabled = false;
            tbEqId.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbEqId.Location = new Point(194, 60);
            tbEqId.Margin = new Padding(2);
            tbEqId.Name = "tbEqId";
            tbEqId.Size = new Size(126, 38);
            tbEqId.TabIndex = 38;
            // 
            // gbPayment
            // 
            gbPayment.Controls.Add(tbPayTotal);
            gbPayment.Controls.Add(label30);
            gbPayment.Controls.Add(ddlPayStatus);
            gbPayment.Controls.Add(label17);
            gbPayment.Controls.Add(ddlPayMethod);
            gbPayment.Controls.Add(label11);
            gbPayment.Controls.Add(label1);
            gbPayment.Controls.Add(tbPatyId);
            gbPayment.Location = new Point(652, 457);
            gbPayment.Name = "gbPayment";
            gbPayment.Size = new Size(599, 254);
            gbPayment.TabIndex = 64;
            gbPayment.TabStop = false;
            gbPayment.Text = "Payment Info";
            // 
            // tbPayTotal
            // 
            tbPayTotal.BackColor = Color.FromArgb(247, 247, 249);
            tbPayTotal.Enabled = false;
            tbPayTotal.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbPayTotal.Location = new Point(216, 99);
            tbPayTotal.Margin = new Padding(2);
            tbPayTotal.Name = "tbPayTotal";
            tbPayTotal.Size = new Size(363, 38);
            tbPayTotal.TabIndex = 65;
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label30.Location = new Point(15, 102);
            label30.Margin = new Padding(2, 0, 2, 0);
            label30.Name = "label30";
            label30.Size = new Size(68, 31);
            label30.TabIndex = 64;
            label30.Text = "Total:";
            // 
            // ddlPayStatus
            // 
            ddlPayStatus.BackColor = Color.FromArgb(247, 247, 249);
            ddlPayStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlPayStatus.Enabled = false;
            ddlPayStatus.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            ddlPayStatus.FormattingEnabled = true;
            ddlPayStatus.Location = new Point(216, 200);
            ddlPayStatus.Margin = new Padding(2);
            ddlPayStatus.Name = "ddlPayStatus";
            ddlPayStatus.Size = new Size(363, 39);
            ddlPayStatus.TabIndex = 46;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label17.Location = new Point(12, 205);
            label17.Margin = new Padding(2, 0, 2, 0);
            label17.Name = "label17";
            label17.Size = new Size(175, 31);
            label17.TabIndex = 45;
            label17.Text = "Payment Status:";
            // 
            // ddlPayMethod
            // 
            ddlPayMethod.BackColor = Color.FromArgb(247, 247, 249);
            ddlPayMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlPayMethod.Enabled = false;
            ddlPayMethod.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            ddlPayMethod.FormattingEnabled = true;
            ddlPayMethod.Location = new Point(216, 150);
            ddlPayMethod.Margin = new Padding(2);
            ddlPayMethod.Name = "ddlPayMethod";
            ddlPayMethod.Size = new Size(363, 39);
            ddlPayMethod.TabIndex = 44;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label11.Location = new Point(12, 155);
            label11.Margin = new Padding(2, 0, 2, 0);
            label11.Name = "label11";
            label11.Size = new Size(194, 31);
            label11.TabIndex = 43;
            label11.Text = "Payment Method:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(15, 53);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(141, 31);
            label1.TabIndex = 30;
            label1.Text = "Payment ID: ";
            // 
            // tbPatyId
            // 
            tbPatyId.BackColor = Color.FromArgb(247, 247, 249);
            tbPatyId.Enabled = false;
            tbPatyId.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbPatyId.Location = new Point(181, 52);
            tbPatyId.Margin = new Padding(2);
            tbPatyId.Name = "tbPatyId";
            tbPatyId.Size = new Size(126, 38);
            tbPatyId.TabIndex = 38;
            // 
            // gbFee
            // 
            gbFee.Controls.Add(lblRecExtraChargeError);
            gbFee.Controls.Add(lblRecExtraChargeDescreptionError);
            gbFee.Controls.Add(tbRecExtraCharge);
            gbFee.Controls.Add(label28);
            gbFee.Controls.Add(tbRecDeposit);
            gbFee.Controls.Add(label21);
            gbFee.Controls.Add(label18);
            gbFee.Controls.Add(tbRecId);
            gbFee.Controls.Add(dtpRecPickupDate);
            gbFee.Controls.Add(label23);
            gbFee.Controls.Add(tbRecExtraChargeDescreption);
            gbFee.Controls.Add(label24);
            gbFee.Controls.Add(tbRecPrice);
            gbFee.Controls.Add(label4);
            gbFee.Location = new Point(21, 457);
            gbFee.Name = "gbFee";
            gbFee.Size = new Size(609, 477);
            gbFee.TabIndex = 65;
            gbFee.TabStop = false;
            gbFee.Text = "Rental Fee";
            // 
            // lblRecExtraChargeError
            // 
            lblRecExtraChargeError.AutoSize = true;
            lblRecExtraChargeError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblRecExtraChargeError.ForeColor = Color.Red;
            lblRecExtraChargeError.Location = new Point(218, 304);
            lblRecExtraChargeError.Name = "lblRecExtraChargeError";
            lblRecExtraChargeError.Size = new Size(73, 25);
            lblRecExtraChargeError.TabIndex = 72;
            lblRecExtraChargeError.Text = "label10";
            // 
            // lblRecExtraChargeDescreptionError
            // 
            lblRecExtraChargeDescreptionError.AutoSize = true;
            lblRecExtraChargeDescreptionError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblRecExtraChargeDescreptionError.ForeColor = Color.Red;
            lblRecExtraChargeDescreptionError.Location = new Point(216, 443);
            lblRecExtraChargeDescreptionError.Name = "lblRecExtraChargeDescreptionError";
            lblRecExtraChargeDescreptionError.Size = new Size(73, 25);
            lblRecExtraChargeDescreptionError.TabIndex = 71;
            lblRecExtraChargeDescreptionError.Text = "label10";
            // 
            // tbRecExtraCharge
            // 
            tbRecExtraCharge.BackColor = Color.FromArgb(247, 247, 249);
            tbRecExtraCharge.Enabled = false;
            tbRecExtraCharge.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbRecExtraCharge.Location = new Point(218, 264);
            tbRecExtraCharge.Margin = new Padding(2);
            tbRecExtraCharge.Name = "tbRecExtraCharge";
            tbRecExtraCharge.Size = new Size(373, 38);
            tbRecExtraCharge.TabIndex = 63;
            tbRecExtraCharge.TextChanged += tbRecExtraCharge_TextChanged;
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label28.Location = new Point(14, 267);
            label28.Margin = new Padding(2, 0, 2, 0);
            label28.Name = "label28";
            label28.Size = new Size(159, 31);
            label28.TabIndex = 62;
            label28.Text = "Extra Charges:";
            // 
            // tbRecDeposit
            // 
            tbRecDeposit.BackColor = Color.FromArgb(247, 247, 249);
            tbRecDeposit.Enabled = false;
            tbRecDeposit.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbRecDeposit.Location = new Point(218, 209);
            tbRecDeposit.Margin = new Padding(2);
            tbRecDeposit.Name = "tbRecDeposit";
            tbRecDeposit.Size = new Size(373, 38);
            tbRecDeposit.TabIndex = 48;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label21.Location = new Point(15, 53);
            label21.Margin = new Padding(2, 0, 2, 0);
            label21.Name = "label21";
            label21.Size = new Size(124, 31);
            label21.TabIndex = 30;
            label21.Text = "Record ID: ";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label18.Location = new Point(14, 212);
            label18.Margin = new Padding(2, 0, 2, 0);
            label18.Name = "label18";
            label18.Size = new Size(98, 31);
            label18.TabIndex = 47;
            label18.Text = "Deposit:";
            // 
            // tbRecId
            // 
            tbRecId.BackColor = Color.FromArgb(247, 247, 249);
            tbRecId.Enabled = false;
            tbRecId.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbRecId.Location = new Point(181, 52);
            tbRecId.Margin = new Padding(2);
            tbRecId.Name = "tbRecId";
            tbRecId.Size = new Size(126, 38);
            tbRecId.TabIndex = 38;
            // 
            // dtpRecPickupDate
            // 
            dtpRecPickupDate.Enabled = false;
            dtpRecPickupDate.Location = new Point(232, 109);
            dtpRecPickupDate.Name = "dtpRecPickupDate";
            dtpRecPickupDate.Size = new Size(356, 34);
            dtpRecPickupDate.TabIndex = 57;
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label23.Location = new Point(15, 112);
            label23.Margin = new Padding(2, 0, 2, 0);
            label23.Name = "label23";
            label23.Size = new Size(212, 31);
            label23.TabIndex = 56;
            label23.Text = "Actual Pickup Date:";
            // 
            // tbRecExtraChargeDescreption
            // 
            tbRecExtraChargeDescreption.BackColor = Color.FromArgb(247, 247, 249);
            tbRecExtraChargeDescreption.Enabled = false;
            tbRecExtraChargeDescreption.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbRecExtraChargeDescreption.Location = new Point(216, 337);
            tbRecExtraChargeDescreption.Margin = new Padding(2);
            tbRecExtraChargeDescreption.Multiline = true;
            tbRecExtraChargeDescreption.Name = "tbRecExtraChargeDescreption";
            tbRecExtraChargeDescreption.Size = new Size(375, 104);
            tbRecExtraChargeDescreption.TabIndex = 44;
            tbRecExtraChargeDescreption.TextChanged += tbRecExtraChargeDescreption_TextChanged;
            // 
            // label24
            // 
            label24.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label24.Location = new Point(13, 337);
            label24.Margin = new Padding(2, 0, 2, 0);
            label24.Name = "label24";
            label24.Size = new Size(155, 73);
            label24.TabIndex = 32;
            label24.Text = "Extra Charge Descreption:";
            // 
            // gbReturn
            // 
            gbReturn.Controls.Add(tbRetLateFee);
            gbReturn.Controls.Add(label8);
            gbReturn.Controls.Add(dtpRetDate);
            gbReturn.Controls.Add(label26);
            gbReturn.Controls.Add(ddlRetCondetion);
            gbReturn.Controls.Add(label33);
            gbReturn.Location = new Point(652, 729);
            gbReturn.Name = "gbReturn";
            gbReturn.Size = new Size(599, 205);
            gbReturn.TabIndex = 66;
            gbReturn.TabStop = false;
            gbReturn.Text = "Return Info";
            // 
            // tbRetLateFee
            // 
            tbRetLateFee.BackColor = Color.FromArgb(247, 247, 249);
            tbRetLateFee.Enabled = false;
            tbRetLateFee.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbRetLateFee.Location = new Point(216, 97);
            tbRetLateFee.Margin = new Padding(2);
            tbRetLateFee.Name = "tbRetLateFee";
            tbRetLateFee.Size = new Size(365, 38);
            tbRetLateFee.TabIndex = 61;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label8.Location = new Point(13, 97);
            label8.Margin = new Padding(2, 0, 2, 0);
            label8.Name = "label8";
            label8.Size = new Size(176, 31);
            label8.TabIndex = 60;
            label8.Text = "Late Return Fee:";
            // 
            // dtpRetDate
            // 
            dtpRetDate.Enabled = false;
            dtpRetDate.Location = new Point(229, 45);
            dtpRetDate.Name = "dtpRetDate";
            dtpRetDate.Size = new Size(349, 34);
            dtpRetDate.TabIndex = 59;
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label26.Location = new Point(13, 48);
            label26.Margin = new Padding(2, 0, 2, 0);
            label26.Name = "label26";
            label26.Size = new Size(211, 31);
            label26.TabIndex = 58;
            label26.Text = "Actual Return Date:";
            // 
            // ddlRetCondetion
            // 
            ddlRetCondetion.BackColor = Color.FromArgb(247, 247, 249);
            ddlRetCondetion.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlRetCondetion.Enabled = false;
            ddlRetCondetion.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            ddlRetCondetion.FormattingEnabled = true;
            ddlRetCondetion.Location = new Point(225, 146);
            ddlRetCondetion.Margin = new Padding(2);
            ddlRetCondetion.Name = "ddlRetCondetion";
            ddlRetCondetion.Size = new Size(358, 39);
            ddlRetCondetion.TabIndex = 42;
            // 
            // label33
            // 
            label33.AutoSize = true;
            label33.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label33.Location = new Point(13, 151);
            label33.Margin = new Padding(2, 0, 2, 0);
            label33.Name = "label33";
            label33.Size = new Size(184, 31);
            label33.TabIndex = 35;
            label33.Text = "Reurn Condition:";
            // 
            // lblStartTransaction
            // 
            lblStartTransaction.BackColor = Color.FromArgb(60, 173, 104);
            lblStartTransaction.Font = new Font("Cascadia Mono", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblStartTransaction.ForeColor = Color.White;
            lblStartTransaction.Location = new Point(457, 963);
            lblStartTransaction.Margin = new Padding(0);
            lblStartTransaction.Name = "lblStartTransaction";
            lblStartTransaction.Size = new Size(283, 48);
            lblStartTransaction.TabIndex = 70;
            lblStartTransaction.Text = "Start Transaction";
            lblStartTransaction.TextAlign = ContentAlignment.MiddleCenter;
            lblStartTransaction.Click += lblStartTransaction_Click;
            // 
            // ManageRental
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1270, 1026);
            Controls.Add(lblStartTransaction);
            Controls.Add(gbReturn);
            Controls.Add(gbFee);
            Controls.Add(gbPayment);
            Controls.Add(gbEquipment);
            Controls.Add(gbCustomer);
            Controls.Add(gbRequest);
            Controls.Add(lblClose);
            Controls.Add(lblDelete);
            Controls.Add(lblSave);
            Name = "ManageRental";
            Text = "ManageRental";
            gbRequest.ResumeLayout(false);
            gbRequest.PerformLayout();
            gbCustomer.ResumeLayout(false);
            gbCustomer.PerformLayout();
            gbEquipment.ResumeLayout(false);
            gbEquipment.PerformLayout();
            gbPayment.ResumeLayout(false);
            gbPayment.PerformLayout();
            gbFee.ResumeLayout(false);
            gbFee.PerformLayout();
            gbReturn.ResumeLayout(false);
            gbReturn.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Label lblClose;
        private Label lblDelete;
        private Label lblSave;
        private TextBox tbReqNotes;
        private ComboBox ddlReqStatus;
        private TextBox tbRecPrice;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label10;
        private DateTimePicker dtpReqStartDate;
        private DateTimePicker dtpReqEndDate;
        private Label label2;
        private GroupBox gbRequest;
        private Label label12;
        private TextBox tbReqId;
        private GroupBox gbCustomer;
        private TextBox tbCustPhoneNumber;
        private Label label16;
        private TextBox tbCustName;
        private TextBox tbCustEmail;
        private Label label15;
        private Label label13;
        private Label label14;
        private TextBox tbCustId;
        private CheckBox cbCustIsActive;
        private GroupBox gbEquipment;
        private CheckBox cbEqIsActive;
        private TextBox tbEqName;
        private Label label19;
        private Label label20;
        private TextBox tbEqId;
        private GroupBox gbPayment;
        private Label label1;
        private TextBox tbPatyId;
        private GroupBox gbFee;
        private Label label21;
        private TextBox tbRecId;
        private DateTimePicker dtpRecPickupDate;
        private Label label23;
        private TextBox tbRecExtraChargeDescreption;
        private Label label24;
        private ComboBox ddlPayMethod;
        private Label label11;
        private TextBox tbRecDeposit;
        private Label label18;
        private ComboBox ddlPayStatus;
        private Label label17;
        private TextBox tbRecExtraCharge;
        private Label label28;
        private TextBox tbPayTotal;
        private Label label30;
        private GroupBox gbReturn;
        private TextBox tbRetLateFee;
        private Label label8;
        private DateTimePicker dtpRetDate;
        private Label label26;
        private ComboBox ddlRetCondetion;
        private Label label33;
        private Label lblReqStatusError;
        private Label lblRecExtraChargeError;
        private Label lblRecExtraChargeDescreptionError;
        private Label lblStartTransaction;
    }
}