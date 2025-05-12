#region Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Database.Core.Repositories;
using Database.Persistence;
using System.Reflection;
using DotNetEnv;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Database.Interfaces;
#endregion

namespace FormsApp.views.controls
{
    /// <summary>
    /// A control for performing text/status/boolean filtering using a dynamically bound input.
    /// </summary>
    public partial class TextStatusFilterControl : BaseSearchControl
    {
        #region Enum

        /// <summary>
        /// Filter viewType determines which kind of control and logic to use.
        /// </summary>
        public enum FilterType
        {
            String,
            Status,
            Boolean
        }

        #endregion

        #region Static Metadata

        /// <summary>
        /// The name of the repository method this control will invoke during search.
        /// </summary>
        private static readonly string _searchMethodName = "SearchByColumn";

        /// <summary>
        /// The expected parameter types for the search method:
        /// - Property name (string)
        /// - Search value (string)
        /// - Page number (int)
        /// - Page size (int)
        /// - Operator (string)
        /// </summary>
        private static readonly Type[] _searchMethodParams = new[]
        {
            typeof(string),
            typeof(string),
            typeof(int),
            typeof(int),
            typeof(string)
        };

        #endregion


        #region Fields

        /// <summary>
        /// Specifies the type of filter being applied: string, status, or boolean.
        /// Determines which input control is rendered.
        /// </summary>
        private readonly FilterType _selectedType;
        /// <summary>
        /// The actual input control displayed to the user, such as a TextBox or ComboBox,
        /// depending on the selected filter type.
        /// </summary>
        private Control _inputControl;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the control with entity, property name and filter viewType.
        /// </summary>
        public TextStatusFilterControl(Type entityType, string propertyName, FilterType selectedType)
            : base(entityType, propertyName, _searchMethodName, _searchMethodParams)
        {
            InitializeComponent();
            _selectedType = selectedType;
            InitializeControls();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the appropriate input control based on the selected filter viewType.
        /// </summary>
        private void InitializeControls()
        {
            // Clear any existing controls from previous initializations
            this.Controls.Clear();
            this.Dock = DockStyle.Fill;

            // Create a wrapper panel to hold the input control
            Panel wrapper = new Panel { Dock = DockStyle.Fill };

            // Dynamically create the correct control based on filter type
            _inputControl = CreateFilterControl();
            if (_inputControl == null)
                return;

            // Center the control inside the wrapper panel
            _inputControl.Anchor = AnchorStyles.None;
            wrapper.Controls.Add(_inputControl);

            // Adjust position dynamically when the wrapper resizes
            wrapper.Resize += (s, e) =>
            {
                _inputControl.Left = (wrapper.Width - _inputControl.Width) / 2;
                _inputControl.Top = (wrapper.Height - _inputControl.Height) / 2;
            };

            // Load values into the control if filter type requires it (Status)
            if (_selectedType == FilterType.Status && _inputControl is ComboBox combo)
            {
                LoadStatusData();
            }

            // Add the wrapper panel to the main control
            this.Controls.Add(wrapper);
        }

        /// <summary>
        /// Creates and returns a new input control (TextBox or ComboBox) based on the selected filter viewType.
        /// </summary>
        /// <returns>
        /// A <see cref="Control"/> object that matches the current <see cref="FilterType"/>:
        /// - <see cref="FilterType.String"/> returns a <see cref="TextBox"/>
        /// - <see cref="FilterType.Status"/> returns a <see cref="ComboBox"/> in dropdown mode (with no items bound)
        /// - <see cref="FilterType.Boolean"/> returns a <see cref="ComboBox"/> with true/false options
        /// - otherwise returns null
        /// </returns>
        /// <remarks>
        /// This method is typically used to generate input controls dynamically based on the filter configuration
        /// (e.g., in a search/filter panel).
        /// </remarks>
        private Control CreateFilterControl()
        {
            return _selectedType switch
            {
                // Simple text filter using a TextBox
                FilterType.String => new TextBox
                {
                    AutoSize = true,
                    MinimumSize = new Size(250, 30),
                    Margin = new Padding(5)
                },

                // Status dropdown (data to be loaded later)
                FilterType.Status => new ComboBox
                {
                    AutoSize = true,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    MinimumSize = new Size(250, 30),
                    Margin = new Padding(5)
                },

                // Boolean dropdown with hardcoded true/false values
                FilterType.Boolean => new ComboBox
                {
                    DataSource = new BindingSource(new Dictionary<string, bool>
                    {
                        { "True", true },
                        { "False", false }
                    }, null),
                    DisplayMember = "Key",
                    ValueMember = "Value",
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    MinimumSize = new Size(250, 30),
                    Margin = new Padding(5)
                },

                // Unsupported type fallback
                _ => null
            };
        }

        #endregion

        #region Search Execution

        /// <summary>
        /// Triggers search with the given value passed as a string.
        /// </summary>
        private void PerformSearch(object searchValue)
        {
            // Convert the input to string for search query
            string searchQuery = searchValue.ToString() ?? string.Empty;

            // Invoke the base class method to perform the search with necessary arguments
            InvokeSearch(new object[]
            {
                PropertyName, searchQuery, PageNumber, PageSize, ""
            });
        }

        /// <summary>
        /// Loads status values into the ComboBox by calling IStatus repository.
        /// </summary>
        private void LoadStatusData()
        {
            try
            {
                // Ensure the property name is set
                if (string.IsNullOrWhiteSpace(PropertyName))
                    throw new ArgumentException("PropertyName is null or empty. Cannot load status data.");

                // Retrieve the appropriate repository
                var repository = GetRepositoryByEntityProperty();

                // Validate that the repository supports IStatus (has GetAllByName)
                if (repository is not IStatus statusRepository)
                {
                    throw new InvalidCastException(
                        $"Repository for property '{PropertyName}' in entity '{EntityType.Name}' does not implement IStatus.");
                }

                // Fetch status data
                var statusData = statusRepository.GetAllByName();

                // Validate control type before binding
                if (_inputControl is not ComboBox cb)
                    throw new InvalidCastException("The input control is not a ComboBox. Cannot bind status data.");

                // Bind the status data to the combo box
                cb.DataSource = new BindingSource(statusData, null);
                cb.DisplayMember = "Value";
                cb.ValueMember = "Key";
            }
            catch (Exception ex)
            {
                // Show error using global handler
                Global.DisplayReportErrorDialog(ex);
            }
        }

        /// <summary>
        /// Uses reflection to locate a repository related to a navigation property on the entity.
        /// </summary>
        private object GetRepositoryByEntityProperty()
        {
            // Locate the navigation property within the entity that matches the property name
            PropertyInfo entityProperty = EntityType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(p => p.Name.Equals(PropertyName, StringComparison.OrdinalIgnoreCase));

            if (entityProperty == null)
                throw new MissingMemberException($"Property '{PropertyName}' not found on entity viewType '{EntityType.Name}'.");

            // Find the repository within UnitOfWork that matches the property's type
            PropertyInfo repositoryProperty = typeof(UnitOfWork)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(p => p.PropertyType.Name.Contains(entityProperty.PropertyType.Name));

            UnitOfWork _unitOfWork = new UnitOfWork();

            if (repositoryProperty == null)
                throw new MissingMemberException(
                    $"No matching sub-repository found in '{_unitOfWork.GetType().Name}' for viewType '{repositoryProperty?.Name}' (from property '{PropertyName}' in entity '{EntityType.Name}').");

            object? result = repositoryProperty.GetValue(_unitOfWork);

            if (result == null)
                throw new NullReferenceException($"Sub-repository '{repositoryProperty.Name}' is null in '{Repository.GetType().Name}'.");

            return result;
        }

        #endregion

        #region Public API

        /// <summary>
        /// Public method to apply the filter. Calls the apply logic via button handler.
        /// </summary>
        public override void Apply()
        {
            // Determine value from the UI based on filter type
            object searchValue = _selectedType switch
            {
                FilterType.String => (_inputControl as TextBox)?.Text,
                FilterType.Status or FilterType.Boolean => (_inputControl as ComboBox)?.SelectedValue,
                _ => null
            };

            // Perform the search if a value is present
            if (searchValue != null)
            {
                PerformSearch(searchValue);
            }
        }
        #endregion
    }
}
