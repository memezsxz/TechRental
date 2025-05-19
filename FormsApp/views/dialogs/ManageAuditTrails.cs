using Database.Core.Domain;
using Microsoft.VisualBasic.ApplicationServices;
using System;

namespace FormsApp.views.dialogs
{
    /*
     * WARNING:
     * To view this form in the Windows Forms Designer,
     * temporarily change the base class from 'BaseViewEditDeleteForm' to 'Form'.
     * 
     * Example:
     *     public partial class ManageAuditTrails : Form
     * 
     * After making design changes, revert the base class back to 'BaseViewEditDeleteForm'
     * to preserve functionality and application behavior.
     */

    /// <summary>
    /// Provides a read-only interface for viewing audit trail entries in the system.
    /// This form disables add, edit, and delete operations to preserve audit integrity.
    /// </summary>
    //public partial class ManageAuditTrails : Form
    public partial class ManageAuditTrails : BaseViewEditDeleteForm
    {
        #region Fields

        /// <summary>
        /// The audit log record currently loaded into the form.
        /// </summary>
        private AuditLog item;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the ManageAuditTrails form with the given view type and optional ID.
        /// </summary>
        /// <param name="viewType">The type of interaction (only View is supported).</param>
        /// <param name="id">The ID of the log entry to load.</param>
        public ManageAuditTrails(BaseViewEditDeleteForm.ViewType viewType, int? id = null)
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
            MessageBox.Show("Cannot add a log");
        }

        /// <inheritdoc/>
        protected override void PrepareForEdit()
        {
            MessageBox.Show("Cannot edit a log");
        }

        /// <summary>
        /// Loads the selected audit log item into the form's UI controls.
        /// </summary>
        private void LoadItemInfo()
        {
            tbId.Text = item.Id.ToString();
            tbActionType.Text = item.ActionType;
            tbDataBeforeAction.Text = item.DataBeforeAction;
            tbDataAfterAction.Text = item.DataAfterAction;
            tbAffectedRecordKey.Text = item.AffectedRecordKey;
            tbSource.Text = item.Source;
            tbSourceEntity.Text = item.SourceEntity;
        }

        #endregion

        #region Data Loaders

        /// <inheritdoc/>
        protected override bool FetchItem()
        {
            item = context.AuditLogs.Get(id.Value);

            if (item != null) return true;

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
            // Not applicable for audit logs (view-only).
        }

        #endregion
    }
}
