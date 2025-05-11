namespace FormsApp.views.panels
{
    partial class ManagerDashboardView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManagerDashboardView));
            label1 = new Label();
            panel1 = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel12 = new Panel();
            tableLayoutPanel7 = new TableLayoutPanel();
            lblDamaged = new Label();
            panel13 = new Panel();
            panel10 = new Panel();
            tableLayoutPanel6 = new TableLayoutPanel();
            panel11 = new Panel();
            lblOverdue = new Label();
            panel8 = new Panel();
            tableLayoutPanel5 = new TableLayoutPanel();
            panel9 = new Panel();
            lblCompleted = new Label();
            panel6 = new Panel();
            tableLayoutPanel4 = new TableLayoutPanel();
            panel7 = new Panel();
            lblTodaysPickups = new Label();
            panel4 = new Panel();
            tableLayoutPanel3 = new TableLayoutPanel();
            panel5 = new Panel();
            lblOngoing = new Label();
            panel2 = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel3 = new Panel();
            lblTotalRentals = new Label();
            tableLayoutPanel9 = new TableLayoutPanel();
            flowLayoutPanel6 = new FlowLayoutPanel();
            pnlRefresh = new Panel();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel12.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            panel10.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            panel8.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            panel6.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            panel4.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            panel2.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel9.SuspendLayout();
            flowLayoutPanel6.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI Black", 30F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(732, 86);
            label1.TabIndex = 0;
            label1.Text = "Dashboard";
            // 
            // panel1
            // 
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Controls.Add(tableLayoutPanel9);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1477, 922);
            panel1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Controls.Add(panel12, 2, 1);
            tableLayoutPanel1.Controls.Add(panel10, 1, 1);
            tableLayoutPanel1.Controls.Add(panel8, 0, 1);
            tableLayoutPanel1.Controls.Add(panel6, 0, 0);
            tableLayoutPanel1.Controls.Add(panel4, 2, 0);
            tableLayoutPanel1.Controls.Add(panel2, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 86);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(1477, 836);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // panel12
            // 
            panel12.BackColor = Color.FromArgb(200, 228, 211);
            panel12.Controls.Add(tableLayoutPanel7);
            panel12.Dock = DockStyle.Fill;
            panel12.Location = new Point(1024, 458);
            panel12.Margin = new Padding(40);
            panel12.Name = "panel12";
            panel12.Size = new Size(413, 338);
            panel12.TabIndex = 4;
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.ColumnCount = 1;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.Controls.Add(lblDamaged, 0, 1);
            tableLayoutPanel7.Controls.Add(panel13, 0, 0);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(0, 0);
            tableLayoutPanel7.Margin = new Padding(20);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.Padding = new Padding(20);
            tableLayoutPanel7.RowCount = 2;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel7.Size = new Size(413, 338);
            tableLayoutPanel7.TabIndex = 0;
            // 
            // lblDamaged
            // 
            lblDamaged.AutoSize = true;
            lblDamaged.Dock = DockStyle.Fill;
            lblDamaged.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblDamaged.Location = new Point(23, 258);
            lblDamaged.Name = "lblDamaged";
            lblDamaged.Size = new Size(367, 60);
            lblDamaged.TabIndex = 2;
            lblDamaged.Text = "Damaged Equipment: 10";
            lblDamaged.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel13
            // 
            panel13.BackgroundImage = (Image)resources.GetObject("panel13.BackgroundImage");
            panel13.BackgroundImageLayout = ImageLayout.Zoom;
            panel13.Dock = DockStyle.Fill;
            panel13.Location = new Point(23, 23);
            panel13.Name = "panel13";
            panel13.Size = new Size(367, 232);
            panel13.TabIndex = 0;
            // 
            // panel10
            // 
            panel10.BackColor = Color.FromArgb(200, 228, 211);
            panel10.Controls.Add(tableLayoutPanel6);
            panel10.Dock = DockStyle.Fill;
            panel10.Location = new Point(532, 458);
            panel10.Margin = new Padding(40);
            panel10.Name = "panel10";
            panel10.Size = new Size(412, 338);
            panel10.TabIndex = 3;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 1;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.Controls.Add(panel11, 0, 0);
            tableLayoutPanel6.Controls.Add(lblOverdue, 0, 1);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(0, 0);
            tableLayoutPanel6.Margin = new Padding(20);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.Padding = new Padding(20);
            tableLayoutPanel6.RowCount = 2;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel6.Size = new Size(412, 338);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // panel11
            // 
            panel11.BackgroundImage = (Image)resources.GetObject("panel11.BackgroundImage");
            panel11.BackgroundImageLayout = ImageLayout.Zoom;
            panel11.Dock = DockStyle.Fill;
            panel11.Location = new Point(23, 23);
            panel11.Name = "panel11";
            panel11.Size = new Size(366, 232);
            panel11.TabIndex = 0;
            // 
            // lblOverdue
            // 
            lblOverdue.AutoSize = true;
            lblOverdue.Dock = DockStyle.Fill;
            lblOverdue.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblOverdue.Location = new Point(23, 258);
            lblOverdue.Name = "lblOverdue";
            lblOverdue.Size = new Size(366, 60);
            lblOverdue.TabIndex = 1;
            lblOverdue.Text = "Overdue Rentals: 10";
            lblOverdue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel8
            // 
            panel8.BackColor = Color.FromArgb(200, 228, 211);
            panel8.Controls.Add(tableLayoutPanel5);
            panel8.Dock = DockStyle.Fill;
            panel8.Location = new Point(40, 458);
            panel8.Margin = new Padding(40);
            panel8.Name = "panel8";
            panel8.Size = new Size(412, 338);
            panel8.TabIndex = 2;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 1;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Controls.Add(panel9, 0, 0);
            tableLayoutPanel5.Controls.Add(lblCompleted, 0, 1);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(0, 0);
            tableLayoutPanel5.Margin = new Padding(20);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.Padding = new Padding(20);
            tableLayoutPanel5.RowCount = 2;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel5.Size = new Size(412, 338);
            tableLayoutPanel5.TabIndex = 0;
            // 
            // panel9
            // 
            panel9.BackgroundImage = (Image)resources.GetObject("panel9.BackgroundImage");
            panel9.BackgroundImageLayout = ImageLayout.Zoom;
            panel9.Dock = DockStyle.Fill;
            panel9.Location = new Point(23, 23);
            panel9.Name = "panel9";
            panel9.Size = new Size(366, 232);
            panel9.TabIndex = 0;
            // 
            // lblCompleted
            // 
            lblCompleted.AutoSize = true;
            lblCompleted.Dock = DockStyle.Fill;
            lblCompleted.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblCompleted.Location = new Point(23, 258);
            lblCompleted.Name = "lblCompleted";
            lblCompleted.Size = new Size(366, 60);
            lblCompleted.TabIndex = 1;
            lblCompleted.Text = "Compleated Rentals: 10";
            lblCompleted.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            panel6.BackColor = Color.FromArgb(200, 228, 211);
            panel6.Controls.Add(tableLayoutPanel4);
            panel6.Dock = DockStyle.Fill;
            panel6.Location = new Point(40, 40);
            panel6.Margin = new Padding(40);
            panel6.Name = "panel6";
            panel6.Size = new Size(412, 338);
            panel6.TabIndex = 1;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(panel7, 0, 0);
            tableLayoutPanel4.Controls.Add(lblTodaysPickups, 0, 1);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(0, 0);
            tableLayoutPanel4.Margin = new Padding(20);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.Padding = new Padding(20);
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.Size = new Size(412, 338);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // panel7
            // 
            panel7.BackgroundImage = (Image)resources.GetObject("panel7.BackgroundImage");
            panel7.BackgroundImageLayout = ImageLayout.Zoom;
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(23, 23);
            panel7.Name = "panel7";
            panel7.Size = new Size(366, 232);
            panel7.TabIndex = 0;
            // 
            // lblTodaysPickups
            // 
            lblTodaysPickups.AutoSize = true;
            lblTodaysPickups.Dock = DockStyle.Fill;
            lblTodaysPickups.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblTodaysPickups.Location = new Point(23, 258);
            lblTodaysPickups.Name = "lblTodaysPickups";
            lblTodaysPickups.Size = new Size(366, 60);
            lblTodaysPickups.TabIndex = 1;
            lblTodaysPickups.Text = "Today's Pickups: 10";
            lblTodaysPickups.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(200, 228, 211);
            panel4.Controls.Add(tableLayoutPanel3);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(1024, 40);
            panel4.Margin = new Padding(40);
            panel4.Name = "panel4";
            panel4.Size = new Size(413, 338);
            panel4.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(panel5, 0, 0);
            tableLayoutPanel3.Controls.Add(lblOngoing, 0, 1);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Margin = new Padding(20);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.Padding = new Padding(20);
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.Size = new Size(413, 338);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // panel5
            // 
            panel5.BackgroundImage = (Image)resources.GetObject("panel5.BackgroundImage");
            panel5.BackgroundImageLayout = ImageLayout.Zoom;
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(23, 23);
            panel5.Name = "panel5";
            panel5.Size = new Size(367, 232);
            panel5.TabIndex = 0;
            // 
            // lblOngoing
            // 
            lblOngoing.AutoSize = true;
            lblOngoing.Dock = DockStyle.Fill;
            lblOngoing.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblOngoing.Location = new Point(23, 258);
            lblOngoing.Name = "lblOngoing";
            lblOngoing.Size = new Size(367, 60);
            lblOngoing.TabIndex = 1;
            lblOngoing.Text = "Ongoing Rentals: 0";
            lblOngoing.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(200, 228, 211);
            panel2.Controls.Add(tableLayoutPanel2);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(532, 40);
            panel2.Margin = new Padding(40);
            panel2.Name = "panel2";
            panel2.Size = new Size(412, 338);
            panel2.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(panel3, 0, 0);
            tableLayoutPanel2.Controls.Add(lblTotalRentals, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Margin = new Padding(20);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.Padding = new Padding(20);
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.Size = new Size(412, 338);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.BackgroundImage = (Image)resources.GetObject("panel3.BackgroundImage");
            panel3.BackgroundImageLayout = ImageLayout.Zoom;
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(23, 23);
            panel3.Name = "panel3";
            panel3.Size = new Size(366, 232);
            panel3.TabIndex = 0;
            // 
            // lblTotalRentals
            // 
            lblTotalRentals.AutoSize = true;
            lblTotalRentals.Dock = DockStyle.Fill;
            lblTotalRentals.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblTotalRentals.Location = new Point(23, 258);
            lblTotalRentals.Name = "lblTotalRentals";
            lblTotalRentals.Size = new Size(366, 60);
            lblTotalRentals.TabIndex = 1;
            lblTotalRentals.Text = "Total Rentals: 10";
            lblTotalRentals.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel9
            // 
            tableLayoutPanel9.ColumnCount = 2;
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel9.Controls.Add(label1, 0, 0);
            tableLayoutPanel9.Controls.Add(flowLayoutPanel6, 1, 0);
            tableLayoutPanel9.Dock = DockStyle.Top;
            tableLayoutPanel9.Location = new Point(0, 0);
            tableLayoutPanel9.Name = "tableLayoutPanel9";
            tableLayoutPanel9.RowCount = 1;
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel9.Size = new Size(1477, 86);
            tableLayoutPanel9.TabIndex = 1;
            // 
            // flowLayoutPanel6
            // 
            flowLayoutPanel6.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel6.AutoSize = true;
            flowLayoutPanel6.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel6.Controls.Add(pnlRefresh);
            flowLayoutPanel6.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel6.Location = new Point(741, 16);
            flowLayoutPanel6.Margin = new Padding(3, 3, 20, 3);
            flowLayoutPanel6.Name = "flowLayoutPanel6";
            flowLayoutPanel6.Size = new Size(716, 53);
            flowLayoutPanel6.TabIndex = 1;
            // 
            // pnlRefresh
            // 
            pnlRefresh.Anchor = AnchorStyles.Left;
            pnlRefresh.BackgroundImage = (Image)resources.GetObject("pnlRefresh.BackgroundImage");
            pnlRefresh.BackgroundImageLayout = ImageLayout.Zoom;
            pnlRefresh.Location = new Point(658, 5);
            pnlRefresh.Margin = new Padding(5);
            pnlRefresh.Name = "pnlRefresh";
            pnlRefresh.Size = new Size(53, 43);
            pnlRefresh.TabIndex = 11;
            pnlRefresh.Click += pnlRefresh_Click;
            // 
            // ManagerDashboardView
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(panel1);
            Margin = new Padding(10);
            Name = "ManagerDashboardView";
            Size = new Size(1477, 922);
            Load += admin_dashboard_Load;
            panel1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panel12.ResumeLayout(false);
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel7.PerformLayout();
            panel10.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            panel8.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            panel6.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            panel4.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            panel2.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel9.ResumeLayout(false);
            tableLayoutPanel9.PerformLayout();
            flowLayoutPanel6.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel9;
        private TableLayoutPanel tableLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel6;
        private Panel pnlRefresh;
        private Panel panel2;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel3;
        private Label lblTotalRentals;
        private Panel panel12;
        private TableLayoutPanel tableLayoutPanel7;
        private Panel panel13;
        private Panel panel10;
        private TableLayoutPanel tableLayoutPanel6;
        private Panel panel11;
        private Label lblOverdue;
        private Panel panel8;
        private TableLayoutPanel tableLayoutPanel5;
        private Panel panel9;
        private Label lblCompleted;
        private Panel panel6;
        private TableLayoutPanel tableLayoutPanel4;
        private Panel panel7;
        private Label lblTodaysPickups;
        private Panel panel4;
        private TableLayoutPanel tableLayoutPanel3;
        private Panel panel5;
        private Label lblOngoing;
        private Label lblDamaged;
    }
}
