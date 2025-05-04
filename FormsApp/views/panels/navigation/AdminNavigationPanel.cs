using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Database.Core.Domain;

namespace FormsApp.views.panels
{
    /// <summary>
    /// A navigation panel for the admin interface that allows access to different views 
    /// (e.g., Dashboard, Equipment, Rental Records, Logs) based on label selection.
    /// </summary>
    public partial class AdminNavigationPanel : UserControl
    {
        #region Fields

        /// <summary>
        /// Maps labels to the associated entity type and permission flags (Add, Edit, Delete).
        /// Used to determine which entity is shown and what operations are allowed.
        /// </summary>
        private Dictionary<Label, (Type entity, bool allowAdd, bool allowEdit, bool allowDelete)> _tabTypeMap;

        /// <summary>
        /// The container panel where selected views will be loaded.
        /// </summary>
        private Panel view;

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes the AdminNavigationPanel with a reference to the main view container.
        /// </summary>
        /// <param name="view">Panel to display entity views and dashboard controls.</param>
        public AdminNavigationPanel(Panel view)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.view = view;
        }
        #endregion

        #region Initialization

        /// <summary>
        /// Handles the control load event. Initializes label-entity mappings and click handlers, then loads the dashboard.
        /// </summary>
        private void AdminNavigationPanel_Load(object sender, EventArgs e)
        {
            InitializeTabMap();
            AttachClickEvents();
            DisplayDashboard();
        }

        /// <summary>
        /// Defines mappings between labels and their corresponding entity types and permission sets.
        /// </summary>
        private void InitializeTabMap()
        {
            _tabTypeMap = new()
            {
                { lblCategories, (typeof(Category), true, true, true) },
                { lblRentalRequests, (typeof(RentalRequest), true, true, true) },
                { lblEquipment, (typeof(Equipment), true, true, true) },
                { lblRentalRecords, (typeof(RentalRecord), true, true, true) },
                { lblAuditTrails, (typeof(AuditLog), false, false, false) },
                { lblErrorLogs, (typeof(SystemErrorLog), false, false, false) },
                { lblUsers, (typeof(User), true, true, true) }
            };
        }

        /// <summary>
        /// Subscribes all mapped labels to the click event that will load their respective views.
        /// </summary>
        private void AttachClickEvents()
        {
            foreach (var label in _tabTypeMap.Keys)
                label.Click += NavigationLabel_Clicked;
        }

        #endregion
        #region Event Handlers

        /// <summary>
        /// Handles label click events, determines which control to load based on label clicked.
        /// </summary>
        private void NavigationLabel_Clicked(object sender, EventArgs e)
        {
            if (sender is not Label label) return;

            SetLabelAsSelected(label);

            if (label == lblDashboard)
            {
                DisplayDashboard();
            }
            else if (_tabTypeMap.TryGetValue(label, out var targetInfo))
            {
                LoadEntityPanel(targetInfo);
            }
        }

        private void lblDashboard_Click(object sender, EventArgs e)
        {
            DisplayDashboard();
        }
        private void pnlProfile_Click(object sender, EventArgs e)
        {
            DisplayProfile();
        }
        private void pnlNotification_Click(object sender, EventArgs e)
        {
            DisplayNotification();
        }
        #endregion

        #region UI State Helpers

        /// <summary>
        /// Applies selected style to a label (green bold text) and resets all others.
        /// </summary>
        /// <param name="label">Label to highlight as selected.</param>
        private void SetLabelAsSelected(Label label)
        {
            ResetAllLabelsToRegularFont();
            label.ForeColor = Global.Green;
            label.Font = new Font(label.Font.FontFamily, label.Font.Size, FontStyle.Bold);
        }

        /// <summary>
        /// Resets all navigation labels to their default visual style (black regular font).
        /// </summary>
        private void ResetAllLabelsToRegularFont()
        {
            foreach (Control control in this.pnlPanel.Controls)
            {
                if (control is Label label)
                {
                    label.Font = new Font(label.Font.FontFamily, label.Font.Size, FontStyle.Regular);
                    label.ForeColor = Color.Black;
                }
            }
        }
        #endregion

        #region View Loading
        /// <summary>
        /// Creates and loads a view control for a given entity with specific permissions.
        /// </summary>
        /// <param name="config">Tuple containing entity type and permission flags.</param>
        private void LoadEntityPanel((Type entity, bool allowAdd, bool allowEdit, bool allowDelete) config)
        {
            UserControl userControl = new BaseDBSetView(
                config.entity,
                config.allowAdd,
                config.allowEdit,
                config.allowDelete
            );

                FillView(userControl);
        }

        /// <summary>
        /// Clears the view container and adds the given UserControl.
        /// </summary>
        /// <param name="panel">The control to display inside the view panel.</param>
        private void FillView(UserControl panel)
        {
            panel.Dock = DockStyle.Fill;
            view.Controls.Clear();
            view.Controls.Add(panel);
        }

        /// <summary>
        /// Loads the admin dashboard view and highlights the dashboard label.
        /// </summary>
        private void DisplayDashboard()
        {
            SetLabelAsSelected(lblDashboard);
            FillView(new AdminDashboardView());
        }

        /// <summary>
        /// Loads the admin profile view. 
        /// </summary>
        private void DisplayProfile()
        {
            ResetAllLabelsToRegularFont();

            // TODO: Implement loading of profile view
            // FillView(new AdminProfileView());
        }

        /// <summary>
        /// Loads the admin notifications view.
        /// </summary>
        private void DisplayNotification()
        {
            ResetAllLabelsToRegularFont();

            // TODO: Implement loading of notification view
            // FillView(new AdminNotificationView());
        }
        #endregion

    }
}
