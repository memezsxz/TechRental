namespace FormsApp.views.dialogs
{
    partial class ManageEquipment
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            cbIsActive = new CheckBox();
            lblId = new TextBox();
            lblName = new TextBox();
            tbDescription = new TextBox();
            lblSave = new Label();
            lblDelete = new Label();
            lblClose = new Label();
            lblNameError = new Label();
            lblDescreptionError = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            lblImage = new Label();
            pnlImage = new Panel();
            lblImageError = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            lblPrice = new TextBox();
            ddlAvalability = new ComboBox();
            ddlCondition = new ComboBox();
            ddlCategory = new ComboBox();
            lblPriceError = new Label();
            tableLayoutPanel1.SuspendLayout();
            pnlImage.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(32, 40);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(165, 31);
            label1.TabIndex = 0;
            label1.Text = "Equipment ID: ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(34, 95);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(80, 31);
            label2.TabIndex = 1;
            label2.Text = "Name:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(34, 434);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(136, 31);
            label3.TabIndex = 2;
            label3.Text = "Description:";
            // 
            // cbIsActive
            // 
            cbIsActive.AutoSize = true;
            cbIsActive.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            cbIsActive.Location = new Point(619, 41);
            cbIsActive.Margin = new Padding(2);
            cbIsActive.Name = "cbIsActive";
            cbIsActive.Size = new Size(103, 35);
            cbIsActive.TabIndex = 7;
            cbIsActive.Text = "Active";
            cbIsActive.UseVisualStyleBackColor = true;
            // 
            // lblId
            // 
            lblId.BackColor = Color.FromArgb(247, 247, 249);
            lblId.Enabled = false;
            lblId.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblId.Location = new Point(203, 39);
            lblId.Margin = new Padding(2);
            lblId.Name = "lblId";
            lblId.Size = new Size(394, 38);
            lblId.TabIndex = 8;
            // 
            // lblName
            // 
            lblName.BackColor = Color.FromArgb(247, 247, 249);
            lblName.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblName.Location = new Point(203, 95);
            lblName.Margin = new Padding(2);
            lblName.Name = "lblName";
            lblName.Size = new Size(394, 38);
            lblName.TabIndex = 9;
            // 
            // tbDescription
            // 
            tbDescription.BackColor = Color.FromArgb(247, 247, 249);
            tbDescription.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbDescription.Location = new Point(202, 434);
            tbDescription.Margin = new Padding(2);
            tbDescription.Multiline = true;
            tbDescription.Name = "tbDescription";
            tbDescription.Size = new Size(394, 104);
            tbDescription.TabIndex = 15;
            // 
            // lblSave
            // 
            lblSave.BackColor = Color.FromArgb(60, 173, 104);
            lblSave.Font = new Font("Cascadia Mono", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblSave.ForeColor = Color.White;
            lblSave.Location = new Point(752, 604);
            lblSave.Margin = new Padding(0);
            lblSave.Name = "lblSave";
            lblSave.Size = new Size(122, 48);
            lblSave.TabIndex = 22;
            lblSave.Text = "Save";
            lblSave.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDelete
            // 
            lblDelete.BackColor = Color.Red;
            lblDelete.Font = new Font("Cascadia Mono", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblDelete.ForeColor = Color.White;
            lblDelete.Location = new Point(32, 606);
            lblDelete.Margin = new Padding(0);
            lblDelete.Name = "lblDelete";
            lblDelete.Size = new Size(122, 48);
            lblDelete.TabIndex = 23;
            lblDelete.Text = "Delete";
            lblDelete.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblClose
            // 
            lblClose.BackColor = Color.White;
            lblClose.Font = new Font("Cascadia Mono", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblClose.ForeColor = Color.FromArgb(60, 173, 104);
            lblClose.Location = new Point(619, 604);
            lblClose.Margin = new Padding(0);
            lblClose.Name = "lblClose";
            lblClose.Size = new Size(122, 48);
            lblClose.TabIndex = 24;
            lblClose.Text = "Close";
            lblClose.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblNameError
            // 
            lblNameError.AutoSize = true;
            lblNameError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblNameError.ForeColor = Color.Red;
            lblNameError.Location = new Point(203, 138);
            lblNameError.Name = "lblNameError";
            lblNameError.Size = new Size(73, 25);
            lblNameError.TabIndex = 25;
            lblNameError.Text = "label10";
            // 
            // lblDescreptionError
            // 
            lblDescreptionError.AutoSize = true;
            lblDescreptionError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblDescreptionError.ForeColor = Color.Red;
            lblDescreptionError.Location = new Point(203, 544);
            lblDescreptionError.Name = "lblDescreptionError";
            lblDescreptionError.Size = new Size(73, 25);
            lblDescreptionError.TabIndex = 28;
            lblDescreptionError.Text = "label12";
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
            lblImage.Click += lblImage_Click;
            // 
            // pnlImage
            // 
            pnlImage.BackColor = Color.FromArgb(247, 247, 249);
            pnlImage.BackgroundImageLayout = ImageLayout.Zoom;
            pnlImage.Controls.Add(tableLayoutPanel1);
            pnlImage.Location = new Point(619, 95);
            pnlImage.Name = "pnlImage";
            pnlImage.Padding = new Padding(20);
            pnlImage.Size = new Size(255, 218);
            pnlImage.TabIndex = 21;
            // 
            // lblImageError
            // 
            lblImageError.AutoSize = true;
            lblImageError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblImageError.ForeColor = Color.Red;
            lblImageError.Location = new Point(620, 319);
            lblImageError.Name = "lblImageError";
            lblImageError.Size = new Size(73, 25);
            lblImageError.TabIndex = 29;
            lblImageError.Text = "label10";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(34, 176);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(139, 31);
            label4.TabIndex = 3;
            label4.Text = "Rental Price:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(32, 259);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(200, 31);
            label5.TabIndex = 4;
            label5.Text = "Availability Status:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label6.Location = new Point(32, 321);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(119, 31);
            label6.TabIndex = 5;
            label6.Text = "Condition:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label7.Location = new Point(32, 379);
            label7.Margin = new Padding(2, 0, 2, 0);
            label7.Name = "label7";
            label7.Size = new Size(111, 31);
            label7.TabIndex = 6;
            label7.Text = "Category:";
            // 
            // lblPrice
            // 
            lblPrice.BackColor = Color.FromArgb(247, 247, 249);
            lblPrice.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblPrice.Location = new Point(203, 176);
            lblPrice.Margin = new Padding(2);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(394, 38);
            lblPrice.TabIndex = 11;
            // 
            // ddlAvalability
            // 
            ddlAvalability.BackColor = Color.FromArgb(247, 247, 249);
            ddlAvalability.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlAvalability.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            ddlAvalability.FormattingEnabled = true;
            ddlAvalability.Location = new Point(244, 259);
            ddlAvalability.Margin = new Padding(2);
            ddlAvalability.Name = "ddlAvalability";
            ddlAvalability.Size = new Size(353, 39);
            ddlAvalability.TabIndex = 12;
            // 
            // ddlCondition
            // 
            ddlCondition.BackColor = Color.FromArgb(247, 247, 249);
            ddlCondition.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlCondition.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            ddlCondition.FormattingEnabled = true;
            ddlCondition.Location = new Point(244, 316);
            ddlCondition.Margin = new Padding(2);
            ddlCondition.Name = "ddlCondition";
            ddlCondition.Size = new Size(353, 39);
            ddlCondition.TabIndex = 13;
            // 
            // ddlCategory
            // 
            ddlCategory.BackColor = Color.FromArgb(247, 247, 249);
            ddlCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlCategory.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            ddlCategory.FormattingEnabled = true;
            ddlCategory.Location = new Point(244, 374);
            ddlCategory.Margin = new Padding(2);
            ddlCategory.Name = "ddlCategory";
            ddlCategory.Size = new Size(353, 39);
            ddlCategory.TabIndex = 14;
            // 
            // lblPriceError
            // 
            lblPriceError.AutoSize = true;
            lblPriceError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblPriceError.ForeColor = Color.Red;
            lblPriceError.Location = new Point(203, 216);
            lblPriceError.Name = "lblPriceError";
            lblPriceError.Size = new Size(73, 25);
            lblPriceError.TabIndex = 26;
            lblPriceError.Text = "label11";
            // 
            // ManageEquipment
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(903, 677);
            Controls.Add(lblImageError);
            Controls.Add(lblDescreptionError);
            Controls.Add(lblPriceError);
            Controls.Add(lblNameError);
            Controls.Add(lblClose);
            Controls.Add(lblDelete);
            Controls.Add(lblSave);
            Controls.Add(pnlImage);
            Controls.Add(tbDescription);
            Controls.Add(ddlCategory);
            Controls.Add(ddlCondition);
            Controls.Add(ddlAvalability);
            Controls.Add(lblPrice);
            Controls.Add(lblName);
            Controls.Add(lblId);
            Controls.Add(cbIsActive);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Margin = new Padding(2);
            Name = "ManageEquipment";
            Text = "Manage Equipment";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            pnlImage.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private CheckBox cbIsActive;
        private TextBox lblId;
        private TextBox lblName;
        private TextBox tbDescription;
        private Panel pnlClose;
        private Label lblDescreptionError;
        private Label label9;
        private Label lblSave;
        private Label lblDelete;
        private Label lblClose;
        private Label lblNameError;
        private TableLayoutPanel tableLayoutPanel1;
        private Label lblImage;
        private Panel pnlImage;
        private Label lblImageError;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private TextBox lblPrice;
        private ComboBox ddlAvalability;
        private ComboBox ddlCondition;
        private ComboBox ddlCategory;
        private Label lblPriceError;
    }
}