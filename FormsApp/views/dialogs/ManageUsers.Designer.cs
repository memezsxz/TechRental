namespace FormsApp.views.dialogs
{
    partial class ManageUser
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
            lblImageError = new Label();
            lblLastNameError = new Label();
            lblFirstNameError = new Label();
            lblClose = new Label();
            lblDelete = new Label();
            lblSave = new Label();
            pnlImage = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblImage = new Label();
            ddlRole = new ComboBox();
            tbLastName = new TextBox();
            tbFirstName = new TextBox();
            tbId = new TextBox();
            cbIsActive = new CheckBox();
            label6 = new Label();
            label4 = new Label();
            label2 = new Label();
            label1 = new Label();
            lblEmailError = new Label();
            tbEmail = new TextBox();
            label9 = new Label();
            lblPhoneNumberError = new Label();
            tbPhoneNumber = new TextBox();
            label11 = new Label();
            pnlImage.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // lblImageError
            // 
            lblImageError.AutoSize = true;
            lblImageError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblImageError.ForeColor = Color.Red;
            lblImageError.Location = new Point(606, 300);
            lblImageError.Name = "lblImageError";
            lblImageError.Size = new Size(73, 25);
            lblImageError.TabIndex = 52;
            lblImageError.Text = "label10";
            // 
            // lblLastNameError
            // 
            lblLastNameError.AutoSize = true;
            lblLastNameError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblLastNameError.ForeColor = Color.Red;
            lblLastNameError.Location = new Point(189, 197);
            lblLastNameError.Name = "lblLastNameError";
            lblLastNameError.Size = new Size(73, 25);
            lblLastNameError.TabIndex = 50;
            lblLastNameError.Text = "label11";
            // 
            // lblFirstNameError
            // 
            lblFirstNameError.AutoSize = true;
            lblFirstNameError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblFirstNameError.ForeColor = Color.Red;
            lblFirstNameError.Location = new Point(189, 119);
            lblFirstNameError.Name = "lblFirstNameError";
            lblFirstNameError.Size = new Size(73, 25);
            lblFirstNameError.TabIndex = 49;
            lblFirstNameError.Text = "label10";
            // 
            // lblClose
            // 
            lblClose.BackColor = Color.White;
            lblClose.Font = new Font("Cascadia Mono", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblClose.ForeColor = Color.FromArgb(60, 173, 104);
            lblClose.Location = new Point(606, 457);
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
            lblDelete.Location = new Point(19, 459);
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
            lblSave.Location = new Point(739, 457);
            lblSave.Margin = new Padding(0);
            lblSave.Name = "lblSave";
            lblSave.Size = new Size(122, 48);
            lblSave.TabIndex = 46;
            lblSave.Text = "Save";
            lblSave.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlImage
            // 
            pnlImage.BackColor = Color.FromArgb(247, 247, 249);
            pnlImage.BackgroundImageLayout = ImageLayout.Zoom;
            pnlImage.Controls.Add(tableLayoutPanel1);
            pnlImage.Location = new Point(605, 76);
            pnlImage.Name = "pnlImage";
            pnlImage.Padding = new Padding(20);
            pnlImage.Size = new Size(255, 218);
            pnlImage.TabIndex = 45;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.Transparent;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(lblImage, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(20, 20);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(40);
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(215, 178);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // lblImage
            // 
            lblImage.AutoSize = true;
            lblImage.BackColor = Color.Transparent;
            lblImage.Dock = DockStyle.Fill;
            lblImage.Font = new Font("Segoe UI", 8.834356F, FontStyle.Bold, GraphicsUnit.Point);
            lblImage.ForeColor = Color.FromArgb(60, 173, 104);
            lblImage.Location = new Point(40, 40);
            lblImage.Margin = new Padding(0);
            lblImage.Name = "lblImage";
            lblImage.Size = new Size(135, 98);
            lblImage.TabIndex = 0;
            lblImage.Text = "Edit";
            lblImage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ddlRole
            // 
            ddlRole.BackColor = Color.FromArgb(247, 247, 249);
            ddlRole.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlRole.Enabled = false;
            ddlRole.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            ddlRole.FormattingEnabled = true;
            ddlRole.Location = new Point(189, 388);
            ddlRole.Margin = new Padding(2);
            ddlRole.Name = "ddlRole";
            ddlRole.Size = new Size(393, 39);
            ddlRole.TabIndex = 42;
            // 
            // tbLastName
            // 
            tbLastName.BackColor = Color.FromArgb(247, 247, 249);
            tbLastName.Enabled = false;
            tbLastName.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbLastName.Location = new Point(189, 157);
            tbLastName.Margin = new Padding(2);
            tbLastName.Name = "tbLastName";
            tbLastName.Size = new Size(394, 38);
            tbLastName.TabIndex = 40;
            // 
            // tbFirstName
            // 
            tbFirstName.BackColor = Color.FromArgb(247, 247, 249);
            tbFirstName.Enabled = false;
            tbFirstName.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbFirstName.Location = new Point(189, 76);
            tbFirstName.Margin = new Padding(2);
            tbFirstName.Name = "tbFirstName";
            tbFirstName.Size = new Size(394, 38);
            tbFirstName.TabIndex = 39;
            // 
            // tbId
            // 
            tbId.BackColor = Color.FromArgb(247, 247, 249);
            tbId.Enabled = false;
            tbId.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbId.Location = new Point(189, 20);
            tbId.Margin = new Padding(2);
            tbId.Name = "tbId";
            tbId.Size = new Size(394, 38);
            tbId.TabIndex = 38;
            // 
            // cbIsActive
            // 
            cbIsActive.AutoSize = true;
            cbIsActive.Enabled = false;
            cbIsActive.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            cbIsActive.Location = new Point(605, 22);
            cbIsActive.Margin = new Padding(2);
            cbIsActive.Name = "cbIsActive";
            cbIsActive.Size = new Size(103, 35);
            cbIsActive.TabIndex = 37;
            cbIsActive.Text = "Active";
            cbIsActive.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label6.Location = new Point(20, 391);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(63, 31);
            label6.TabIndex = 35;
            label6.Text = "Role:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(20, 157);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(127, 31);
            label4.TabIndex = 33;
            label4.Text = "Last Name:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(20, 76);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(129, 31);
            label2.TabIndex = 31;
            label2.Text = "First Name:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(18, 21);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(99, 31);
            label1.TabIndex = 30;
            label1.Text = "User ID: ";
            // 
            // lblEmailError
            // 
            lblEmailError.AutoSize = true;
            lblEmailError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblEmailError.ForeColor = Color.Red;
            lblEmailError.Location = new Point(188, 276);
            lblEmailError.Name = "lblEmailError";
            lblEmailError.Size = new Size(73, 25);
            lblEmailError.TabIndex = 55;
            lblEmailError.Text = "label11";
            // 
            // tbEmail
            // 
            tbEmail.BackColor = Color.FromArgb(247, 247, 249);
            tbEmail.Enabled = false;
            tbEmail.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbEmail.Location = new Point(188, 236);
            tbEmail.Margin = new Padding(2);
            tbEmail.Name = "tbEmail";
            tbEmail.Size = new Size(394, 38);
            tbEmail.TabIndex = 54;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label9.Location = new Point(19, 236);
            label9.Margin = new Padding(2, 0, 2, 0);
            label9.Name = "label9";
            label9.Size = new Size(75, 31);
            label9.TabIndex = 53;
            label9.Text = "Email:";
            // 
            // lblPhoneNumberError
            // 
            lblPhoneNumberError.AutoSize = true;
            lblPhoneNumberError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblPhoneNumberError.ForeColor = Color.Red;
            lblPhoneNumberError.Location = new Point(189, 353);
            lblPhoneNumberError.Name = "lblPhoneNumberError";
            lblPhoneNumberError.Size = new Size(73, 25);
            lblPhoneNumberError.TabIndex = 58;
            lblPhoneNumberError.Text = "label11";
            // 
            // tbPhoneNumber
            // 
            tbPhoneNumber.BackColor = Color.FromArgb(247, 247, 249);
            tbPhoneNumber.Enabled = false;
            tbPhoneNumber.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbPhoneNumber.Location = new Point(189, 313);
            tbPhoneNumber.Margin = new Padding(2);
            tbPhoneNumber.Name = "tbPhoneNumber";
            tbPhoneNumber.Size = new Size(394, 38);
            tbPhoneNumber.TabIndex = 57;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label11.Location = new Point(20, 313);
            label11.Margin = new Padding(2, 0, 2, 0);
            label11.Name = "label11";
            label11.Size = new Size(124, 31);
            label11.TabIndex = 56;
            label11.Text = "Phone No.:";
            // 
            // ManageUser
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(883, 521);
            Controls.Add(lblPhoneNumberError);
            Controls.Add(tbPhoneNumber);
            Controls.Add(label11);
            Controls.Add(lblEmailError);
            Controls.Add(tbEmail);
            Controls.Add(label9);
            Controls.Add(lblImageError);
            Controls.Add(lblLastNameError);
            Controls.Add(lblFirstNameError);
            Controls.Add(lblClose);
            Controls.Add(lblDelete);
            Controls.Add(lblSave);
            Controls.Add(pnlImage);
            Controls.Add(ddlRole);
            Controls.Add(tbLastName);
            Controls.Add(tbFirstName);
            Controls.Add(tbId);
            Controls.Add(cbIsActive);
            Controls.Add(label6);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "ManageUser";
            Text = "ManageCategory";
            pnlImage.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblImageError;
        private Label lblLastNameError;
        private Label lblFirstNameError;
        private Label lblClose;
        private Label lblDelete;
        private Label lblSave;
        private Panel pnlImage;
        private TableLayoutPanel tableLayoutPanel1;
        private Label lblImage;
        private ComboBox ddlRole;
        private TextBox tbLastName;
        private TextBox tbFirstName;
        private TextBox tbId;
        private CheckBox cbIsActive;
        private Label label6;
        private Label label4;
        private Label label2;
        private Label label1;
        private Label lblEmailError;
        private TextBox tbEmail;
        private Label label9;
        private Label lblPhoneNumberError;
        private TextBox tbPhoneNumber;
        private Label label11;
    }
}