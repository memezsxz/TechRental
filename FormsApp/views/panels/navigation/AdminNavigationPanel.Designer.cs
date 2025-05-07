namespace FormsApp.views.panels
{
    partial class AdminNavigationPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminNavigationPanel));
            pnlPanel = new TableLayoutPanel();
            lblErrorLogs = new Label();
            lblDashboard = new Label();
            lblAuditTrails = new Label();
            panel1 = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            pnlLogo = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            pnlNotification = new Panel();
            pnlProfile = new Panel();
            lblRentalRecords = new Label();
            lblRentalRequests = new Label();
            lblEquipment = new Label();
            lblCategories = new Label();
            lblUsers = new Label();
            pnlPanel.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // pnlPanel
            // 
            pnlPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlPanel.BackColor = Color.White;
            pnlPanel.ColumnCount = 1;
            pnlPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlPanel.Controls.Add(lblErrorLogs, 0, 3);
            pnlPanel.Controls.Add(lblDashboard, 0, 1);
            pnlPanel.Controls.Add(lblAuditTrails, 0, 2);
            pnlPanel.Controls.Add(panel1, 0, 0);
            pnlPanel.Controls.Add(tableLayoutPanel1, 0, 9);
            pnlPanel.Controls.Add(lblRentalRecords, 0, 8);
            pnlPanel.Controls.Add(lblRentalRequests, 0, 7);
            pnlPanel.Controls.Add(lblEquipment, 0, 6);
            pnlPanel.Controls.Add(lblCategories, 0, 5);
            pnlPanel.Controls.Add(lblUsers, 0, 4);
            pnlPanel.Dock = DockStyle.Fill;
            pnlPanel.Location = new Point(0, 0);
            pnlPanel.Margin = new Padding(0);
            pnlPanel.Name = "pnlPanel";
            pnlPanel.RowCount = 10;
            pnlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            pnlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            pnlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            pnlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            pnlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            pnlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            pnlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            pnlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            pnlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            pnlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            pnlPanel.Size = new Size(340, 938);
            pnlPanel.TabIndex = 0;
            // 
            // lblErrorLogs
            // 
            lblErrorLogs.AutoSize = true;
            lblErrorLogs.Dock = DockStyle.Fill;
            lblErrorLogs.Font = new Font("Cascadia Mono", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lblErrorLogs.ForeColor = Color.Black;
            lblErrorLogs.Location = new Point(3, 299);
            lblErrorLogs.Margin = new Padding(3, 20, 3, 20);
            lblErrorLogs.Name = "lblErrorLogs";
            lblErrorLogs.Size = new Size(334, 53);
            lblErrorLogs.TabIndex = 16;
            lblErrorLogs.Text = "Error Logs";
            lblErrorLogs.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDashboard
            // 
            lblDashboard.AutoSize = true;
            lblDashboard.Dock = DockStyle.Fill;
            lblDashboard.Font = new Font("Cascadia Mono", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lblDashboard.ForeColor = Color.Black;
            lblDashboard.Location = new Point(3, 113);
            lblDashboard.Margin = new Padding(3, 20, 3, 20);
            lblDashboard.Name = "lblDashboard";
            lblDashboard.Size = new Size(334, 53);
            lblDashboard.TabIndex = 13;
            lblDashboard.Text = "Dashboard";
            lblDashboard.TextAlign = ContentAlignment.MiddleCenter;
            lblDashboard.Click += lblDashboard_Click;
            // 
            // lblAuditTrails
            // 
            lblAuditTrails.AutoSize = true;
            lblAuditTrails.Dock = DockStyle.Fill;
            lblAuditTrails.Font = new Font("Cascadia Mono", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lblAuditTrails.ForeColor = Color.Black;
            lblAuditTrails.Location = new Point(3, 206);
            lblAuditTrails.Margin = new Padding(3, 20, 3, 20);
            lblAuditTrails.Name = "lblAuditTrails";
            lblAuditTrails.Size = new Size(334, 53);
            lblAuditTrails.TabIndex = 12;
            lblAuditTrails.Text = "Audit Trails";
            lblAuditTrails.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            panel1.Controls.Add(tableLayoutPanel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(334, 87);
            panel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel2.BackgroundImageLayout = ImageLayout.Zoom;
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28.57143F));
            tableLayoutPanel2.Controls.Add(pnlLogo, 0, 0);
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.Padding = new Padding(20);
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(334, 87);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // pnlLogo
            // 
            pnlLogo.BackgroundImage = (Image)resources.GetObject("pnlLogo.BackgroundImage");
            pnlLogo.BackgroundImageLayout = ImageLayout.Zoom;
            pnlLogo.Dock = DockStyle.Fill;
            pnlLogo.Location = new Point(23, 23);
            pnlLogo.Name = "pnlLogo";
            pnlLogo.Size = new Size(288, 41);
            pnlLogo.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(pnlNotification, 0, 0);
            tableLayoutPanel1.Controls.Add(pnlProfile, 1, 0);
            tableLayoutPanel1.Location = new Point(0, 857);
            tableLayoutPanel1.Margin = new Padding(0, 20, 0, 20);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(340, 53);
            tableLayoutPanel1.TabIndex = 14;
            // 
            // pnlNotification
            // 
            pnlNotification.BackgroundImage = (Image)resources.GetObject("pnlNotification.BackgroundImage");
            pnlNotification.BackgroundImageLayout = ImageLayout.Zoom;
            pnlNotification.Location = new Point(40, 10);
            pnlNotification.Margin = new Padding(40, 10, 40, 10);
            pnlNotification.Name = "pnlNotification";
            pnlNotification.Size = new Size(90, 33);
            pnlNotification.TabIndex = 1;
            pnlNotification.Click += pnlNotification_Click;
            // 
            // pnlProfile
            // 
            pnlProfile.BackgroundImage = (Image)resources.GetObject("pnlProfile.BackgroundImage");
            pnlProfile.BackgroundImageLayout = ImageLayout.Zoom;
            pnlProfile.Location = new Point(210, 0);
            pnlProfile.Margin = new Padding(40, 0, 40, 0);
            pnlProfile.Name = "pnlProfile";
            pnlProfile.Size = new Size(90, 53);
            pnlProfile.TabIndex = 0;
            pnlProfile.Click += pnlProfile_Click;
            // 
            // lblRentalRecords
            // 
            lblRentalRecords.AutoSize = true;
            lblRentalRecords.Dock = DockStyle.Fill;
            lblRentalRecords.Font = new Font("Cascadia Mono", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lblRentalRecords.ForeColor = Color.Black;
            lblRentalRecords.Location = new Point(3, 764);
            lblRentalRecords.Margin = new Padding(3, 20, 3, 20);
            lblRentalRecords.Name = "lblRentalRecords";
            lblRentalRecords.Size = new Size(334, 53);
            lblRentalRecords.TabIndex = 15;
            lblRentalRecords.Text = "Rental Records";
            lblRentalRecords.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblRentalRequests
            // 
            lblRentalRequests.AutoSize = true;
            lblRentalRequests.Dock = DockStyle.Fill;
            lblRentalRequests.Font = new Font("Cascadia Mono", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lblRentalRequests.ForeColor = Color.Black;
            lblRentalRequests.Location = new Point(3, 671);
            lblRentalRequests.Margin = new Padding(3, 20, 3, 20);
            lblRentalRequests.Name = "lblRentalRequests";
            lblRentalRequests.Size = new Size(334, 53);
            lblRentalRequests.TabIndex = 8;
            lblRentalRequests.Text = "Rental Requests";
            lblRentalRequests.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblEquipment
            // 
            lblEquipment.AutoSize = true;
            lblEquipment.Dock = DockStyle.Fill;
            lblEquipment.Font = new Font("Cascadia Mono", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lblEquipment.ForeColor = Color.Black;
            lblEquipment.Location = new Point(3, 578);
            lblEquipment.Margin = new Padding(3, 20, 3, 20);
            lblEquipment.Name = "lblEquipment";
            lblEquipment.Size = new Size(334, 53);
            lblEquipment.TabIndex = 7;
            lblEquipment.Text = "Equipment";
            lblEquipment.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblCategories
            // 
            lblCategories.AutoSize = true;
            lblCategories.Dock = DockStyle.Fill;
            lblCategories.Font = new Font("Cascadia Mono", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lblCategories.ForeColor = Color.Black;
            lblCategories.Location = new Point(3, 485);
            lblCategories.Margin = new Padding(3, 20, 3, 20);
            lblCategories.Name = "lblCategories";
            lblCategories.Size = new Size(334, 53);
            lblCategories.TabIndex = 10;
            lblCategories.Text = "Categories";
            lblCategories.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblUsers
            // 
            lblUsers.AutoSize = true;
            lblUsers.Dock = DockStyle.Fill;
            lblUsers.Font = new Font("Cascadia Mono", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lblUsers.ForeColor = Color.Black;
            lblUsers.Location = new Point(3, 392);
            lblUsers.Margin = new Padding(3, 20, 3, 20);
            lblUsers.Name = "lblUsers";
            lblUsers.Size = new Size(334, 53);
            lblUsers.TabIndex = 11;
            lblUsers.Text = "Users";
            lblUsers.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // AdminNavigationPanel
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            BackColor = Color.FromArgb(159, 109, 224);
            Controls.Add(pnlPanel);
            Name = "AdminNavigationPanel";
            Size = new Size(340, 938);
            Load += AdminNavigationPanel_Load;
            pnlPanel.ResumeLayout(false);
            pnlPanel.PerformLayout();
            panel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel pnlPanel;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel pnlLogo;
        private Label lblDashboard;
        private Label lblAuditTrails;
        private Label lblUsers;
        private Label lblCategories;
        private Label lblRentalRequests;
        private Label lblEquipment;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel pnlProfile;
        private Panel pnlNotification;
        private Label lblErrorLogs;
        private Label lblRentalRecords;
    }
}
