using FormsApp.views.panels;
using System;
using System.Drawing;
using System.Windows.Forms;
using Database;

namespace FormsApp.views.controls
{
    /// <summary>
    /// A concrete search control that retrieves all records of a specified entity type
    /// with paging support, using the "GetAll" method on the repository.
    /// </summary>
    public partial class GetAllControl : BaseSearchControl
    {
        #region Constants

        /// <summary>
        /// The name of the method to invoke on the repository (expected to support paging).
        /// </summary>
        private static readonly string _searchMethodName = "GetAll";

        /// <summary>
        /// The expected parameter types for the search method: page number and page size.
        /// </summary>
        private static readonly Type[] _searchMethodParams = new[] { typeof(int), typeof(int) };

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllControl"/> class,
        /// binding to the "GetAll(int page, int pageSize)" method of the corresponding repository.
        /// </summary>
        /// <param name="propertyType">The entity type to retrieve records for.</param>
        public GetAllControl(Type propertyType)
            : base(propertyType, "", _searchMethodName, _searchMethodParams)
        {
            InitializeComponent();

            // Perform initial search on load
            PerformSearch();
        }

        #endregion

        #region Public Overrides

        /// <summary>
        /// Triggers the search logic. This method is usually invoked by the UI
        /// (e.g., when the user presses an "Apply" or "Search" button).
        /// </summary>
        public override void Apply()
        {
            PerformSearch();
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Executes the "GetAll" method on the bound repository with current pagination values.
        /// </summary>
        private void PerformSearch()
        {
            InvokeSearch(new object[] { PageNumber, PageSize });
        }

        #endregion
    }
}
