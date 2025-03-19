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
using Microsoft.VisualBasic;

namespace FormsApp.views.controls
{
    public partial class NumericFilterControl : UserControl, ISearch
    {
        private readonly Type _propertyType;
        private readonly Type _colType;
        private readonly string _propertyName;
        private Control _inputValue1;
        private Control _inputValue2;
        private Button _applyButton;
        private Label _andLabel;

        public event Action<object> OnSearchCompleted;

        public NumericFilterControl(Type propertyType, string propertyName, Type colType)
        {
            InitializeComponent();
            _propertyType = propertyType;
            _propertyName = propertyName;
            _colType = colType;
            InitializeControls();
        }

        private void InitializeControls()
        {
            this.Dock = DockStyle.Fill;
            if (_propertyName == "Id") cbOperands.Visible = false;

            // Set column styles to distribute space properly
            tlpFill.ColumnStyles.Clear();
            tlpFill.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));   // Operator dropdown
            tlpFill.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));    // First input
            tlpFill.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));   // "AND" Label
            tlpFill.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));    // Second input (if "between" selected)
            tlpFill.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));       // Apply button

            cbOperands.Items.AddRange(new object[] { "==", ">=", "<=", ">", "<", "between" });
            cbOperands.SelectedIndexChanged += OperatorChanged;

            // Create input fields dynamically based on type
            if (_colType == typeof(DateTime))
            {
                _inputValue1 = new DateTimePicker { Width = 120, Margin = new Padding(5), Format = DateTimePickerFormat.Short };
                _inputValue2 = new DateTimePicker { Width = 120, Margin = new Padding(5), Format = DateTimePickerFormat.Short, Visible = false };
            }
            else if (_colType == typeof(int) || _colType == typeof(double) || _colType == typeof(decimal))
            {
                _inputValue1 = new NumericUpDown { Width = 120, Margin = new Padding(5), DecimalPlaces = _colType == typeof(int) ? 0 : 2, Minimum = 0, Maximum = (_colType == typeof(int) ? 10000 : 10000.0m) };
                _inputValue2 = new NumericUpDown { Width = 120, Margin = new Padding(5), DecimalPlaces = _colType == typeof(int) ? 0 : 2, Minimum = 0, Maximum = (_colType == typeof(int) ? 10000 : 10000.0m), Visible = false };
                _inputValue1.KeyPress += NumericInput_KeyPress;
                _inputValue2.KeyPress += NumericInput_KeyPress;
            }
            else
            {
                _inputValue1 = new TextBox { Width = 120, Margin = new Padding(5) };
                _inputValue2 = new TextBox { Width = 120, Margin = new Padding(5), Visible = false };
            }

            _inputValue1.Dock = DockStyle.Fill;
            _inputValue2.Dock = DockStyle.Fill;

            // "AND" Label (Initially hidden)
            _andLabel = new Label
            {
                Text = "and",
                AutoSize = true,
                Margin = new Padding(5),
                TextAlign = ContentAlignment.MiddleCenter,
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

            // Add controls to TableLayoutPanel
            tlpFill.Controls.Add(_inputValue1, 1, 0);      // First Input
            tlpFill.Controls.Add(_andLabel, 2, 0);        // "AND" Label
            tlpFill.Controls.Add(_inputValue2, 3, 0);      // Second Input
            tlpFill.Controls.Add(_applyButton, 4, 0);      // Apply Button
            cbOperands.SelectedIndex = 0;
        }



        private void OperatorChanged(object sender, EventArgs e)
        {
            bool isBetween = cbOperands.SelectedItem?.ToString() == "between";

            _inputValue2.Visible = isBetween;
            _andLabel.Visible = isBetween;

            if (!isBetween)
            {
                if (_inputValue2 is TextBox txt) txt.Text = string.Empty;
                if (_inputValue2 is NumericUpDown num) num.Value = 0;
                if (_inputValue2 is DateTimePicker dt) dt.Value = DateTime.Now;
            }
        }


        private void BtnApply_Click(object sender, EventArgs e)
        {
            string selectedOperator = cbOperands.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedOperator)) return;

            object value1 = GetInputValue(_inputValue1);
            object value2 = GetInputValue(_inputValue2);

            PerformSearch(selectedOperator, value1, value2);
        }

        private object GetInputValue(Control input)
        {
            if (input is TextBox txt) return txt.Text;
            if (input is NumericUpDown num) return num.Value;
            if (input is DateTimePicker dt) return dt.Value.Date;
            return null;
        }

        private void PerformSearch(string selectedOperator, object value1, object value2)
        {
            var repository = new UnitOfWork(new RentalDBContext()).Equipment;
            if (repository == null) return;

            MethodInfo searchMethod = repository.GetType().GetMethod("SearchByColumnOperation");
            if (searchMethod == null) return;

            string searchQuery;
            if (_colType == typeof(DateTime))
            {
                string formattedDate1 = ((DateTime)value1).ToString("yyyy-MM-dd");
                string formattedDate2 = value2 != null ? ((DateTime)value2).ToString("yyyy-MM-dd") : "";

                searchQuery = selectedOperator == "between" ? $"{formattedDate1},{formattedDate2}" : formattedDate1;
            }
            else
            {
                searchQuery = selectedOperator == "between" ? $"{value1},{value2}" : value1.ToString();
            }

            var result = searchMethod.Invoke(repository, new object[] { _propertyName, searchQuery, 1, 10, selectedOperator });

            OnSearchCompleted?.Invoke(result);
        }

        private void NumericInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            NumericUpDown numBox = sender as NumericUpDown;
            if (numBox == null) return;

            char decimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];

            // Allow digits, backspace, and a single decimal point
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != decimalSeparator)
            {
                e.Handled = true; // Block invalid characters
            }

            // Prevent multiple decimal points
            if (e.KeyChar == decimalSeparator && numBox.Text.Contains(decimalSeparator))
            {
                e.Handled = true;
            }
        }

    }
}
