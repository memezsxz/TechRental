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
using Database;
using DotNetEnv;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FormsApp.views.controls
{
    public partial class GetAllControl : BaseSearchControl
    {

        private static readonly string _searchMethodName = "GetAll";

        private static readonly Type[] _searchMethodParams = new[] { typeof(int), typeof(int) };
        public GetAllControl(Type propertyType) : base(propertyType, "", _searchMethodName, _searchMethodParams)
        {
            InitializeComponent();
            PerformSearch();
        }

        public override void Apply()
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            InvokeSearch(new object[] { PageNumber, PageSize });
        }

    }
}
