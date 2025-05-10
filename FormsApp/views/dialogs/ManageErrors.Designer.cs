namespace FormsApp.views.dialogs
{
    partial class ManageErrors
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
            tbUserId = new TextBox();
            tbId = new TextBox();
            label4 = new Label();
            label2 = new Label();
            tbSourceProseadure = new TextBox();
            label9 = new Label();
            tbErrorMessage = new TextBox();
            label3 = new Label();
            dtpTimestamp = new DateTimePicker();
            label11 = new Label();
            tbSource = new TextBox();
            SuspendLayout();
            // 
            // lblClose
            // 
            lblClose.BackColor = Color.White;
            lblClose.Font = new Font("Cascadia Mono", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblClose.ForeColor = Color.FromArgb(60, 173, 104);
            lblClose.Location = new Point(324, 538);
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
            lblDelete.Location = new Point(19, 540);
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
            lblSave.Location = new Point(457, 538);
            lblSave.Margin = new Padding(0);
            lblSave.Name = "lblSave";
            lblSave.Size = new Size(122, 48);
            lblSave.TabIndex = 46;
            lblSave.Text = "Save";
            lblSave.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tbUserId
            // 
            tbUserId.BackColor = Color.FromArgb(247, 247, 249);
            tbUserId.Enabled = false;
            tbUserId.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbUserId.Location = new Point(189, 79);
            tbUserId.Margin = new Padding(2);
            tbUserId.Name = "tbUserId";
            tbUserId.Size = new Size(390, 38);
            tbUserId.TabIndex = 39;
            // 
            // tbId
            // 
            tbId.BackColor = Color.FromArgb(247, 247, 249);
            tbId.Enabled = false;
            tbId.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbId.Location = new Point(189, 20);
            tbId.Margin = new Padding(2);
            tbId.Name = "tbId";
            tbId.Size = new Size(390, 38);
            tbId.TabIndex = 38;
            // 
            // label4
            // 
            label4.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(20, 260);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(150, 98);
            label4.TabIndex = 33;
            label4.Text = "Source Procedure:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(21, 79);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(93, 31);
            label2.TabIndex = 31;
            label2.Text = "User ID:";
            // 
            // tbSourceProseadure
            // 
            tbSourceProseadure.BackColor = Color.FromArgb(247, 247, 249);
            tbSourceProseadure.Enabled = false;
            tbSourceProseadure.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbSourceProseadure.Location = new Point(189, 260);
            tbSourceProseadure.Margin = new Padding(2);
            tbSourceProseadure.Multiline = true;
            tbSourceProseadure.Name = "tbSourceProseadure";
            tbSourceProseadure.Size = new Size(394, 114);
            tbSourceProseadure.TabIndex = 40;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label9.Location = new Point(20, 141);
            label9.Margin = new Padding(2, 0, 2, 0);
            label9.Name = "label9";
            label9.Size = new Size(133, 31);
            label9.TabIndex = 53;
            label9.Text = "Timestamp:";
            // 
            // tbErrorMessage
            // 
            tbErrorMessage.BackColor = Color.FromArgb(247, 247, 249);
            tbErrorMessage.Enabled = false;
            tbErrorMessage.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbErrorMessage.Location = new Point(189, 395);
            tbErrorMessage.Margin = new Padding(2);
            tbErrorMessage.Multiline = true;
            tbErrorMessage.Name = "tbErrorMessage";
            tbErrorMessage.Size = new Size(394, 114);
            tbErrorMessage.TabIndex = 59;
            // 
            // label3
            // 
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(20, 395);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(150, 98);
            label3.TabIndex = 58;
            label3.Text = "Error Message:";
            // 
            // dtpTimestamp
            // 
            dtpTimestamp.Enabled = false;
            dtpTimestamp.Location = new Point(189, 141);
            dtpTimestamp.Name = "dtpTimestamp";
            dtpTimestamp.Size = new Size(390, 34);
            dtpTimestamp.TabIndex = 62;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label11.Location = new Point(21, 198);
            label11.Margin = new Padding(2, 0, 2, 0);
            label11.Name = "label11";
            label11.Size = new Size(88, 31);
            label11.TabIndex = 56;
            label11.Text = "Source:";
            // 
            // tbSource
            // 
            tbSource.BackColor = Color.FromArgb(247, 247, 249);
            tbSource.Enabled = false;
            tbSource.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbSource.Location = new Point(189, 198);
            tbSource.Margin = new Padding(2);
            tbSource.Name = "tbSource";
            tbSource.Size = new Size(390, 38);
            tbSource.TabIndex = 57;
            // 
            // ManageErrors
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(600, 604);
            Controls.Add(dtpTimestamp);
            Controls.Add(tbErrorMessage);
            Controls.Add(label3);
            Controls.Add(tbSource);
            Controls.Add(label11);
            Controls.Add(label9);
            Controls.Add(lblClose);
            Controls.Add(lblDelete);
            Controls.Add(lblSave);
            Controls.Add(tbSourceProseadure);
            Controls.Add(tbUserId);
            Controls.Add(tbId);
            Controls.Add(label4);
            Controls.Add(label2);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "ManageErrors";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Manage Error";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lblClose;
        private Label lblDelete;
        private Label lblSave;
        private TextBox tbUserId;
        private TextBox tbId;
        private Label label4;
        private Label label2;
        private TextBox tbSourceProseadure;
        private Label label9;
        private TextBox tbErrorMessage;
        private Label label3;
        private DateTimePicker dtpTimestamp;
        private Label label11;
        private TextBox tbSource;
    }
}