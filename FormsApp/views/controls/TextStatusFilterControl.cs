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

namespace FormsApp.views.controls
{
    public partial class TextStatusFilterControl : UserControl, ISearch
    {
        public enum FilterType
        {
            String,
            Status,
            Boolean
        }

        private int _pageNumber = 1;
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? 10 : value; 
        }


        private readonly UnitOfWork _unitOfWork = new UnitOfWork(new RentalDBContext());
        private readonly FilterType _selectedType; // Now an Enum!
        private readonly Type _entityType;
        private readonly string _propertyName;

        private ComboBox _statusDropdown;
        private ComboBox _booleanDropdown;
        private TextBox _textBox;

        public event Action<object> OnSearchCompleted;

        public TextStatusFilterControl(Type entityType, FilterType selectedType, string propertyName)
        {
            InitializeComponent();
            _entityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
            _selectedType = selectedType; // Enum prevents null values
            _propertyName = propertyName;

            InitializeControls();
        }

        private void InitializeControls()
        {
            this.Controls.Clear();
            this.Dock = DockStyle.Fill;

            if (_selectedType == FilterType.String)
            {
                _textBox = new TextBox
                {
                    Dock = DockStyle.Fill
                };
                this.Controls.Add(_textBox);
            }
            else if (_selectedType == FilterType.Status)
            {
                _statusDropdown = new ComboBox
                {
                    Dock = DockStyle.Fill,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                LoadStatusData();
                this.Controls.Add(_statusDropdown);
            }
            else if (_selectedType == FilterType.Boolean)
            {
                _booleanDropdown = new ComboBox
                {
                    Dock = DockStyle.Fill,
                    DataSource =
                        new BindingSource(new Dictionary<string, bool> { { "True", true }, { "False", false } },
                            null),
                    DisplayMember = "Key",
                    ValueMember = "Value",

                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                this.Controls.Add(_booleanDropdown);
            }

            var btnApply = new Button
            {
                Text = "Apply",
                Dock = DockStyle.Right
            };

            btnApply.Click += BtnApply_Click;
            this.Controls.Add(btnApply);
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            object searchValue = _selectedType switch
            {
                FilterType.String => _textBox?.Text,
                FilterType.Status => _statusDropdown?.SelectedValue,
                FilterType.Boolean => _booleanDropdown?.SelectedValue,
                _ => null
            };

            if (searchValue != null)
            {
                PerformSearch(searchValue);
            }
        }

        private void PerformSearch(object searchValue)
        {
            Console.WriteLine("testing in preform search");
            var repository = GetRepositoryByEntityType(); 

            if (repository == null)
            {
                Console.WriteLine($"No repository found for entity 'Equipment'.");
                return;
            }

            // Convert navigation property (e.g., "AvailabilityStatus") to foreign key column ("AvailabilityStatusId")
            string columnToSearch = _selectedType switch
            {
                FilterType.String => _propertyName,
                FilterType.Boolean => _propertyName,
                FilterType.Status => _propertyName + "Id",
                _ => throw new InvalidOperationException("Unknown filter type.")
            };

            // Ensure repository supports filtering
            MethodInfo searchMethod = repository.GetType().GetMethod("SearchByColumn");
            if (searchMethod == null)
            {
                Console.WriteLine($"Repository does not support searching by '{columnToSearch}'.");
                return;
            }

            // Convert searchValue to string (SearchByColumn expects a string)
            string searchString = searchValue.ToString();

            Console.WriteLine(columnToSearch);
            Console.WriteLine(searchString);

            // Invoke the search method dynamically with pagination parameters
            var result = searchMethod.Invoke(repository, new object[] { columnToSearch, searchString, _pageNumber, _pageSize });

            // Notify the main form with the filtered results
            OnSearchCompleted?.Invoke(result);
        }

        private object GetRepositoryByEntityType()
        {
            var unitOfWorkType = typeof(UnitOfWork);
            var properties = unitOfWorkType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var repoInstance = prop.GetValue(_unitOfWork);
                if (repoInstance == null) continue;

                var interfaces = repoInstance.GetType().GetInterfaces();

                foreach (var iface in interfaces)
                {
                    if (!iface.IsGenericType) continue;

                    var genericDef = iface.GetGenericTypeDefinition();
                    var genericArgs = iface.GetGenericArguments();

                    // Match generic interface IRepository<T> or any known IRepository-like interface
                    if ((genericDef == typeof(IRepository<>)
                         || genericDef.Name.Contains("Repository")) // Catch custom interfaces like IEquipmentRepository
                        && genericArgs.Length == 1
                        && genericArgs[0] == _entityType)
                    {
                        return repoInstance;
                    }
                }
            }

            Console.WriteLine($"No repository found in UnitOfWork for entity type {_entityType.Name}.");
            return null;
        }

        private void LoadStatusData()
        {
            if (string.IsNullOrEmpty(_propertyName))
            {
                Console.WriteLine("Property name is null or empty.");
                return;
            }

            var repository = GetRepositoryByEntityProperty();

            if (repository is IStatus statusRepository)
            {
                var statusData = statusRepository.GetAllByName();
                if (statusData != null && statusData.Any())
                {
                    _statusDropdown.DataSource = new BindingSource(statusData, null);
                    _statusDropdown.DisplayMember = "Value";
                    _statusDropdown.ValueMember = "Key";
                }
                else
                {
                    Console.WriteLine($"No status data found for '{_propertyName}'.");
                }
            }
            else
            {
                Console.WriteLine($"Repository for property '{_propertyName}' not found or does not implement IStatus.");
            }
        }

        private object GetRepositoryByEntityProperty()
        {
            PropertyInfo entityProperty = _entityType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(p => p.Name.Equals(_propertyName, StringComparison.OrdinalIgnoreCase));

            if (entityProperty == null)
            {
                Console.WriteLine($"Property '{_propertyName}' not found in entity '{_entityType.Name}'.");
                return null;
            }

            PropertyInfo repositoryProperty = typeof(UnitOfWork)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(p => p.PropertyType.Name.Contains(entityProperty.PropertyType.Name));

            if (repositoryProperty == null)
            {
                Console.WriteLine($"Repository for '{entityProperty.PropertyType.Name}' not found in UnitOfWork.");
                return null;
            }

            return repositoryProperty.GetValue(_unitOfWork);
        }


        public void Apply()
        {
            BtnApply_Click(null, null);
        }

        public string GetTextValue() => _textBox?.Text;
        public object GetStatusValue() => _statusDropdown?.SelectedValue;

    }
}
