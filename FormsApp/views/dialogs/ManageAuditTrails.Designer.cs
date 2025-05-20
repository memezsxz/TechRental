namespace FormsApp.views.dialogs
{
    partial class ManageAuditTrails
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
            tbActionType = new TextBox();
            tbId = new TextBox();
            label4 = new Label();
            label2 = new Label();
            label1 = new Label();
            tbSource = new TextBox();
            label11 = new Label();
            tbDataBeforeAction = new TextBox();
            label9 = new Label();
            tbAffectedRecordKey = new TextBox();
            tbDataAfterAction = new TextBox();
            label3 = new Label();
            tbSourceEntity = new TextBox();
            label5 = new Label();
            SuspendLayout();
            // 
            // lblClose
            // 
            lblClose.BackColor = Color.White;
            lblClose.Font = new Font("Cascadia Mono", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblClose.ForeColor = Color.FromArgb(60, 173, 104);
            lblClose.Location = new Point(324, 591);
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
            lblDelete.Location = new Point(19, 593);
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
            lblSave.Location = new Point(457, 591);
            lblSave.Margin = new Padding(0);
            lblSave.Name = "lblSave";
            lblSave.Size = new Size(122, 48);
            lblSave.TabIndex = 46;
            lblSave.Text = "Save";
            lblSave.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tbActionType
            // 
            tbActionType.BackColor = Color.FromArgb(247, 247, 249);
            tbActionType.Enabled = false;
            tbActionType.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbActionType.Location = new Point(189, 79);
            tbActionType.Margin = new Padding(2);
            tbActionType.Name = "tbActionType";
            tbActionType.Size = new Size(390, 38);
            tbActionType.TabIndex = 39;
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
            label4.Location = new Point(20, 321);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(150, 98);
            label4.TabIndex = 33;
            label4.Text = "Data Before Action:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(24, 79);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(139, 31);
            label2.TabIndex = 31;
            label2.Text = "Action Type:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(18, 21);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(91, 31);
            label1.TabIndex = 30;
            label1.Text = "Log ID: ";
            // 
            // tbSource
            // 
            tbSource.BackColor = Color.FromArgb(247, 247, 249);
            tbSource.Enabled = false;
            tbSource.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbSource.Location = new Point(189, 259);
            tbSource.Margin = new Padding(2);
            tbSource.Name = "tbSource";
            tbSource.Size = new Size(390, 38);
            tbSource.TabIndex = 57;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label11.Location = new Point(21, 259);
            label11.Margin = new Padding(2, 0, 2, 0);
            label11.Name = "label11";
            label11.Size = new Size(88, 31);
            label11.TabIndex = 56;
            label11.Text = "Source:";
            // 
            // tbDataBeforeAction
            // 
            tbDataBeforeAction.BackColor = Color.FromArgb(247, 247, 249);
            tbDataBeforeAction.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbDataBeforeAction.Location = new Point(189, 321);
            tbDataBeforeAction.Margin = new Padding(2);
            tbDataBeforeAction.Multiline = true;
            tbDataBeforeAction.Name = "tbDataBeforeAction";
            tbDataBeforeAction.ReadOnly = true;
            tbDataBeforeAction.Size = new Size(394, 114);
            tbDataBeforeAction.TabIndex = 40;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label9.Location = new Point(23, 141);
            label9.Margin = new Padding(2, 0, 2, 0);
            label9.Name = "label9";
            label9.Size = new Size(224, 31);
            label9.TabIndex = 53;
            label9.Text = "Affected Record Key:";
            // 
            // tbAffectedRecordKey
            // 
            tbAffectedRecordKey.BackColor = Color.FromArgb(247, 247, 249);
            tbAffectedRecordKey.Enabled = false;
            tbAffectedRecordKey.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbAffectedRecordKey.Location = new Point(251, 138);
            tbAffectedRecordKey.Margin = new Padding(2);
            tbAffectedRecordKey.Name = "tbAffectedRecordKey";
            tbAffectedRecordKey.Size = new Size(328, 38);
            tbAffectedRecordKey.TabIndex = 54;
            // 
            // tbDataAfterAction
            // 
            tbDataAfterAction.BackColor = Color.FromArgb(247, 247, 249);
            tbDataAfterAction.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbDataAfterAction.Location = new Point(189, 456);
            tbDataAfterAction.Margin = new Padding(2);
            tbDataAfterAction.Multiline = true;
            tbDataAfterAction.Name = "tbDataAfterAction";
            tbDataAfterAction.ReadOnly = true;
            tbDataAfterAction.Size = new Size(394, 114);
            tbDataAfterAction.TabIndex = 59;
            // 
            // label3
            // 
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(20, 456);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(150, 98);
            label3.TabIndex = 58;
            label3.Text = "Data After Action:";
            // 
            // tbSourceEntity
            // 
            tbSourceEntity.BackColor = Color.FromArgb(247, 247, 249);
            tbSourceEntity.Enabled = false;
            tbSourceEntity.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbSourceEntity.Location = new Point(189, 199);
            tbSourceEntity.Margin = new Padding(2);
            tbSourceEntity.Name = "tbSourceEntity";
            tbSourceEntity.Size = new Size(390, 38);
            tbSourceEntity.TabIndex = 61;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(20, 199);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(152, 31);
            label5.TabIndex = 60;
            label5.Text = "Source Entity:";
            // 
            // ManageAuditTrails
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(600, 659);
            Controls.Add(tbSourceEntity);
            Controls.Add(label5);
            Controls.Add(tbDataAfterAction);
            Controls.Add(label3);
            Controls.Add(tbSource);
            Controls.Add(label11);
            Controls.Add(tbAffectedRecordKey);
            Controls.Add(label9);
            Controls.Add(lblClose);
            Controls.Add(lblDelete);
            Controls.Add(lblSave);
            Controls.Add(tbDataBeforeAction);
            Controls.Add(tbActionType);
            Controls.Add(tbId);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "ManageAuditTrails";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Manage Log";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lblClose;
        private Label lblDelete;
        private Label lblSave;
        private TextBox tbActionType;
        private TextBox tbId;
        private Label label4;
        private Label label2;
        private Label label1;
        private TextBox tbSource;
        private Label label11;
        private TextBox tbDataBeforeAction;
        private Label label9;
        private TextBox tbAffectedRecordKey;
        private TextBox tbDataAfterAction;
        private Label label3;
        private TextBox tbSourceEntity;
        private Label label5;
    }
}