namespace FormsApp
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
            pnlMainView.Location = new Point(540, 0);
            pnlMainView.Margin = new Padding(32, 32, 32, 32);
            pnlMainView.Name = "pnlMainView";
            pnlMainView.Padding = new Padding(32, 32, 32, 32);
            pnlMainView.Size = new Size(1805, 1490);
            pnlMainView.TabIndex = 1;
            // 
            // pnlNavigation
            // 
            pnlNavigation.Dock = DockStyle.Left;
            pnlNavigation.Location = new Point(0, 0);
            pnlNavigation.Margin = new Padding(5, 5, 5, 5);
            pnlNavigation.Name = "pnlNavigation";
            pnlNavigation.Size = new Size(540, 1490);
            pnlNavigation.TabIndex = 0;
            // 
            // Home
            // 
            AutoScaleDimensions = new SizeF(18F, 45F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 241, 245);
            ClientSize = new Size(2345, 1490);
            Controls.Add(pnlMainView);
            Controls.Add(pnlNavigation);
            Margin = new Padding(5, 5, 5, 5);
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
