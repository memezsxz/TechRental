using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Database.Persistence;
using System.Reflection;

namespace FormsApp.views.controls
{
    public partial class NumericFilterControl : UserControl, ISearch
    {
        private readonly Type _propertyType;
        private readonly string _propertyName;
        private TextBox _inputValue1;
        private TextBox _inputValue2;
        private Button _applyButton;
        private Label _andLabel; 

        public event Action<object> OnSearchCompleted;

        public NumericFilterControl(Type propertyType, string propertyName)
        {
            InitializeComponent();
            _propertyType = propertyType;
            _propertyName = propertyName;
            InitializeControls();
        }

        private void InitializeControls()
        {
            this.Dock = DockStyle.Fill;

            // Set column styles to distribute space properly
            tlpFill.ColumnStyles.Clear();
            tlpFill.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));   // Operator dropdown
            tlpFill.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));    // First input
            tlpFill.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));       // "AND" Label
            tlpFill.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));    // Second input (if "between" selected)
            tlpFill.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));       // Apply button

            cbOperands.Items.AddRange(new object[] { "==", ">=", "<=", ">", "<", "between" });
            cbOperands.SelectedIndexChanged += OperatorChanged;
            // First Input Field
            _inputValue1 = new TextBox
            {
                Width = 120,
                Margin = new Padding(5),
                Anchor = AnchorStyles.Left | AnchorStyles.Right
            };


            // "AND" Label (Initially hidden)
            _andLabel = new Label
            {
                Text = "and",
                AutoSize = true,
                Margin = new Padding(5),
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };


            // Second Input Field (For "between" option)
            _inputValue2 = new TextBox
            {
                Width = 120,
                Margin = new Padding(5),
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Visible = false
            };

            // Apply Button
            _applyButton = new Button
            {
                Text = "Apply",
                AutoSize = true,
                Padding = new Padding(5),
                Margin = new Padding(5)
            };
            _applyButton.Click += BtnApply_Click;

            // Add controls to TableLayoutPanel at specific positions
            tlpFill.Controls.Add(_inputValue1, 1, 0);      // First Input Box
            tlpFill.Controls.Add(_andLabel, 2, 0);         // "AND" Label (Initially hidden)
            tlpFill.Controls.Add(_inputValue2, 3, 0);      // Second Input Box (Initially hidden)
            tlpFill.Controls.Add(_applyButton, 4, 0);      // Apply Button
        }



        private void OperatorChanged(object sender, EventArgs e)
        {
            bool isBetween = cbOperands.SelectedItem?.ToString() == "between";

            _inputValue2.Visible = isBetween;
            _andLabel.Visible = isBetween;

            // If "between" is not selected, clear the second input
            if (!isBetween)
            {
                _inputValue2.Text = string.Empty;
            }

        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            string selectedOperator = cbOperands.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedOperator)) return;

            string value1 = _inputValue1.Text;
            string value2 = _inputValue2.Text;

            PerformSearch(selectedOperator, value1, value2);
        }

        private void PerformSearch(string selectedOperator, string value1, string value2)
        {
            var repository = new UnitOfWork(new RentalDBContext()).Equipment;
            if (repository == null) return;

            MethodInfo searchMethod = repository.GetType().GetMethod("SearchByColumnOperation");
            if (searchMethod == null) return;

            // Ensure that "between" passes two values, otherwise use value1 only
            string searchQuery = selectedOperator == "between" ? $"{value1},{value2}" : value1;

            var result = searchMethod.Invoke(repository, new object[] { _propertyName, searchQuery, 1, 10, selectedOperator });

            OnSearchCompleted?.Invoke(result);
        }
    }
}
