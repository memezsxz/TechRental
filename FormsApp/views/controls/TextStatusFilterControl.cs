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
using Database;
using DotNetEnv;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        /// Filter type determines which kind of control and logic to use.
        /// </summary>
        public enum FilterType
        {
            String,
            Status,
            Boolean
        }

        #endregion

        #region Static Metadata

        private static readonly string _searchMethodName = "SearchByColumn";

        private static readonly Type[] _searchMethodParams = new[]
            { typeof(string), typeof(string), typeof(int), typeof(int), typeof(string) };

        #endregion

        #region Fields

        private readonly FilterType _selectedType;
        private Control _inputControl;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the control with entity, property name and filter type.
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
        /// Initializes the appropriate input control based on the selected filter type.
        /// </summary>
        private void InitializeControls()
        {
            this.Controls.Clear();
            this.Dock = DockStyle.Fill;

            Panel wrapper = new Panel { Dock = DockStyle.Fill };

            // Dynamically create the input control based on type
            _inputControl = CreateFilterControl();

            if (_inputControl == null)
                return;

            _inputControl.Anchor = AnchorStyles.None;
            wrapper.Controls.Add(_inputControl);

            // Center the control within the panel when resized
            wrapper.Resize += (s, e) =>
            {
                _inputControl.Left = (wrapper.Width - _inputControl.Width) / 2;
                _inputControl.Top = (wrapper.Height - _inputControl.Height) / 2;
            };

            // Load data if needed
            if (_selectedType == FilterType.Status && _inputControl is ComboBox combo)
            {
                LoadStatusData();
            }

            this.Controls.Add(wrapper);
        }

        /// <summary>
        /// Creates and returns a new input control (TextBox or ComboBox) based on the selected filter type.
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
          return  _selectedType switch
            {
                // For string input: return a plain TextBox
                FilterType.String => new TextBox
                {
                    AutoSize = true,
                    MinimumSize = new Size(250, 30),
                    Margin = new Padding(5)
                },

                // For status selections: return a ComboBox (items to be bound later)
                FilterType.Status => new ComboBox
                {
                    AutoSize = true,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    MinimumSize = new Size(250, 30),
                    Margin = new Padding(5)
                },

                // For true/false input: return a ComboBox with hardcoded boolean options
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
            string searchQuery = searchValue.ToString() ?? string.Empty;
            InvokeSearch(new object[] { PropertyName, searchQuery, PageNumber, PageSize, "" });
        }

        /// <summary>
        /// Loads status values into the ComboBox by calling IStatus repository.
        /// </summary>
        private void LoadStatusData()
        {
            try
            {
                // Validate
                if (string.IsNullOrWhiteSpace(PropertyName))
                    throw new ArgumentException("PropertyName is null or empty. Cannot load status data.");

                var repository = GetRepositoryByEntityProperty();

                // Cast check
                if (repository is not IStatus statusRepository)
                {
                    throw new InvalidCastException(
                        $"Repository for property '{PropertyName}' in entity '{EntityType.Name}' does not implement IStatus.");
                }

                // Get values and bind
                var statusData = statusRepository.GetAllByName();

                if (_inputControl is not ComboBox cb)
                    throw new InvalidCastException("The input control is not a ComboBox. Cannot bind status data.");

                cb.DataSource = new BindingSource(statusData, null);
                cb.DisplayMember = "Value";
                cb.ValueMember = "Key";
            }
            catch (Exception ex)
            {
                Global.DisplayReportErrorDialog(ex);
            }
        }

        /// <summary>
        /// Uses reflection to locate a repository related to a navigation property on the entity.
        /// </summary>
        private object GetRepositoryByEntityProperty()
        {
            // Locate navigation property
            PropertyInfo entityProperty = EntityType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(p => p.Name.Equals(PropertyName, StringComparison.OrdinalIgnoreCase));

            if (entityProperty == null)
                throw new MissingMemberException($"Property '{PropertyName}' not found on entity type '{EntityType.Name}'.");

            // Match repository by property type
            PropertyInfo repositoryProperty = typeof(UnitOfWork)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(p => p.PropertyType.Name.Contains(entityProperty.PropertyType.Name));

            UnitOfWork _unitOfWork = new UnitOfWork(new RentalDBContext());

            if (repositoryProperty == null)
                throw new MissingMemberException(
                    $"No matching sub-repository found in '{_unitOfWork.GetType().Name}' for type '{repositoryProperty.Name}' " +
                    $"(from property '{PropertyName}' in entity '{EntityType.Name}').");

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
            // Resolve value from input control based on type
            object searchValue = _selectedType switch
            {
                FilterType.String => (_inputControl as TextBox)?.Text,
                FilterType.Status or FilterType.Boolean => (_inputControl as ComboBox)?.SelectedValue,
                _ => null
            };

            if (searchValue != null)
            {
                PerformSearch(searchValue);
            }
        }

        #endregion
    }
}
