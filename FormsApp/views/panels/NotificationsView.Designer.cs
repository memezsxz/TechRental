namespace FormsApp.views.panels
{
    partial class NotificationsView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvNotifications = new DataGridView();
            label1 = new Label();
            lblAllRead = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvNotifications).BeginInit();
            SuspendLayout();
            // 
            // dgvNotifications
            // 
            dgvNotifications.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvNotifications.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvNotifications.Location = new Point(34, 193);
            dgvNotifications.Name = "dgvNotifications";
            dgvNotifications.RowHeadersWidth = 70;
            dgvNotifications.RowTemplate.Height = 36;
            dgvNotifications.Size = new Size(1116, 613);
            dgvNotifications.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI Black", 30F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(20, 20);
            label1.Name = "label1";
            label1.Size = new Size(476, 91);
            label1.TabIndex = 1;
            label1.Text = "Notifications";
            // 
            // lblAllRead
            // 
            lblAllRead.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblAllRead.AutoSize = true;
            lblAllRead.BackColor = Color.White;
            lblAllRead.Font = new Font("Cascadia Mono", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblAllRead.ForeColor = Color.FromArgb(60, 173, 104);
            lblAllRead.Location = new Point(909, 125);
            lblAllRead.Margin = new Padding(0);
            lblAllRead.Name = "lblAllRead";
            lblAllRead.Padding = new Padding(10);
            lblAllRead.Size = new Size(241, 50);
            lblAllRead.TabIndex = 47;
            lblAllRead.Text = "Mark All As Read";
            lblAllRead.TextAlign = ContentAlignment.MiddleCenter;
            lblAllRead.Click += lblAllRead_Click;
            // 
            // NotificationsView
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(lblAllRead);
            Controls.Add(label1);
            Controls.Add(dgvNotifications);
            Margin = new Padding(20);
            Name = "NotificationsView";
            Padding = new Padding(20);
            Size = new Size(1185, 836);
            ((System.ComponentModel.ISupportInitialize)dgvNotifications).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvNotifications;
        private Label label1;
        private Label lblAllRead;
    }
}
