namespace FormsApp
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel1 = new Panel();
            lblPasswordError = new Label();
            lblLogin = new Label();
            lblLoginError = new Label();
            lblEmailError = new Label();
            tbPassword = new TextBox();
            tbEmail = new TextBox();
            label2 = new Label();
            label1 = new Label();
            panel2 = new Panel();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.White;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28.57142F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42.8571434F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28.57143F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 22.4719124F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 55.0561829F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 22.4719124F));
            tableLayoutPanel1.Size = new Size(1433, 927);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(panel1, 0, 1);
            tableLayoutPanel2.Controls.Add(panel2, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(412, 211);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            tableLayoutPanel2.Size = new Size(608, 504);
            tableLayoutPanel2.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(lblPasswordError);
            panel1.Controls.Add(lblLogin);
            panel1.Controls.Add(lblLoginError);
            panel1.Controls.Add(lblEmailError);
            panel1.Controls.Add(tbPassword);
            panel1.Controls.Add(tbEmail);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 151);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(608, 353);
            panel1.TabIndex = 0;
            // 
            // lblPasswordError
            // 
            lblPasswordError.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblPasswordError.AutoSize = true;
            lblPasswordError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblPasswordError.ForeColor = Color.Red;
            lblPasswordError.Location = new Point(204, 174);
            lblPasswordError.Name = "lblPasswordError";
            lblPasswordError.Size = new Size(373, 25);
            lblPasswordError.TabIndex = 46;
            lblPasswordError.Text = "Password must be longer than 5 charecters";
            // 
            // lblLogin
            // 
            lblLogin.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblLogin.BackColor = Color.FromArgb(60, 173, 104);
            lblLogin.Font = new Font("Cascadia Mono", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblLogin.ForeColor = Color.White;
            lblLogin.Location = new Point(238, 280);
            lblLogin.Margin = new Padding(0);
            lblLogin.Name = "lblLogin";
            lblLogin.Size = new Size(122, 48);
            lblLogin.TabIndex = 45;
            lblLogin.Text = "Login";
            lblLogin.TextAlign = ContentAlignment.MiddleCenter;
            lblLogin.Click += lblLogin_Click;
            // 
            // lblLoginError
            // 
            lblLoginError.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblLoginError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblLoginError.ForeColor = Color.Red;
            lblLoginError.Location = new Point(127, 228);
            lblLoginError.Name = "lblLoginError";
            lblLoginError.Size = new Size(373, 25);
            lblLoginError.TabIndex = 44;
            lblLoginError.Text = "Invalid email or password, please try again.";
            lblLoginError.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblEmailError
            // 
            lblEmailError.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblEmailError.AutoSize = true;
            lblEmailError.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblEmailError.ForeColor = Color.Red;
            lblEmailError.Location = new Point(203, 90);
            lblEmailError.Name = "lblEmailError";
            lblEmailError.Size = new Size(226, 25);
            lblEmailError.TabIndex = 43;
            lblEmailError.Text = "Please enter a valid email";
            // 
            // tbPassword
            // 
            tbPassword.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tbPassword.BackColor = Color.FromArgb(247, 247, 249);
            tbPassword.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbPassword.Location = new Point(203, 132);
            tbPassword.Margin = new Padding(2);
            tbPassword.Name = "tbPassword";
            tbPassword.PasswordChar = '*';
            tbPassword.Size = new Size(333, 38);
            tbPassword.TabIndex = 39;
            // 
            // tbEmail
            // 
            tbEmail.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tbEmail.BackColor = Color.FromArgb(247, 247, 249);
            tbEmail.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbEmail.Location = new Point(203, 48);
            tbEmail.Margin = new Padding(2);
            tbEmail.Name = "tbEmail";
            tbEmail.Size = new Size(333, 38);
            tbEmail.TabIndex = 38;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(66, 132);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(115, 31);
            label2.TabIndex = 37;
            label2.Text = "Password:";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(106, 48);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(75, 31);
            label1.TabIndex = 36;
            label1.Text = "Email:";
            // 
            // panel2
            // 
            panel2.BackgroundImage = (Image)resources.GetObject("panel2.BackgroundImage");
            panel2.BackgroundImageLayout = ImageLayout.Zoom;
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(602, 145);
            panel2.TabIndex = 1;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            ClientSize = new Size(1433, 927);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Login";
            Text = "TechRental";
            Load += Login_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private TextBox tbPassword;
        private TextBox tbEmail;
        private Label label2;
        private Label label1;
        private Label lblLoginError;
        private Label lblEmailError;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel2;
        private Label lblLogin;
        private Label lblPasswordError;
    }
}