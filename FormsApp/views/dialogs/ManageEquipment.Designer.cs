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
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            cbIsActive = new CheckBox();
            lblId = new TextBox();
            lblName = new TextBox();
            lblPrice = new TextBox();
            ddlAvalability = new ComboBox();
            ddlCondition = new ComboBox();
            ddlCategory = new ComboBox();
            tbDescription = new TextBox();
            lblSave = new Label();
            lblClose = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(52, 64);
            label1.Name = "label1";
            label1.Size = new Size(254, 48);
            label1.TabIndex = 0;
            label1.Text = "Equipment ID: ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(56, 143);
            label2.Name = "label2";
            label2.Size = new Size(123, 48);
            label2.TabIndex = 1;
            label2.Text = "Name:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(56, 589);
            label3.Name = "label3";
            label3.Size = new Size(209, 48);
            label3.TabIndex = 2;
            label3.Text = "Description:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(56, 235);
            label4.Name = "label4";
            label4.Size = new Size(215, 48);
            label4.TabIndex = 3;
            label4.Text = "Rental Price:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(52, 326);
            label5.Name = "label5";
            label5.Size = new Size(303, 48);
            label5.TabIndex = 4;
            label5.Text = "Availability Status:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label6.Location = new Point(52, 413);
            label6.Name = "label6";
            label6.Size = new Size(183, 48);
            label6.TabIndex = 5;
            label6.Text = "Condition:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label7.Location = new Point(52, 496);
            label7.Name = "label7";
            label7.Size = new Size(163, 48);
            label7.TabIndex = 6;
            label7.Text = "Category";
            // 
            // cbIsActive
            // 
            cbIsActive.AutoSize = true;
            cbIsActive.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            cbIsActive.Location = new Point(590, 764);
            cbIsActive.Name = "cbIsActive";
            cbIsActive.Size = new Size(155, 52);
            cbIsActive.TabIndex = 7;
            cbIsActive.Text = "Active";
            cbIsActive.UseVisualStyleBackColor = true;
            // 
            // lblId
            // 
            lblId.Enabled = false;
            lblId.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblId.Location = new Point(332, 62);
            lblId.Name = "lblId";
            lblId.Size = new Size(413, 55);
            lblId.TabIndex = 8;
            // 
            // lblName
            // 
            lblName.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblName.Location = new Point(332, 143);
            lblName.Name = "lblName";
            lblName.Size = new Size(413, 55);
            lblName.TabIndex = 9;
            // 
            // lblPrice
            // 
            lblPrice.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblPrice.Location = new Point(332, 235);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(413, 55);
            lblPrice.TabIndex = 11;
            // 
            // ddlAvalability
            // 
            ddlAvalability.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlAvalability.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            ddlAvalability.FormattingEnabled = true;
            ddlAvalability.Location = new Point(377, 326);
            ddlAvalability.Name = "ddlAvalability";
            ddlAvalability.Size = new Size(368, 56);
            ddlAvalability.TabIndex = 12;
            // 
            // ddlCondition
            // 
            ddlCondition.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlCondition.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            ddlCondition.FormattingEnabled = true;
            ddlCondition.Location = new Point(377, 405);
            ddlCondition.Name = "ddlCondition";
            ddlCondition.Size = new Size(368, 56);
            ddlCondition.TabIndex = 13;
            // 
            // ddlCategory
            // 
            ddlCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            ddlCategory.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            ddlCategory.FormattingEnabled = true;
            ddlCategory.Location = new Point(377, 488);
            ddlCategory.Name = "ddlCategory";
            ddlCategory.Size = new Size(368, 56);
            ddlCategory.TabIndex = 14;
            // 
            // tbDescription
            // 
            tbDescription.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbDescription.Location = new Point(332, 565);
            tbDescription.Multiline = true;
            tbDescription.Name = "tbDescription";
            tbDescription.Size = new Size(413, 165);
            tbDescription.TabIndex = 15;
            // 
            // lblSave
            // 
            lblSave.BackColor = Color.FromArgb(29, 31, 38);
            lblSave.Font = new Font("Cascadia Mono", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblSave.ForeColor = Color.FromArgb(191, 199, 217);
            lblSave.Location = new Point(203, 849);
            lblSave.Margin = new Padding(5, 0, 5, 0);
            lblSave.Name = "lblSave";
            lblSave.Padding = new Padding(15);
            lblSave.Size = new Size(200, 77);
            lblSave.TabIndex = 16;
            lblSave.Text = "Save";
            lblSave.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblClose
            // 
            lblClose.BackColor = Color.FromArgb(29, 31, 38);
            lblClose.Font = new Font("Cascadia Mono", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblClose.ForeColor = Color.FromArgb(191, 199, 217);
            lblClose.Location = new Point(413, 849);
            lblClose.Margin = new Padding(5, 0, 5, 0);
            lblClose.Name = "lblClose";
            lblClose.Padding = new Padding(15);
            lblClose.Size = new Size(200, 77);
            lblClose.TabIndex = 17;
            lblClose.Text = "Close";
            lblClose.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ManageEquipment
            // 
            AutoScaleDimensions = new SizeF(18F, 45F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 959);
            Controls.Add(lblClose);
            Controls.Add(lblSave);
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
            Name = "ManageEquipment";
            Text = "Manage Equipment";
            Load += ManageEquipment_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private CheckBox cbIsActive;
        private TextBox lblId;
        private TextBox lblName;
        private TextBox lblPrice;
        private ComboBox ddlAvalability;
        private ComboBox ddlCondition;
        private ComboBox ddlCategory;
        private TextBox tbDescription;
        private Label lblSave;
        private Label lblClose;
    }
}