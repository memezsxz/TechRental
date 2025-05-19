using Database.Core.Domain;

namespace FormsApp.views.dialogs
{
    //public partial class ManageErrors : Form
    public partial class ManageErrors : BaseViewEditDeleteForm
    {
        #region Fields
        private SystemErrorLog item;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the ManageErrors form with the specified view type and optional equipment ID.
        /// </summary>
        /// <param name="viewType">The mode in which the form is opened (Add, Edit, or View).</param>
        /// <param name="id">Optional ID of the category to load in Edit mode.</param>
        public ManageErrors(BaseViewEditDeleteForm.ViewType viewType, int? id = null) : base(viewType, id) { }

        #endregion

        #region Form Initialization

        /// <summary>
        /// Initializes the form components, disables validation errors,
        /// maps action buttons, and loads dropdown lists.
        /// </summary>
        protected override void InitializeForm()
        {
            InitializeComponent();
            MapActionButtons(lblClose, lblSave, lblDelete);
        }

        #endregion

        #region View Preparation

        protected override void PrepareForView()
        {
            base.PrepareForView();
            LoadItemInfo();
        }

        protected override void PrepareForAdd()
        {
            MessageBox.Show("Cannot add a log");
            return;

        }
        protected override void PrepareForEdit()
        {
            MessageBox.Show("Cannot edit a log");
            return;
        }

        /// <summary>
        /// Loads data from the item into the form controls.
        /// </summary>
        private void LoadItemInfo()
        {
            tbId.Text = item.Id.ToString();
            tbUserId.Text = item.UserId.ToString();
            tbSourceProseadure.Text = item.SourceProcedure;
            tbErrorMessage.Text = item.ErrorMessage;
            dtpTimestamp.Text = item.Timestamp.ToString() ;
            tbSource.Text = item.ErrorSource;
        }

        #endregion



        #region Data Loaders
        protected override bool FetchItem()
        {
            item = context.SystemErrorLogs.Get(id.Value);

            if (item != null) return true;

            MessageBox.Show($"Log with the id {id.Value} not found");
            return false;
        }
     

        #endregion

        #region Save/Delete Logic
        public override void Delete()
        {
            MessageBox.Show("Cannot Delete a log.");
            Dispose();
        }
        protected override async Task SaveItem()
        {
            MessageBox.Show("Cannot edit a log.");
            Dispose();
        }

        protected override void MapFormToEntity()
        {
            
        }

        #endregion
    }
}
