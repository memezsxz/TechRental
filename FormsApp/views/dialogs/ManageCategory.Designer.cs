namespace FormsApp.views.dialogs
{
    partial class ManageCategory
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
            lblDescreptionError = new Label();
            lblNameError = new Label();
            tbDescription = new TextBox();
            lblName = new TextBox();
            lblId = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            lblClose = new Label();
            lblDelete = new Label();
            lblSave = new Label();
            cbIsActive = new CheckBox();
            SuspendLayout();
            // 
            // lblDescreptionError
            // 
            lblDescreptionError.AutoSize = true;
            lblDescreptionError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblDescreptionError.ForeColor = Color.Red;
            lblDescreptionError.Location = new Point(187, 265);
            lblDescreptionError.Name = "lblDescreptionError";
            lblDescreptionError.Size = new Size(73, 25);
            lblDescreptionError.TabIndex = 42;
            lblDescreptionError.Text = "label11";
            // 
            // lblNameError
            // 
            lblNameError.AutoSize = true;
            lblNameError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblNameError.ForeColor = Color.Red;
            lblNameError.Location = new Point(188, 123);
            lblNameError.Name = "lblNameError";
            lblNameError.Size = new Size(73, 25);
            lblNameError.TabIndex = 41;
            lblNameError.Text = "label10";
            // 
            // tbDescription
            // 
            tbDescription.BackColor = Color.FromArgb(247, 247, 249);
            tbDescription.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbDescription.Location = new Point(187, 156);
            tbDescription.Margin = new Padding(2);
            tbDescription.Multiline = true;
            tbDescription.Name = "tbDescription";
            tbDescription.Size = new Size(394, 104);
            tbDescription.TabIndex = 40;
            // 
            // lblName
            // 
            lblName.BackColor = Color.FromArgb(247, 247, 249);
            lblName.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblName.Location = new Point(188, 80);
            lblName.Margin = new Padding(2);
            lblName.Name = "lblName";
            lblName.Size = new Size(394, 38);
            lblName.TabIndex = 35;
            // 
            // lblId
            // 
            lblId.BackColor = Color.FromArgb(247, 247, 249);
            lblId.Enabled = false;
            lblId.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblId.Location = new Point(188, 24);
            lblId.Margin = new Padding(2);
            lblId.Name = "lblId";
            lblId.Size = new Size(117, 38);
            lblId.TabIndex = 34;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(19, 156);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(136, 31);
            label3.TabIndex = 29;
            label3.Text = "Description:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(19, 80);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(80, 31);
            label2.TabIndex = 28;
            label2.Text = "Name:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(17, 25);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(145, 31);
            label1.TabIndex = 27;
            label1.Text = "Category ID: ";
            // 
            // lblClose
            // 
            lblClose.BackColor = Color.White;
            lblClose.Font = new Font("Cascadia Mono", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblClose.ForeColor = Color.FromArgb(60, 173, 104);
            lblClose.Location = new Point(323, 323);
            lblClose.Margin = new Padding(0);
            lblClose.Name = "lblClose";
            lblClose.Size = new Size(122, 48);
            lblClose.TabIndex = 45;
            lblClose.Text = "Close";
            lblClose.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDelete
            // 
            lblDelete.BackColor = Color.Red;
            lblDelete.Font = new Font("Cascadia Mono", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblDelete.ForeColor = Color.White;
            lblDelete.Location = new Point(21, 323);
            lblDelete.Margin = new Padding(0);
            lblDelete.Name = "lblDelete";
            lblDelete.Size = new Size(122, 48);
            lblDelete.TabIndex = 44;
            lblDelete.Text = "Delete";
            lblDelete.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSave
            // 
            lblSave.BackColor = Color.FromArgb(60, 173, 104);
            lblSave.Font = new Font("Cascadia Mono", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblSave.ForeColor = Color.White;
            lblSave.Location = new Point(456, 323);
            lblSave.Margin = new Padding(0);
            lblSave.Name = "lblSave";
            lblSave.Size = new Size(122, 48);
            lblSave.TabIndex = 43;
            lblSave.Text = "Save";
            lblSave.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cbIsActive
            // 
            cbIsActive.AutoSize = true;
            cbIsActive.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            cbIsActive.Location = new Point(479, 24);
            cbIsActive.Margin = new Padding(2);
            cbIsActive.Name = "cbIsActive";
            cbIsActive.Size = new Size(103, 35);
            cbIsActive.TabIndex = 46;
            cbIsActive.Text = "Active";
            cbIsActive.UseVisualStyleBackColor = true;
            // 
            // ManageUser
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 386);
            Controls.Add(cbIsActive);
            Controls.Add(lblClose);
            Controls.Add(lblDelete);
            Controls.Add(lblSave);
            Controls.Add(lblDescreptionError);
            Controls.Add(lblNameError);
            Controls.Add(tbDescription);
            Controls.Add(lblName);
            Controls.Add(lblId);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "ManageUser";
            Text = "ManageUser";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblDescreptionError;
        private Label lblNameError;
        private TextBox tbDescription;
        private TextBox lblName;
        private TextBox lblId;
        private Label label3;
        private Label label2;
        private Label label1;
        private Label lblClose;
        private Label lblDelete;
        private Label lblSave;
        private CheckBox cbIsActive;
    }
}