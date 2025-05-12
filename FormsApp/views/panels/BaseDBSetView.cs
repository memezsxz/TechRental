#region Using Directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Database.Core.Domain;
using Database.Core.Repositories;
using Database.Persistence;
using Database.Search;
using FormsApp.views.controls;
using FormsApp.views.dialogs;
using Microsoft.EntityFrameworkCore;
using Sprache;

#endregion

namespace FormsApp.views.panels;

public partial class BaseDBSetView : UserControl
{

    #region Fields

    /// <summary>
    /// The UnitOfWork instance responsible for coordinating database transactions and repositories.
    /// </summary>
    private UnitOfWork _context = new UnitOfWork();

    /// <summary>
    /// The type of entity currently being managed (e.g., Equipment, RentalRequest, etc.).
    /// Used for dynamic UI and repository operations.
    /// </summary>
    private Type currentType;

    /// <summary>
    /// The current filter/search control being used to display and apply filters on the data.
    /// </summary>
    private BaseSearchControl currentControl;

    /// <summary>
    /// The total number of pages available based on the current filter and page size.
    /// Used for pagination navigation logic.
    /// </summary>
    private int totalPages = 0;

    /// <summary>
    /// Flag indicating whether the current user is allowed to add new records of the selected entity type.
    /// </summary>
    private bool allowAdd = false;

    /// <summary>
    /// Flag indicating whether the current user is allowed to edit existing records of the selected entity type.
    /// </summary>
    private bool allowEdit = false;

    /// <summary>
    /// Flag indicating whether the current user is allowed to delete records of the selected entity type.
    /// </summary>
    private bool allowDelete = false;

    #endregion


    #region Constructor

    public BaseDBSetView(Type currentType, bool allowAdd, bool allowEdit, bool allowDelete)
    {
        InitializeComponent();
        this.currentType = currentType;
        this.allowAdd = allowAdd;
        this.allowEdit = allowEdit;
        this.allowDelete = allowDelete;
    }

    #endregion

    #region Form Load

    private void admin_inventory_Load(object sender, EventArgs e)
    {
        cbRecordsNum.DataSource = Global.pageSizes;
        LoadColumnDropdown();
        HandlePermissions();
    }

    private void HandlePermissions()
    {
        btnAdd.Enabled = allowAdd;
        btnEdit.Text = allowEdit ? "Edit" : "View";
        btnDelete.Enabled = allowDelete;
    }

    #endregion

    #region Event Handlers

    private void dropdownColumns_SelectedIndexChanged(object sender, EventArgs e) => ChangeFilterControl();

    /// <summary>
    /// Event handler for the DataBindingComplete event of a DataGridView.
    /// This method formats the column headers by splitting PascalCase into spaced words
    /// and auto-sizes each column to fit its content.
    /// </summary>
    /// <param name="sender">The DataGridView that triggered the event.</param>
    /// <param name="e">Event arguments for data binding completion.</param>
    private void dgvData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
        // Cast the sender to a DataGridView
        if (sender is not DataGridView grid) return;

        // Loop through each column in the grid
        foreach (DataGridViewColumn col in grid.Columns)
        {
            // Format header text by splitting PascalCase (e.g., "RentalDate" -> "Rental Date")
            col.HeaderText = SplitPascalCase(col.DataPropertyName);

            // Automatically resize the column width based on its content
            col.Width = col.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
        }
    }


    /// <summary>
    /// Custom Paint event for the Delete button to draw a red rounded border.
    /// </summary>
    private void btnDelete_Paint(object sender, PaintEventArgs e)
    {
        // Ensure the sender is a Button
        if (sender is not Button btn) return;

        Global.DrawRoundedBorder(btn, e, Color.Red);
    }

    /// <summary>
    /// Handles the click event for pagination panels (Next or Previous),
    /// and delegates the logic to <see cref="HandelPageChange"/>.
    /// </summary>
    /// <param name="sender">The clicked panel (expected to be <c>pnlNext</c> or <c>pnlPrevios</c>).</param>
    /// <param name="e">The event arguments.</param>
    private void page_toggle_Click(object sender, EventArgs e)
    {
        if (sender is not Panel panel || (panel != pnlNext && panel != pnlPrevios)) return;
        HandelPageChange(panel);
    }

    /// <summary>
    /// Handles the change event for the page size dropdown (<c>cbRecordsNum</c>).
    /// Updates the page size of the active <see cref="BaseSearchControl"/> control and reapplies the filter starting from the first page.
    /// </summary>
    /// <param name="sender">The combo box control that triggered the event.</param>
    /// <param name="e">The event arguments.</param>
    private void cbRecordsNum_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbRecordsNum.SelectedItem is not int selectedPageSize) return;
        if (currentControl == null) return;

        currentControl.PageSize = selectedPageSize;
        ApplyFilters();
    }


    /// <summary>
    /// Handles the manual "Apply" button click.
    /// Triggers a fresh application of the active filter starting from page 1.
    /// </summary>
    /// <param name="sender">The button that triggered the event.</param>
    /// <param name="e">The event arguments.</param>
    private void btnApply_Click(object sender, EventArgs e)
    {
        ApplyFilters();
    }

    /// <summary>
    /// Resets the column dropdown to its default ("None") selection.
    /// </summary>
    private void btnReset_Click(object sender, EventArgs e)
    {
        cbColumn.SelectedIndex = 0;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        OpenManageForm(BaseViewEditDeleteForm.ViewType.ADD);
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
        OpenManageForm(allowEdit ? BaseViewEditDeleteForm.ViewType.EDIT : BaseViewEditDeleteForm.ViewType.VIEW);
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        OpenManageForm(BaseViewEditDeleteForm.ViewType.DELETE);
    }

    #endregion

    #region UI Logic

    /// <summary>
    /// Loads the column dropdown (cbColumn) with formatted column names.
    /// Combines a default "None" option with dynamically retrieved column metadata,
    /// then formats and binds them to the ComboBox.
    /// </summary>
    private void LoadColumnDropdown()
    {
        // Attempt to retrieve column metadata from the repository
        try
        {
            var columns = GetColumnsFromRepository();
            if (columns == null) return;

            // Merge the "None" default option with actual columns and format the keys
            var formattedColumns = FormatColumnNames(
                new Dictionary<string, string> { { "None", "" } }
                    .Concat(columns)
                    .ToDictionary(kv => kv.Key, kv => kv.Value)
            );

            // Bind the formatted columns to the ComboBox
            BindColumnDropdown(formattedColumns);
        }
        catch (Exception ex)
        {
            HandleErrors(ex);
        }
    }

    /// <summary>
    /// Binds a formatted dictionary of column display names and internal values to the ComboBox control.
    /// </summary>
    /// <param name="columnInfo">The dictionary of columns to bind, with keys as display names and values as viewType names.</param>
    private void BindColumnDropdown(Dictionary<string, string> columnInfo)
    {
        cbColumn.DisplayMember = "Key"; // Text shown to the user
        cbColumn.ValueMember = "Value"; // Internal identifier (e.g., datatype)
        cbColumn.DataSource = new BindingSource(columnInfo, null); // Bind to combo
    }

    /// <summary>
    /// Updates the filter input control displayed in the UI based on the selected column's data viewType.
    /// Dynamically creates an appropriate control (e.g., textbox, dropdown, date picker) for filtering,
    /// binds it to the UI, and applies the filter if the "None" option is selected.
    /// </summary>
    private void ChangeFilterControl()
    {
        BaseSearchControl control = CreateControlForType(cbColumn.SelectedValue?.ToString(), GetSelectedColumnName());

        if (control == null)
        {
            cbColumn.SelectedIndex = 0;
            return;
        }

        currentControl = control as BaseSearchControl;
        if (cbColumn.SelectedIndex == 0) currentControl.Apply();
        pnlSearch.Controls.Clear();
        pnlSearch.Controls.Add(control);
        control.Dock = DockStyle.Fill;
    }

    /// <summary>
    /// Applies the current search filter from the beginning by resetting the page number to 1
    /// and invoking the <see cref="BaseSearchControl.Apply"/> method on the active filter control.
    /// </summary>
    private void ApplyFilters()
    {
        currentControl.PageNumber = 1;
        currentControl.Apply();
    }

    /// <summary>
    /// Updates the <see cref="currentControl.PageNumber"/> based on which pagination panel was clicked,
    /// and re-applies the current filter by calling <see cref="BaseSearchControl.Apply"/>.
    ///
    /// - If <paramref name="panel"/> is <c>pnlNext</c> and the current page is less than <c>totalPages</c>, it increments the page.
    /// - If <paramref name="panel"/> is <c>pnlPrevios</c> and the current page is greater than 1, it decrements the page.
    /// </summary>
    /// <param name="panel">The panel control that was clicked to trigger pagination.</param>
    private void HandelPageChange(Panel panel)
    {
        // change the page number on the control
        if (panel == pnlNext && currentControl.PageNumber < totalPages)
        {
            currentControl.PageNumber++;
            currentControl.Apply();
        }
        else if (panel == pnlPrevios && currentControl.PageNumber > 1)
        {
            currentControl.PageNumber--;
            currentControl.Apply();
        }

    }

    /// <summary>
    /// Updates the DataGridView and related UI labels with paginated result data.
    /// To be called called after a search or filter operation to display the current page of results,
    /// along with metadata such as total record count and page number.
    /// </summary>
    /// <param name="val">
    /// A <see cref="PaginatedResult"/> containing the data to display, total record count,
    /// and total number of pages.
    /// </param>
    private void UpdatePaginatedData(PaginatedResult val)
    {
        totalPages = val.TotalPages;
        lblTotal.Text = $"Total Records: {val.TotalRecords}";
        lblPagPage.Text = $"Page {currentControl.PageNumber} of {totalPages}";
        dvgItems.DataSource = val.Data;
    }

    /// <summary>
    /// Dynamically creates a filtering control based on the provided data viewType and column name.
    /// This method is used to build the appropriate UI control to filter a specific column,
    /// supporting numeric, text, boolean, status, and date types.
    ///
    /// The generated control implements the <see cref="BaseSearchControl"/> interface and is expected
    /// to raise a <c>OnSearchCompleted</c> event when the filter is applied.
    ///
    /// </summary>
    /// <param name="type">
    /// A string representing the CLR viewType name of the column to filter
    /// (e.g., "String", "Int32", "Decimal", "DateTime", "Boolean").
    /// </param>
    /// <param name="col">
    /// The name of the column being filtered, used to bind filtering logic internally.
    /// </param>
    /// <returns>
    /// A <see cref="BaseSearchControl"/> instance that allows the user to enter a filter condition
    /// for the specified column viewType.
    /// </returns>
    private BaseSearchControl CreateControlForType(string type, string col)
    {
        try
        {
            // get the right control based on the data viewType
            BaseSearchControl control = type switch
            {
                "" => new GetAllControl(currentType),
                "DateTime" => new NumericFilterControl(currentType, col, typeof(DateTime)),
                "Decimal" => new NumericFilterControl(currentType, col, typeof(decimal)),
                "Int32" => new NumericFilterControl(currentType, col, typeof(int)),
                "Boolean" => new TextStatusFilterControl(currentType, col, TextStatusFilterControl.FilterType.Boolean),
                "String" => new TextStatusFilterControl(currentType, col, TextStatusFilterControl.FilterType.String),
                _ => new TextStatusFilterControl(currentType, col, TextStatusFilterControl.FilterType.Status)
            };

            if (!int.TryParse(cbRecordsNum.SelectedItem?.ToString(), out var selectedPageSize)) return null;

            // Set page size
            control.PageSize = selectedPageSize;
            // attach the search handler 
            control.OnSearchCompleted += UpdatePaginatedData;

            return control;
        }
        catch (Exception ex)
        {
            HandleErrors(ex);
            return null;
        }

    }

    /// <summary>
    /// Opens the appropriate management form (Add/Edit/View/Delete) based on the current entity type and requested action.
    /// If the action requires an item to be selected (i.e., not Add), it validates selection and passes the ID to the form.
    /// </summary>
    /// <param name="type">The type of form operation to perform.</param>
    private void OpenManageForm(BaseViewEditDeleteForm.ViewType type)
    {
        int? id = null;

        // For Edit, View, or Delete actions, ensure a row is selected
        if (type != BaseViewEditDeleteForm.ViewType.ADD)
        {
            id = GetSelectedRowId();
            if (id == null)
            {
                MessageBox.Show("Please select an item to perform this action.");
                return;
            }
        }

        BaseViewEditDeleteForm form = null;

        try
        {
            // Determine which form to open based on the entity type
            if (currentType == typeof(Equipment))
            {
                form = new ManageEquipment(type, id);
            }
            else if (currentType == typeof(RentalRequest))
            {
                form = new ManageRental(ManageRental.ItemType.Request, type, id);
            }
            else if (currentType == typeof(RentalRecord))
            {
                form = new ManageRental(ManageRental.ItemType.Record, type, id);
            }
            else if (currentType == typeof(Category))
            {
                form = new ManageCategory(type, id);
            }
            else if (currentType == typeof(User))
            {
                form = new ManageUser(type, id);
            }
            else if (currentType == typeof(AuditLog))
            {
                form = new ManageAuditTrails(type, id);
            }
            else if (currentType == typeof(SystemErrorLog))
            {
                form = new ManageErrors(type, id);
            }

            if (form != null)
            {
                // Reapply the filter/search after any changes from the form
                form.OnSuccessfulComplete += () => currentControl.Apply();

                // Perform delete immediately or show form for other actions
                if (type == BaseViewEditDeleteForm.ViewType.DELETE)
                    form.Delete();
                else
                    form.ShowDialog();
            }
        }
        catch (Exception ex)
        {
            // Handle error during form instantiation or operation
            Global.DisplayReportErrorDialog(ex);
            form?.Close();
        }
    }

    #endregion

    #region Data Access

    /// <summary>
    /// Uses reflection to get a dictionary of column names and data types from the current entity's repository.
    /// </summary>
    /// <returns>
    /// A <c>Dictionary&lt;string, string&gt;</c> containing column names and types,
    /// or <c>null</c> if an error occurs or the method is not found.
    /// </returns>
    private Dictionary<string, string>? GetColumnsFromRepository()
    {


        // Attempt to invoke the metadata method and return the result
        try
        {
            // Try to get the appropriate repository for the current entity viewType
            object repo = Helpers.GetRepositoryForType(currentType);
            if (repo == null)
            {
                throw new InvalidOperationException($"No repository found for viewType '{currentType.Name}'.");
            }

            // Check if the repository implements the required metadata method
            MethodInfo? getColumnsMethod = repo.GetType().GetMethod("GetEntityColumnsWithTypes");
            if (getColumnsMethod == null)
            {
                throw new MissingMethodException(
                    $"Repository for {currentType.Name} does not implement GetEntityColumnsWithTypes.");
            }
            return getColumnsMethod.Invoke(repo, null) as Dictionary<string, string>;
        }
        catch (Exception ex)
        {
            HandleErrors(ex);
            return null;
        }
    }

    #endregion

    #region Utility

    private string SplitPascalCase(string input)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
            Regex.Replace(input, "([a-z])([A-Z])", "$1 $2"));
    }

    /// <summary>
    /// Retrieves the name of the selected column from the dropdown, with any spaces removed,
    /// to ensure it maps correctly to a property in the underlying data model.
    /// </summary>
    /// <returns>The sanitized column name as a string.</returns>
    private string GetSelectedColumnName() =>
        ((KeyValuePair<string, string>)cbColumn.SelectedItem).Key.Replace(" ", "");

    /// <summary>
    /// Formats the keys of a dictionary representing column names for display purposes.
    /// It splits PascalCase into readable strings and optionally removes "Id" suffixes for foreign key references
    /// </summary>
    /// <param name="columns">The original dictionary of column keys and their datatypes.</param>
    /// <returns>A new dictionary with formatted keys for display and the original values.</returns>
    private Dictionary<string, string> FormatColumnNames(Dictionary<string, string> columns)
    {
        // New dictionary to hold the formatted column names
        Dictionary<string, string> newColumns = new Dictionary<string, string>();
        var numericTypes = new HashSet<string> { "Byte", "SByte", "Int16", "UInt16", "Int32", "UInt32", "Int64", "UInt64" };

        foreach (var column in columns)
        {
            // Split PascalCase (e.g., "RentalDate" => "Rental Date")
            string formattedKey = SplitPascalCase(column.Key);

            // Special case: keep "Id" as-is
            if (column.Key == "Id")
            {
                formattedKey = "Id";
            }
            // If the column ends with "Id" (likely a foreign key), remove the " Id" part after splitting
            else if (column.Key.EndsWith("Id") && !numericTypes.Contains(column.Value))
            {
                formattedKey = formattedKey.Replace(" Id", "");
            }

            // Add the formatted key and its corresponding value to the new dictionary
            newColumns.Add(formattedKey, column.Value);
        }

        return newColumns;
    }

    /// <summary>
    /// Gets the ID of the currently selected row in the DataGridView.
    /// </summary>
    /// <returns>The row's ID if valid; otherwise, null.</returns>
    private int? GetSelectedRowId()
    {
        // Ensure a row is selected
        if (dvgItems.CurrentRow == null || dvgItems.CurrentRow.Index < 0)
            return null;

        try
        {
            // Try to extract the ID from the first column
            var value = dvgItems.CurrentRow.Cells[0].Value;
            return value != null ? Convert.ToInt32(value) : null;
        }
        catch
        {
            // Return null if value is not convertible to int
            return null;
        }
    }

    #endregion

    #region Error Handling

    /// <summary>
    /// Centralized error handling for UI operations in this control.
    /// It shows an error dialog, removes the control from its parent, and disposes it.
    /// </summary>
    /// <param name="ex">The exception that occurred.</param>
    private void HandleErrors(Exception ex)
    {
        // Display the error dialog (without showing raw error to the user)
        Global.DisplayReportErrorDialog(ex);

        // Remove this control from its parent container if possible
        if (this.Parent != null)
        {
            this.Parent.Controls.Remove(this);
        }

        // Dispose of this control to free up resources
        this.Dispose();
    }
    #endregion
}