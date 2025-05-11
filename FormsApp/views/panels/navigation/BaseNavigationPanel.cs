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
    public partial class BaseNavigationPanel : UserControl
    {
        #region Fields

        /// <summary>
        /// Maps labels to the associated entity type and permission flags (Add, Edit, Delete).
        /// Used to determine which entity is shown and what operations are allowed.
        /// </summary>
        private Dictionary<Label, (Type entity, bool allowAdd, bool allowEdit, bool allowDelete)> _tabTypeMap;

        private Dictionary<Type, Label> _entityToLabel = new();

        /// <summary>
        /// The container panel where selected views will be loaded.
        /// </summary>
        private Panel view;

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes the BaseNavigationPanel with a reference to the main view container.
        /// </summary>
        /// <param name="view">Panel to display entity views and dashboard controls.</param>
        public BaseNavigationPanel(Panel view)
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
            AdjustRowHeights();
            AttachClickEvents();
            DisplayDashboard();
        }

        /// <summary>
        /// Defines mappings between labels and their corresponding entity types and permission sets.
        /// </summary>
            private void InitializeTabMap()
            {
                _tabTypeMap = new();

                // 1. Remember current profile control and its row index
                pnlPanel.Controls.Remove(tlpProfile);
                pnlPanel.RowStyles.RemoveAt(3);
                pnlPanel.RowCount--; // temporarily remove it

                int insertAt = 2; // Insert role-specific labels starting at row 2

                // 2. Insert role-specific labels
                foreach (var kvp in Global.TabTypeMap)
                {
                    var entityType = kvp.Key;
                    var (allowAdd, allowEdit, allowDelete) = kvp.Value;

                    Label label = new Label
                    {
                        AutoSize = true,
                        Dock = DockStyle.Fill,
                        Font = new Font("Cascadia Mono", 14F, FontStyle.Regular, GraphicsUnit.Point),
                        ForeColor = Color.Black,
                        Margin = new Padding(3, 20, 3, 20),
                        Name = $"lbl{entityType.Name}",
                        Size = new Size(334, 53),
                        TabIndex = 10 + insertAt,
                        Text = GetDisplayName(entityType),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Cursor = Cursors.Hand
                    };

                    pnlPanel.RowStyles.Insert(insertAt, new RowStyle(SizeType.Percent, 1));
                    pnlPanel.RowCount++;
                    pnlPanel.Controls.Add(label, 0, insertAt);

                    _tabTypeMap[label] = (entityType, allowAdd, allowEdit, allowDelete);
                    _entityToLabel[entityType] = label;

                    insertAt++;
                }

                // 3. Re-add the profile control to the last row
                pnlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
                pnlPanel.RowCount++;
                pnlPanel.Controls.Add(tlpProfile, 0, pnlPanel.RowCount - 1);
            }

        

        private void AdjustRowHeights()
        {
            int totalRows = pnlPanel.RowCount;

            if (totalRows < 3) return; // Need at least 3 rows to apply this logic

            pnlPanel.RowStyles.Clear();

            pnlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10)); // First row (brand)

            int dynamicRowCount = totalRows - 2;
            float dynamicHeight = 80f / dynamicRowCount;

            for (int i = 0; i < dynamicRowCount; i++)
            {
                pnlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, dynamicHeight)); // Middle rows (nav)
            }

            pnlPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10)); // Last row (profile/notification)
        }

        private static string GetDisplayName(Type type)
        {
            return type.Name switch
            {
                nameof(AuditLog) => "Audit Trails",
                nameof(SystemErrorLog) => "System Errors",
                nameof(RentalRequest) => "Rental Requests",
                nameof(RentalRecord) => "Rental Records",
                _ => type.Name
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
            FillView(new ProfileView());
        }

        /// <summary>
        /// Loads the admin notifications view.
        /// </summary>
        private void DisplayNotification()
        {
            ResetAllLabelsToRegularFont();

             FillView(new NotificationsView());
        }
        #endregion

    }
}
