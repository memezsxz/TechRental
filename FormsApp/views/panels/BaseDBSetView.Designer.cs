namespace FormsApp.views.panels
{
    partial class BaseDBSetView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseDBSetView));
            tableLayoutPanel1 = new TableLayoutPanel();
            gbOptions = new GroupBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            btnEdit = new Button();
            btnAdd = new Button();
            btnDelete = new Button();
            groupBox1 = new GroupBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label3 = new Label();
            cbColumn = new ComboBox();
            label7 = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            btnApply = new Button();
            btnReset = new Button();
            pnlSearch = new Panel();
            panel1 = new Panel();
            panel2 = new Panel();
            panel4 = new Panel();
            dvgItems = new DataGridView();
            panel3 = new Panel();
            tableLayoutPanel3 = new TableLayoutPanel();
            label1 = new Label();
            pnlPrevios = new Panel();
            pnlNext = new Panel();
            lblTotal = new Label();
            cbRecordsNum = new ComboBox();
            lblPagPage = new Label();
            tableLayoutPanel1.SuspendLayout();
            gbOptions.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dvgItems).BeginInit();
            panel3.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.White;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68.06115F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31.93886F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(gbOptions, 1, 0);
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1307, 161);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // gbOptions
            // 
            gbOptions.Controls.Add(tableLayoutPanel4);
            gbOptions.Dock = DockStyle.Fill;
            gbOptions.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            gbOptions.Location = new Point(892, 3);
            gbOptions.Name = "gbOptions";
            gbOptions.Size = new Size(412, 155);
            gbOptions.TabIndex = 5;
            gbOptions.TabStop = false;
            gbOptions.Text = "Options";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(btnEdit, 1, 0);
            tableLayoutPanel4.Controls.Add(btnAdd, 0, 0);
            tableLayoutPanel4.Controls.Add(btnDelete, 0, 1);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 34);
            tableLayoutPanel4.Margin = new Padding(5);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.Padding = new Padding(5);
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Size = new Size(406, 118);
            tableLayoutPanel4.TabIndex = 6;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.White;
            btnEdit.Dock = DockStyle.Fill;
            btnEdit.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnEdit.Location = new Point(206, 8);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(192, 48);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.White;
            btnAdd.Dock = DockStyle.Fill;
            btnAdd.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnAdd.Location = new Point(8, 8);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(192, 48);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.White;
            tableLayoutPanel4.SetColumnSpan(btnDelete, 2);
            btnDelete.Dock = DockStyle.Fill;
            btnDelete.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnDelete.ForeColor = Color.Red;
            btnDelete.Location = new Point(8, 62);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(390, 48);
            btnDelete.TabIndex = 1;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            btnDelete.Paint += btnDelete_Paint;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel5);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(883, 155);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "Filters";
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel5.Controls.Add(flowLayoutPanel1, 0, 0);
            tableLayoutPanel5.Controls.Add(tableLayoutPanel2, 1, 0);
            tableLayoutPanel5.Controls.Add(pnlSearch, 0, 1);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 34);
            tableLayoutPanel5.Margin = new Padding(5);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.Padding = new Padding(5);
            tableLayoutPanel5.RowCount = 2;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Size = new Size(877, 118);
            tableLayoutPanel5.TabIndex = 7;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.BackColor = Color.White;
            flowLayoutPanel1.Controls.Add(label3);
            flowLayoutPanel1.Controls.Add(cbColumn);
            flowLayoutPanel1.Controls.Add(label7);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(8, 8);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(687, 48);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(5, 5);
            label3.Margin = new Padding(5, 5, 20, 0);
            label3.Name = "label3";
            label3.Size = new Size(33, 31);
            label3.TabIndex = 7;
            label3.Text = "In";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbColumn
            // 
            cbColumn.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            cbColumn.FormattingEnabled = true;
            cbColumn.Location = new Point(58, 5);
            cbColumn.Margin = new Padding(0, 5, 20, 0);
            cbColumn.Name = "cbColumn";
            cbColumn.Size = new Size(263, 39);
            cbColumn.TabIndex = 6;
            cbColumn.SelectedIndexChanged += dropdownColumns_SelectedIndexChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label7.Location = new Point(346, 5);
            label7.Margin = new Padding(5, 5, 20, 0);
            label7.Name = "label7";
            label7.Size = new Size(120, 31);
            label7.TabIndex = 9;
            label7.Text = "Search For";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(btnApply, 0, 0);
            tableLayoutPanel2.Controls.Add(btnReset, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(703, 10);
            tableLayoutPanel2.Margin = new Padding(5);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel5.SetRowSpan(tableLayoutPanel2, 2);
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(164, 98);
            tableLayoutPanel2.TabIndex = 4;
            // 
            // btnApply
            // 
            btnApply.BackColor = Color.White;
            btnApply.Dock = DockStyle.Fill;
            btnApply.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnApply.Location = new Point(3, 3);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(158, 43);
            btnApply.TabIndex = 0;
            btnApply.Text = "Apply";
            btnApply.UseVisualStyleBackColor = false;
            btnApply.Click += btnApply_Click;
            // 
            // btnReset
            // 
            btnReset.Dock = DockStyle.Fill;
            btnReset.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            btnReset.Location = new Point(3, 52);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(158, 43);
            btnReset.TabIndex = 1;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // pnlSearch
            // 
            pnlSearch.Dock = DockStyle.Fill;
            pnlSearch.Location = new Point(8, 62);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Size = new Size(687, 48);
            pnlSearch.TabIndex = 10;
            // 
            // panel1
            // 
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(20, 20);
            panel1.Name = "panel1";
            panel1.Size = new Size(1307, 161);
            panel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(panel4);
            panel2.Controls.Add(panel3);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(20, 181);
            panel2.Name = "panel2";
            panel2.Size = new Size(1307, 770);
            panel2.TabIndex = 2;
            // 
            // panel4
            // 
            panel4.Controls.Add(dvgItems);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 0);
            panel4.Margin = new Padding(0);
            panel4.Name = "panel4";
            panel4.Padding = new Padding(20);
            panel4.Size = new Size(1307, 721);
            panel4.TabIndex = 1;
            // 
            // dvgItems
            // 
            dvgItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dvgItems.Dock = DockStyle.Fill;
            dvgItems.Location = new Point(20, 20);
            dvgItems.Name = "dvgItems";
            dvgItems.ReadOnly = true;
            dvgItems.RowHeadersWidth = 70;
            dvgItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dvgItems.Size = new Size(1267, 681);
            dvgItems.TabIndex = 0;
            dvgItems.DataBindingComplete += dgvData_DataBindingComplete;
            // 
            // panel3
            // 
            panel3.Controls.Add(tableLayoutPanel3);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 721);
            panel3.Name = "panel3";
            panel3.Size = new Size(1307, 49);
            panel3.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 7;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
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
            tableLayoutPanel3.Size = new Size(1307, 49);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(460, 0);
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
            pnlPrevios.Location = new Point(860, 3);
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
            pnlNext.Location = new Point(960, 3);
            pnlNext.Name = "pnlNext";
            pnlNext.Size = new Size(94, 43);
            pnlNext.TabIndex = 9;
            pnlNext.Click += page_toggle_Click;
            // 
            // lblTotal
            // 
            lblTotal.Anchor = AnchorStyles.Left;
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblTotal.Location = new Point(1060, 9);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(173, 31);
            lblTotal.TabIndex = 10;
            lblTotal.Text = "Total Records: 0";
            lblTotal.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbRecordsNum
            // 
            cbRecordsNum.Dock = DockStyle.Fill;
            cbRecordsNum.DropDownStyle = ComboBoxStyle.DropDownList;
            cbRecordsNum.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            cbRecordsNum.FormattingEnabled = true;
            cbRecordsNum.Location = new Point(557, 5);
            cbRecordsNum.Margin = new Padding(0, 5, 20, 0);
            cbRecordsNum.Name = "cbRecordsNum";
            cbRecordsNum.Size = new Size(80, 39);
            cbRecordsNum.TabIndex = 7;
            cbRecordsNum.SelectedIndexChanged += cbRecordsNum_SelectedIndexChanged;
            // 
            // lblPagPage
            // 
            lblPagPage.AutoSize = true;
            lblPagPage.Dock = DockStyle.Fill;
            lblPagPage.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblPagPage.Location = new Point(660, 0);
            lblPagPage.Name = "lblPagPage";
            lblPagPage.Size = new Size(194, 49);
            lblPagPage.TabIndex = 11;
            lblPagPage.Text = "Page 0 of 0";
            lblPagPage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // BaseDBSetView
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            BackColor = Color.White;
            Controls.Add(panel2);
            Controls.Add(panel1);
            Margin = new Padding(20);
            Name = "BaseDBSetView";
            Padding = new Padding(20);
            Size = new Size(1347, 971);
            Load += admin_inventory_Load;
            tableLayoutPanel1.ResumeLayout(false);
            gbOptions.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dvgItems).EndInit();
            panel3.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Button btnApply;
        private Button btnReset;
        private Panel panel1;
        private GroupBox gbOptions;
        private TableLayoutPanel tableLayoutPanel4;
        private Button btnEdit;
        private Button btnAdd;
        private Button btnDelete;
        private ComboBox cbColumn;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel5;
        private Label label7;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label3;
        private Panel panel2;
        private Panel panel4;
        private DataGridView dvgItems;
        private Panel panel3;
        private TableLayoutPanel tableLayoutPanel3;
        private Label label1;
        private Panel pnlPrevios;
        private Panel pnlNext;
        private Label lblTotal;
        private ComboBox cbRecordsNum;
        private Label lblPagPage;
        private Panel pnlSearch;
    }
}
