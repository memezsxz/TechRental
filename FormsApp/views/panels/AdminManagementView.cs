using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Database.Core.Domain;

namespace FormsApp.views.panels;

public partial class AdminManagementView : UserControl
{
    #region Fields

    /// <summary>
    /// Maps labels to their corresponding entity viewType and permission flags (Add, Edit, Delete).
    /// </summary>
    private Dictionary<Label, (Type entity, bool allowAdd, bool allowEdit, bool allowDelete)> _tabTypeMap;

    /// <summary>
    /// Currently selected label for navigation.
    /// </summary>
    private Label _selectedLabel;

    #endregion

    #region Constructor

    public AdminManagementView()
    {
        InitializeComponent();
    }

    #endregion

    #region Lifecycle Events

    /// <summary>
    /// Initializes the component and sets up the label-to-entity mapping and event handlers.
    /// </summary>
    private void AdminManagementView_Load(object sender, EventArgs e)
    {
        InitializeTabMap();
        AttachClickEvents();
        AttachPaintToSelectedLabel();

        NavigationLabel_Clicked(_selectedLabel, EventArgs.Empty);
    }

    #endregion

    #region Initialization Methods

    /// <summary>
    /// Initializes the mapping of labels to entities and permission flags.
    /// </summary>
    private void InitializeTabMap()
    {
        _tabTypeMap = new()
        {
            { lblCategory, (typeof(Category), true, true, true) },
            { lblRequests, (typeof(RentalRequest), true, true, true) },
            { lblInventory, (typeof(Equipment), true, true, true) },
            { lblReturns, (typeof(RentalRecord), true, true, true) }, // TODO: Update to rental return class later
            { lblHistory, (typeof(Log), false, false, false) },
            { lblExceptions, (typeof(ErrorLog), false, false, false) },
            { lblUsers, (typeof(User), true, true, true) }
        };

        _selectedLabel = lblCategory;
    }

    /// <summary>
    /// Attaches Click event handlers to all navigation labels.
    /// </summary>
    private void AttachClickEvents()
    {
        foreach (var label in _tabTypeMap.Keys) label.Click += NavigationLabel_Clicked;
    }

    /// <summary>
    /// Attaches the Paint event only to the selected label for highlighting.
    /// </summary>
    private void AttachPaintToSelectedLabel()
    {
        foreach (var label in _tabTypeMap.Keys) label.Paint -= PaintBorder;

        _selectedLabel.Paint += PaintBorder;
        _selectedLabel.Invalidate();
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Handles label click: updates selected label, redraws border, and loads the relevant control.
    /// </summary>
    private void NavigationLabel_Clicked(object sender, EventArgs e)
    {
        if (sender is not Label clickedLabel || !_tabTypeMap.TryGetValue(clickedLabel, out var targetInfo)) return;

        // Remove border from previously selected label
        _selectedLabel?.Invalidate();

        // Update current selection
        _selectedLabel = clickedLabel;

        // Reattach paint handler and redraw
        AttachPaintToSelectedLabel();

        // Load the appropriate panel
        LoadEntityPanel(targetInfo);
    }

    /// <summary>
    /// Paints a dashed highlight border around the selected label.
    /// </summary>
    private void PaintBorder(object sender, PaintEventArgs e)
    {
        // Ensure the sender is a Label; exit if not
        if (sender is not Label label) return;

        // Create a dashed pen with a soft blue-gray color
        using Pen dashedPen = new Pen(Color.FromArgb(191, 199, 217))
        {
            DashStyle = System.Drawing.Drawing2D.DashStyle.Dash,
            Width = 5
        };

        // Define the rectangle boundary for the label border
        Rectangle rect = new Rectangle(0, 0, label.Width - 1, label.Height - 1);

        // Draw the border using the specified pen
        e.Graphics.DrawRectangle(dashedPen, rect);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Loads the UserControl associated with the selected label and permissions.
    /// </summary>
    /// <param name="config">Tuple containing the entity viewType and its permission flags.</param>
    private void LoadEntityPanel((Type entity, bool allowAdd, bool allowEdit, bool allowDelete) config)
    {
        UserControl userControl = new BaseDBSetView(
            config.entity,
            config.allowAdd,
            config.allowEdit,
            config.allowDelete
        );

        pnlDataView.Controls.Clear();
        userControl.Dock = DockStyle.Fill;
        pnlDataView.Controls.Add(userControl);
    }

    #endregion
}