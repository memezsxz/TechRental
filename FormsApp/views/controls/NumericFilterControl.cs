#region Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Database;
using Database.Persistence;
using Microsoft.VisualBasic;
#endregion

namespace FormsApp.views.controls;

/// <summary>
/// A dynamic filter control used for numeric or date-based filtering using an operator and up to two inputs.
/// </summary>
//public partial class NumericFilterControl : UserControl
public partial class NumericFilterControl : BaseSearchControl
{
    #region Fields

    /// <summary>
    /// The underlying data type of the column (e.g., int, decimal, DateTime).
    /// Determines which input controls are used.
    /// </summary>
    private readonly Type _colType;

    /// <summary>
    /// First input control for the value to compare.
    /// </summary>
    private Control _inputValue1;

    /// <summary>
    /// Second input control for "between" operations.
    /// </summary>
    private Control _inputValue2;

    /// <summary>
    /// Label used between inputs when using "between" as the operator.
    /// </summary>
    private Label _andLabel;

    /// <summary>
    /// The repository method name to call for filtered queries.
    /// </summary>
    private static readonly string _searchMethodName = "SearchByColumn";

    /// <summary>
    /// The expected parameter types for the search method.
    /// </summary>
    private static readonly Type[] _searchMethodParams = new[]
        { typeof(string), typeof(string), typeof(int), typeof(int), typeof(string) };

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="NumericFilterControl"/> class.
    /// </summary>
    public NumericFilterControl(Type entityType, string propertyName, Type colType)
        : base(entityType, propertyName, _searchMethodName, _searchMethodParams)
    {
        InitializeComponent();
        _colType = colType;
        InitializeControls();
    }

    #endregion

    #region Initialization

    /// <summary>
    /// Initializes the layout, operator dropdown, and filter controls.
    /// </summary>
    private void InitializeControls()
    {
        // Dock the control to fill its container
        this.Dock = DockStyle.Fill;

        // Hide operands if the property is "Id", as filtering typically isn't needed
        if (PropertyName == "Id")
            cbOperands.Visible = false;

        // Clear and configure layout columns
        tlpFill.ColumnStyles.Clear();
        tlpFill.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120)); // Operand dropdown
        tlpFill.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));   // First input field
        tlpFill.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));  // AND label
        tlpFill.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));   // Second input field

        // Populate the operand dropdown with supported operators
        cbOperands.Items.AddRange(new object[] { "==", ">=", "<=", ">", "<", "between" });
        cbOperands.SelectedIndexChanged += OperatorChanged;

        // Dynamically create the appropriate input controls for the column type
        CreateFilterControl();

        // Dock both input fields
        _inputValue1.Dock = DockStyle.Fill;
        _inputValue2.Dock = DockStyle.Fill;

        // Initialize the AND label for "between" operator
        _andLabel = new Label
        {
            Text = "and",
            AutoSize = true,
            Margin = new Padding(5),
            TextAlign = ContentAlignment.MiddleCenter,
            Visible = false // Initially hidden unless "between" is selected
        };

        // Add all components to the layout panel
        tlpFill.Controls.Add(_inputValue1, 1, 0);
        tlpFill.Controls.Add(_andLabel, 2, 0);
        tlpFill.Controls.Add(_inputValue2, 3, 0);

        // Set default selected operator
        cbOperands.SelectedIndex = 0;
    }

    /// <summary>
    /// Dynamically creates input controls based on the column viewType (DateTime, numeric, or text).
    /// </summary>
    private void CreateFilterControl()
    {
        // If the column is a DateTime, use DateTimePickers
        if (_colType == typeof(DateTime))
        {
            _inputValue1 = new DateTimePicker
            {
                Width = 120,
                Margin = new Padding(5),
                Format = DateTimePickerFormat.Short
            };

            _inputValue2 = new DateTimePicker
            {
                Width = 120,
                Margin = new Padding(5),
                Format = DateTimePickerFormat.Short,
                Visible = false
            };
        }
        // If numeric (int, decimal, etc.), use NumericUpDowns
        else if (_colType == typeof(int) || _colType == typeof(double) || _colType == typeof(decimal))
        {
            _inputValue1 = new NumericUpDown
            {
                Width = 120,
                Margin = new Padding(5),
                DecimalPlaces = _colType == typeof(int) ? 0 : 2,
                Minimum = 0,
                Maximum = _colType == typeof(int) ? 10000 : 10000.0m
            };

            _inputValue2 = new NumericUpDown
            {
                Width = 120,
                Margin = new Padding(5),
                DecimalPlaces = _colType == typeof(int) ? 0 : 2,
                Minimum = 0,
                Maximum = _colType == typeof(int) ? 10000 : 10000.0m,
                Visible = false
            };

            // Prevent invalid characters in numeric input
            _inputValue1.KeyPress += NumericInput_KeyPress;
            _inputValue2.KeyPress += NumericInput_KeyPress;
        }
        // Default fallback: use TextBox
        else
        {
            _inputValue1 = new TextBox { Width = 120, Margin = new Padding(5) };
            _inputValue2 = new TextBox { Width = 120, Margin = new Padding(5), Visible = false };
        }
    }

    #endregion

    #region Search Logic

    /// <summary>
    /// Retrieves the current value from a given input control.
    /// </summary>
    private object GetInputValue(Control input)
    {
        return input switch
        {
            TextBox txt => txt.Text,
            NumericUpDown num => num.Value,
            DateTimePicker dt => dt.Value.Date,
            _ => null
        };
    }

    /// <summary>
    /// Executes the search based on the selected operator and input values.
    /// </summary>
    private void PerformSearch(string selectedOperator, object value1, object value2)
    {
        string searchQuery;

        // Format input as a string suitable for the query
        if (_colType == typeof(DateTime))
        {
            string formattedDate1 = ((DateTime)value1).ToString("yyyy-MM-dd");
            string formattedDate2 = value2 != null ? ((DateTime)value2).ToString("yyyy-MM-dd") : "";

            // If using "between", combine both dates
            searchQuery = selectedOperator == "between"
                ? $"{formattedDate1},{formattedDate2}"
                : formattedDate1;
        }
        else
        {
            // Same idea for numbers or text
            searchQuery = selectedOperator == "between"
                ? $"{value1},{value2}"
                : value1?.ToString();
        }

        // Call base class's InvokeSearch method with resolved values
        InvokeSearch(new object[]
        {
            PropertyName, searchQuery, PageNumber, PageSize, selectedOperator
        });
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Event handler to show/hide second input field based on operator selection.
    /// </summary>
    private void OperatorChanged(object sender, EventArgs e)
    {
        // Check if selected operator is "between"
        bool isBetween = cbOperands.SelectedItem?.ToString() == "between";

        // Show or hide second input accordingly
        _inputValue2.Visible = isBetween;
        _andLabel.Visible = isBetween;

        // If switching away from "between", reset second input field
        if (!isBetween)
        {
            if (_inputValue2 is TextBox txt) txt.Text = string.Empty;
            if (_inputValue2 is NumericUpDown num) num.Value = 0;
            if (_inputValue2 is DateTimePicker dt) dt.Value = DateTime.Now;
        }
    }

    /// <summary>
    /// Prevents invalid characters in numeric input fields (e.g. multiple decimals).
    /// </summary>
    private void NumericInput_KeyPress(object sender, KeyPressEventArgs e)
    {
        NumericUpDown numBox = sender as NumericUpDown;
        if (numBox == null) return;

        // Get the current culture's decimal separator (usually '.' or ',')
        char decimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];

        // Block any character that is not a digit, control key, or the decimal separator
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != decimalSeparator)
            e.Handled = true;

        // Prevent entering more than one decimal separator
        if (e.KeyChar == decimalSeparator && numBox.Text.Contains(decimalSeparator))
            e.Handled = true;
    }

    #endregion

    #region Public API

    /// <summary>
    /// Public method to initiate the search process.
    /// </summary>
    public override void Apply()
    {
        // Read selected operator
        string selectedOperator = cbOperands.SelectedItem?.ToString();
        if (string.IsNullOrEmpty(selectedOperator)) return;

        // Read values from both input controls
        object value1 = GetInputValue(_inputValue1);
        object value2 = GetInputValue(_inputValue2);

        // Execute the search with the current input and operator
        PerformSearch(selectedOperator, value1, value2);
    }

    #endregion
}
