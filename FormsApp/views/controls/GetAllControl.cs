using FormsApp.views.panels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsApp.views.controls
{
    public partial class GetAllControl : UserControl, ISearch
    {

        private readonly Type _propertyType;
        private readonly Type _colType;
        private readonly string _propertyName;
        private Control _inputValue1;
        private Control _inputValue2;
        //private Button _applyButton;
        private Label _andLabel;

        private int _pageNumber = 1;
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }


        public GetAllControl(Type propertyType)
        {
            InitializeComponent();
            _propertyType = propertyType;

            PerformSearch();
        }

        public void Apply()
        {
            PerformSearch();
        }

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? 10 : value;
        }

        public event Action<object> OnSearchCompleted;

        private void PerformSearch()
        {

            var repository = Global.GetRepositoryForType(_propertyType);
            MethodInfo method = repository.GetType().GetMethod("GetAll", new[] { typeof(int), typeof(int) });

            if (method != null)
            {
                var result = method.Invoke(repository, new object[] { PageNumber, PageSize });

                OnSearchCompleted?.Invoke(result);
            }
            else
            {
                Console.WriteLine("Method 'GetAll(int, int)' not found in repository.");
            }
        }

    }
}
