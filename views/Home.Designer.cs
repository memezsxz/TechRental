namespace TechRental
{
    partial class Home
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlMainView = new Panel();
            pnlNavigation = new Panel();
            SuspendLayout();
            // 
            // pnlMainView
            // 
            pnlMainView.Dock = DockStyle.Fill;
            pnlMainView.Location = new Point(340, 0);
            pnlMainView.Margin = new Padding(20);
            pnlMainView.Name = "pnlMainView";
            pnlMainView.Padding = new Padding(20);
            pnlMainView.Size = new Size(1136, 938);
            pnlMainView.TabIndex = 1;
            // 
            // pnlNavigation
            // 
            pnlNavigation.Dock = DockStyle.Left;
            pnlNavigation.Location = new Point(0, 0);
            pnlNavigation.Name = "pnlNavigation";
            pnlNavigation.Size = new Size(340, 938);
            pnlNavigation.TabIndex = 0;
            // 
            // Home
            // 
            AutoScaleDimensions = new SizeF(163F, 163F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(240, 241, 245);
            ClientSize = new Size(1476, 938);
            Controls.Add(pnlMainView);
            Controls.Add(pnlNavigation);
            Name = "Home";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TechRental";
            Load += Home_Load;
            ResumeLayout(false);
        }

        #endregion
        private Panel pnlMainView;
        private Panel pnlNavigation;
    }
}
