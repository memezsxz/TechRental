namespace FormsApp.views.panels
{
    partial class BaseNavigationPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseNavigationPanel));
            pnlPanel = new TableLayoutPanel();
            tlpProfile = new TableLayoutPanel();
            pnlProfile = new Panel();
            lblDashboard = new Label();
            panel1 = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            pnlLogo = new Panel();
            pnlPanel.SuspendLayout();
            tlpProfile.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // pnlPanel
            // 
            pnlPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlPanel.BackColor = Color.White;
            pnlPanel.ColumnCount = 1;
            pnlPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlPanel.Controls.Add(tlpProfile, 0, 2);
            pnlPanel.Controls.Add(lblDashboard, 0, 1);
            pnlPanel.Controls.Add(panel1, 0, 0);
            pnlPanel.Dock = DockStyle.Fill;
            pnlPanel.Location = new Point(0, 0);
            pnlPanel.Margin = new Padding(0);
            pnlPanel.Name = "pnlPanel";
            pnlPanel.RowCount = 3;
            pnlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            pnlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 71.42858F));
            pnlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 14.2857141F));
            pnlPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            pnlPanel.Size = new Size(340, 938);
            pnlPanel.TabIndex = 0;
            // 
            // tlpProfile
            // 
            tlpProfile.ColumnCount = 1;
            tlpProfile.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpProfile.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tlpProfile.Controls.Add(pnlProfile, 0, 0);
            tlpProfile.Dock = DockStyle.Fill;
            tlpProfile.Location = new Point(0, 823);
            tlpProfile.Margin = new Padding(0, 20, 0, 20);
            tlpProfile.Name = "tlpProfile";
            tlpProfile.RowCount = 1;
            tlpProfile.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpProfile.Size = new Size(340, 95);
            tlpProfile.TabIndex = 15;
            // 
            // pnlProfile
            // 
            pnlProfile.BackgroundImage = (Image)resources.GetObject("pnlProfile.BackgroundImage");
            pnlProfile.BackgroundImageLayout = ImageLayout.Zoom;
            pnlProfile.Dock = DockStyle.Fill;
            pnlProfile.Location = new Point(40, 0);
            pnlProfile.Margin = new Padding(40, 0, 40, 0);
            pnlProfile.Name = "pnlProfile";
            pnlProfile.Size = new Size(260, 95);
            pnlProfile.TabIndex = 0;
            pnlProfile.Click += pnlProfile_Click;
            // 
            // lblDashboard
            // 
            lblDashboard.AutoSize = true;
            lblDashboard.Dock = DockStyle.Fill;
            lblDashboard.Font = new Font("Cascadia Mono", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lblDashboard.ForeColor = Color.Black;
            lblDashboard.Location = new Point(3, 153);
            lblDashboard.Margin = new Padding(3, 20, 3, 20);
            lblDashboard.Name = "lblDashboard";
            lblDashboard.Size = new Size(334, 630);
            lblDashboard.TabIndex = 13;
            lblDashboard.Text = "Dashboard";
            lblDashboard.TextAlign = ContentAlignment.MiddleCenter;
            lblDashboard.Click += lblDashboard_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(tableLayoutPanel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(334, 127);
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
            tableLayoutPanel2.Size = new Size(334, 127);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // pnlLogo
            // 
            pnlLogo.BackgroundImage = (Image)resources.GetObject("pnlLogo.BackgroundImage");
            pnlLogo.BackgroundImageLayout = ImageLayout.Zoom;
            pnlLogo.Dock = DockStyle.Fill;
            pnlLogo.Location = new Point(23, 23);
            pnlLogo.Name = "pnlLogo";
            pnlLogo.Size = new Size(288, 81);
            pnlLogo.TabIndex = 0;
            // 
            // BaseNavigationPanel
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            BackColor = Color.FromArgb(159, 109, 224);
            Controls.Add(pnlPanel);
            Name = "BaseNavigationPanel";
            Size = new Size(340, 938);
            Load += AdminNavigationPanel_Load;
            pnlPanel.ResumeLayout(false);
            pnlPanel.PerformLayout();
            tlpProfile.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel pnlPanel;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel pnlLogo;
        private Label lblDashboard;
        private TableLayoutPanel tlpProfile;
        private Panel pnlProfile;
    }
}
