namespace FormsApp.views.controls
{
    partial class NumericFilterControl
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
            tlpFill = new TableLayoutPanel();
            cbOperands = new ComboBox();
            tlpFill.SuspendLayout();
            SuspendLayout();
            // 
            // tlpFill
            // 
            tlpFill.BackColor = Color.White;
            tlpFill.ColumnCount = 5;
            tlpFill.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpFill.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpFill.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpFill.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpFill.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpFill.Controls.Add(cbOperands, 0, 0);
            tlpFill.Dock = DockStyle.Fill;
            tlpFill.Location = new Point(0, 0);
            tlpFill.Name = "tlpFill";
            tlpFill.RowCount = 1;
            tlpFill.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpFill.Size = new Size(684, 55);
            tlpFill.TabIndex = 0;
            // 
            // cbOperands
            // 
            cbOperands.Dock = DockStyle.Fill;
            cbOperands.DropDownStyle = ComboBoxStyle.DropDownList;
            cbOperands.FormattingEnabled = true;
            cbOperands.Location = new Point(3, 3);
            cbOperands.Name = "cbOperands";
            cbOperands.Size = new Size(130, 36);
            cbOperands.TabIndex = 0;
            // 
            // NumericFilterControl
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveBorder;
            Controls.Add(tlpFill);
            MinimumSize = new Size(500, 55);
            Name = "NumericFilterControl";
            Size = new Size(684, 55);
            tlpFill.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpFill;
        private ComboBox cbOperands;
    }
}
