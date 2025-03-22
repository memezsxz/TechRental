namespace FormsApp.views.panels
{
    partial class admin_inventory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(admin_inventory));
            tableLayoutPanel1 = new TableLayoutPanel();
            flpSearch = new FlowLayoutPanel();
            label4 = new Label();
            cbColumn = new ComboBox();
            label2 = new Label();
            pnlInput = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            btnApply = new Button();
            btnReset = new Button();
            panel1 = new Panel();
            panel3 = new Panel();
            tableLayoutPanel3 = new TableLayoutPanel();
            label1 = new Label();
            pnlPrevios = new Panel();
            pnlNext = new Panel();
            lblTotal = new Label();
            cbRecordsNum = new ComboBox();
            lblPagPage = new Label();
            panel4 = new Panel();
            dgvEquipment = new DataGridView();
            panel2 = new Panel();
            tableLayoutPanel1.SuspendLayout();
            flpSearch.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEquipment).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.White;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Controls.Add(flpSearch, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(1096, 136);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // flpSearch
            // 
            flpSearch.AutoSize = true;
            flpSearch.BackColor = Color.IndianRed;
            flpSearch.Controls.Add(label4);
            flpSearch.Controls.Add(cbColumn);
            flpSearch.Controls.Add(label2);
            flpSearch.Controls.Add(pnlInput);
            flpSearch.Dock = DockStyle.Fill;
            flpSearch.Location = new Point(3, 3);
            flpSearch.Name = "flpSearch";
            flpSearch.Padding = new Padding(10);
            flpSearch.Size = new Size(870, 130);
            flpSearch.TabIndex = 0;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.None;
            label4.AutoSize = true;
            label4.Location = new Point(15, 18);
            label4.Margin = new Padding(5, 5, 20, 0);
            label4.Name = "label4";
            label4.Size = new Size(31, 30);
            label4.TabIndex = 7;
            label4.Text = "In";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbColumn
            // 
            cbColumn.Anchor = AnchorStyles.Left;
            cbColumn.FormattingEnabled = true;
            cbColumn.Location = new Point(66, 15);
            cbColumn.Margin = new Padding(0, 5, 20, 0);
            cbColumn.Name = "cbColumn";
            cbColumn.Size = new Size(292, 36);
            cbColumn.TabIndex = 6;
            cbColumn.SelectedIndexChanged += dropdownColumns_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.Location = new Point(383, 18);
            label2.Margin = new Padding(5, 5, 20, 0);
            label2.Name = "label2";
            label2.Size = new Size(110, 30);
            label2.TabIndex = 9;
            label2.Text = "Search For";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlInput
            // 
            pnlInput.Location = new Point(516, 13);
            pnlInput.Name = "pnlInput";
            pnlInput.Size = new Size(305, 34);
            pnlInput.TabIndex = 8;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(btnApply, 0, 0);
            tableLayoutPanel2.Controls.Add(btnReset, 0, 1);
            tableLayoutPanel2.Location = new Point(896, 20);
            tableLayoutPanel2.Margin = new Padding(20);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(180, 96);
            tableLayoutPanel2.TabIndex = 4;
            // 
            // btnApply
            // 
            btnApply.BackColor = Color.White;
            btnApply.Dock = DockStyle.Fill;
            btnApply.Location = new Point(3, 3);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(174, 42);
            btnApply.TabIndex = 0;
            btnApply.Text = "Apply";
            btnApply.UseVisualStyleBackColor = false;
            btnApply.Click += btnApply_Click;
            // 
            // btnReset
            // 
            btnReset.Dock = DockStyle.Fill;
            btnReset.Location = new Point(3, 51);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(174, 42);
            btnReset.TabIndex = 1;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(20, 20);
            panel1.Name = "panel1";
            panel1.Size = new Size(1096, 136);
            panel1.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(tableLayoutPanel3);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 713);
            panel3.Name = "panel3";
            panel3.Size = new Size(1096, 49);
            panel3.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 7;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            tableLayoutPanel3.Controls.Add(label1, 1, 0);
            tableLayoutPanel3.Controls.Add(pnlPrevios, 4, 0);
            tableLayoutPanel3.Controls.Add(pnlNext, 5, 0);
            tableLayoutPanel3.Controls.Add(lblTotal, 6, 0);
            tableLayoutPanel3.Controls.Add(cbRecordsNum, 2, 0);
            tableLayoutPanel3.Controls.Add(lblPagPage, 3, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(1096, 49);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(349, 0);
            label1.Name = "label1";
            label1.Padding = new Padding(0, 0, 10, 0);
            label1.Size = new Size(94, 49);
            label1.TabIndex = 0;
            label1.Text = "Show";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlPrevios
            // 
            pnlPrevios.Anchor = AnchorStyles.Right;
            pnlPrevios.BackgroundImage = (Image)resources.GetObject("pnlPrevios.BackgroundImage");
            pnlPrevios.BackgroundImageLayout = ImageLayout.Zoom;
            pnlPrevios.Location = new Point(699, 3);
            pnlPrevios.Name = "pnlPrevios";
            pnlPrevios.Size = new Size(94, 43);
            pnlPrevios.TabIndex = 8;
            pnlPrevios.Click += page_toggle_Click;
            // 
            // pnlNext
            // 
            pnlNext.Anchor = AnchorStyles.Left;
            pnlNext.BackgroundImage = (Image)resources.GetObject("pnlNext.BackgroundImage");
            pnlNext.BackgroundImageLayout = ImageLayout.Zoom;
            pnlNext.Location = new Point(799, 3);
            pnlNext.Name = "pnlNext";
            pnlNext.Size = new Size(94, 43);
            pnlNext.TabIndex = 9;
            pnlNext.Click += page_toggle_Click;
            // 
            // lblTotal
            // 
            lblTotal.Anchor = AnchorStyles.Left;
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(899, 9);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(158, 30);
            lblTotal.TabIndex = 10;
            lblTotal.Text = "Total Records: 0";
            lblTotal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbRecordsNum
            // 
            cbRecordsNum.Dock = DockStyle.Fill;
            cbRecordsNum.DropDownStyle = ComboBoxStyle.DropDownList;
            cbRecordsNum.FormattingEnabled = true;
            cbRecordsNum.Location = new Point(446, 5);
            cbRecordsNum.Margin = new Padding(0, 5, 20, 0);
            cbRecordsNum.Name = "cbRecordsNum";
            cbRecordsNum.Size = new Size(80, 36);
            cbRecordsNum.TabIndex = 7;
            cbRecordsNum.SelectedIndexChanged += cbRecordsNum_SelectedIndexChanged;
            // 
            // lblPagPage
            // 
            lblPagPage.AutoSize = true;
            lblPagPage.Dock = DockStyle.Fill;
            lblPagPage.Location = new Point(549, 0);
            lblPagPage.Name = "lblPagPage";
            lblPagPage.Size = new Size(144, 49);
            lblPagPage.TabIndex = 11;
            lblPagPage.Text = "Page 0 of 0";
            lblPagPage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            panel4.Controls.Add(dgvEquipment);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 0);
            panel4.Margin = new Padding(0);
            panel4.Name = "panel4";
            panel4.Padding = new Padding(20);
            panel4.Size = new Size(1096, 713);
            panel4.TabIndex = 1;
            // 
            // dgvEquipment
            // 
            dgvEquipment.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEquipment.Dock = DockStyle.Fill;
            dgvEquipment.Location = new Point(20, 20);
            dgvEquipment.Name = "dgvEquipment";
            dgvEquipment.ReadOnly = true;
            dgvEquipment.RowHeadersWidth = 70;
            dgvEquipment.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEquipment.Size = new Size(1056, 673);
            dgvEquipment.TabIndex = 0;
            dgvEquipment.CellDoubleClick += dgvEquipment_CellDoubleClick;
            // 
            // panel2
            // 
            panel2.Controls.Add(panel4);
            panel2.Controls.Add(panel3);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(20, 156);
            panel2.Name = "panel2";
            panel2.Size = new Size(1096, 762);
            panel2.TabIndex = 1;
            // 
            // admin_inventory
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            BackColor = Color.White;
            Controls.Add(panel2);
            Controls.Add(panel1);
            Margin = new Padding(20);
            Name = "admin_inventory";
            Padding = new Padding(20);
            Size = new Size(1136, 938);
            Load += admin_inventory_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            flpSearch.ResumeLayout(false);
            flpSearch.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvEquipment).EndInit();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private FlowLayoutPanel flpSearch;
        private Label label4;
        private ComboBox cbColumn;
        private TableLayoutPanel tableLayoutPanel2;
        private Button btnApply;
        private Button btnReset;
        private Panel panel1;
        private Panel panel3;
        private Panel panel4;
        private DataGridView dgvEquipment;
        private Panel panel2;
        private Panel pnlInput;
        private TableLayoutPanel tableLayoutPanel3;
        private ComboBox cbRecordsNum;
        private Label label1;
        private Panel pnlPrevios;
        private Panel pnlNext;
        private Label lblTotal;
        private Label lblPagPage;
        private Label label2;
    }
}
