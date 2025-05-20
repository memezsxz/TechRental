using Database.Core.Domain;

namespace FormsApp.views.dialogs
{
    /*
     * WARNING:
     * To view this form in the Windows Forms Designer,
     * temporarily change the base class from 'BaseViewEditDeleteForm' to 'Form'.
     *
     * Example:
     *     public partial class ManageErrors : Form
     *
     * After making design changes, revert the base class back to 'BaseViewEditDeleteForm'
     * to preserve functionality and application behavior.
     */

    /// <summary>
    /// A read-only form for viewing system error logs. Editing, adding, and deleting are disabled
    /// to preserve the integrity of historical error records.
    /// </summary>
    //public partial class ManageErrors : Form
    public partial class ManageErrors : BaseViewEditDeleteForm
    {
        #region Fields

        /// <summary>
        /// The error log item currently being displayed.
        /// </summary>
        private SystemErrorLog item;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the ManageErrors form with the specified view type and optional error log ID.
        /// </summary>
        /// <param name="viewType">The mode in which the form is opened (View only is supported).</param>
        /// <param name="id">Optional ID of the error log entry to load in View mode.</param>
        public ManageErrors(BaseViewEditDeleteForm.ViewType viewType, int? id = null)
            : base(viewType, id) { }

        #endregion

        #region Form Initialization

        /// <inheritdoc/>
        protected override void InitializeForm()
        {
            InitializeComponent();
            MapActionButtons(lblClose, lblSave, lblDelete);
        }

        #endregion

        #region View Preparation

        /// <inheritdoc/>
        protected override void PrepareForView()
        {
            base.PrepareForView();
            LoadItemInfo();
        }

        /// <inheritdoc/>
        protected override void PrepareForAdd()
        {
            MessageBox.Show("Cannot add a log.");
        }

        /// <inheritdoc/>
        protected override void PrepareForEdit()
        {
            MessageBox.Show("Cannot edit a log.");
        }

        /// <summary>
        /// Loads the error log details into the form controls for viewing.
        /// </summary>
        private void LoadItemInfo()
        {
            tbId.Text = item.Id.ToString();
            tbUserId.Text = item.UserId.ToString();
            tbSourceProseadure.Text = item.SourceProcedure;
            tbErrorMessage.Text = item.ErrorMessage;
            dtpTimestamp.Text = item.Timestamp.ToString();
            tbSource.Text = item.ErrorSource;
        }

        #endregion

        #region Data Loaders

        /// <inheritdoc/>
        protected override bool FetchItem()
        {
            item = context.SystemErrorLogs.Get(id.Value);

            if (item != null)
                return true;

            MessageBox.Show($"Log with the ID {id.Value} was not found.");
            return false;
        }

        #endregion

        #region Save/Delete Logic

        /// <inheritdoc/>
        public override void Delete()
        {
            MessageBox.Show("Cannot delete a log.");
            Dispose();
        }

        /// <inheritdoc/>
        protected override async Task SaveItem()
        {
            MessageBox.Show("Cannot edit a log.");
            Dispose();
        }

        /// <inheritdoc/>
        protected override void MapFormToEntity()
        {
            // Not applicable; logs are read-only
        }

        #endregion

    }
}
