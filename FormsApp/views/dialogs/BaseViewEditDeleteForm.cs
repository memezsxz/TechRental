using Database.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormsApp.views.dialogs;

public abstract class BaseViewEditDeleteForm : Form
{
    #region Fields

    /// <summary>
    /// The unique identifier of the item being managed (used in Edit, View, or Delete modes).
    /// </summary>
    protected int? id;

    /// <summary>
    /// Shared UnitOfWork context used for database operations.
    /// </summary>
    protected UnitOfWork context = new UnitOfWork(Global.userID);

    /// <summary>
    /// Reference to the delete action label in the form UI.
    /// </summary>
    protected Label deleteLabel;

    /// <summary>
    /// Reference to the close action label in the form UI.
    /// </summary>
    protected Label closeLabel;

    /// <summary>
    /// Reference to the save action label in the form UI.
    /// </summary>
    protected Label saveLabel;

    /// <summary>
    /// Indicates whether the delete action is allowed for the current view.
    /// </summary>
    protected bool canDelete;

    #endregion

    #region Enums

    /// <summary>
    /// Defines the mode in which the form is being opened.
    /// </summary>
    public enum ViewType
    {
        ADD,
        EDIT,
        DELETE,
        VIEW
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the current form view mode (Add, Edit, View, or Delete).
    /// </summary>
    public ViewType FormViewType { get; protected set; }

    /// <summary>
    /// Event triggered when an operation completes successfully (Save/Delete).
    /// </summary>
    public event Action OnSuccessfulComplete;

    /// <summary>
    /// Event triggered when an operation fails.
    /// </summary>
    public event Action OnFailedComplete;

    /// <summary>
    /// Event triggered when the form is canceled.
    /// </summary>
    public event Action OnCancel;

    /// <summary>
    /// Indicates whether the form should automatically close after completing an action.
    /// </summary>
    public bool ShouldAutoClose { get; private set; } = false;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the form with the specified view type and item ID.
    /// </summary>
    /// <param name="viewType">The intended mode for this form (Add, Edit, View, Delete).</param>
    /// <param name="id">The ID of the item to edit/view/delete (if applicable).</param>
    /// <param name="canDelete">Flag indicating if delete is permitted for this instance.</param>
    /// <param name="switchType">Determines whether to auto-switch and execute the corresponding preparation logic immediately.</param>
    protected BaseViewEditDeleteForm(ViewType viewType, int? id, bool canDelete = false, bool switchType = true)
    {
        this.FormViewType = viewType;
        this.id = id;
        this.canDelete = canDelete;
        if (switchType) SwitchType();
    }

    #endregion

    #region Form Logic

    /// <summary>
    /// Executes the appropriate logic and UI setup based on the current view type.
    /// </summary>
    protected void SwitchType()
    {
        switch (FormViewType)
        {
            case ViewType.ADD:
                {
                    InitializeForm();
                    PrepareForAdd();
                    break;
                }
            case ViewType.EDIT:
                {
                    if (id == null)
                    {
                        MessageBox.Show("Cannot edit: ID is null.");
                        Close();
                        return;
                    }

                    if (!FetchItem())
                    {
                        Close();
                        return;
                    }

                    InitializeForm();
                    PrepareForEdit();
                    break;
                }

            case ViewType.VIEW:
                {
                    if (id == null)
                    {
                        MessageBox.Show("Cannot view: ID is null.");
                        Close();
                        return;
                    }

                    if (!FetchItem())
                    {
                        Close();
                        return;
                    }

                    InitializeForm();
                    PrepareForView();
                    break;
                }

            case ViewType.DELETE:
                {
                    // Note: DELETE view type is not used for form display, instead Delete() is called externally.
                    break;
                }
        }
    }

    #endregion

    #region Form Lifecycle Methods

    /// <summary>
    /// Called once at form initialization. Must be implemented by derived forms to initialize controls, bindings, and layout.
    /// </summary>
    protected abstract void InitializeForm();

    /// <summary>
    /// Prepares the form UI for editing an existing item.
    /// Must populate fields with existing data and configure UI accordingly.
    /// </summary>
    protected abstract void PrepareForEdit();

    /// <summary>
    /// Prepares the form UI for viewing an existing item without editing.
    /// Hides the save button, optionally shows delete if allowed, and repositions close button.
    /// </summary>
    protected virtual void PrepareForView()
    {
        saveLabel.Visible = false;
        deleteLabel.Visible = canDelete;
        closeLabel.Location = saveLabel.Location;
    }

    /// <summary>
    /// Prepares the form UI for adding a new item.
    /// Must configure controls and default values appropriately.
    /// </summary>
    protected abstract void PrepareForAdd();

    /// <summary>
    /// Fetches the item from the database using the provided ID and prepares it for display/edit.
    /// </summary>
    /// <returns>True if the item is found; otherwise, false.</returns>
    protected abstract bool FetchItem();

    /// <summary>
    /// Performs validation, data mapping, and saves the item to the database.
    /// Should be triggered by save action in the form.
    /// </summary>
    protected abstract Task SaveItem();

    /// <summary>
    /// Deletes the item from the database or marks it as inactive if it is referenced elsewhere.
    /// This should be callable externally and does not require the form to be displayed.
    /// If the form was shown then the method was called from it -from outside the class- it will cause an exception.
    /// </summary>
    public abstract void Delete();

    /// <summary>
    /// Converts form input into an entity object. Must be implemented by each derived form.
    /// </summary>
    protected abstract void MapFormToEntity();

    #endregion

    #region Button Mapping & Styling

    /// <summary>
    /// Attaches save, close, and delete event handlers to the corresponding labels in the form.
    /// </summary>
    protected void PrepareActionButtons()
    {
        if (saveLabel != null)
        {
            saveLabel.Click += lblSave_Click;
        }

        if (closeLabel != null)
        {
            closeLabel.Paint += (s, e) => SetBorderColor(closeLabel, e, Global.Green);
            closeLabel.Click += lblClose_Click;
        }

        if (deleteLabel != null)
        {
            deleteLabel.Visible = canDelete;
            deleteLabel.Click += lblDelete_Click;
        }
    }

    #endregion

    #region UI Utilities

    /// <summary>
    /// Draws a solid border of the specified color around a label.
    /// </summary>
    private void SetBorderColor(Label sender, PaintEventArgs e, Color color)
    {
        ControlPaint.DrawBorder(e.Graphics, sender.DisplayRectangle, color, ButtonBorderStyle.Solid);
    }

    /// <summary>
    /// Validates that a text field is not empty and within specified length bounds.
    /// </summary>
    protected bool ValidateTextLength(string value, Label errorLabel, string fieldName, bool required, int minLength,
        int maxLength)
    {
        value = value.Trim();

        if (string.IsNullOrWhiteSpace(value))
        {
            if (required)
                return ActivateError(errorLabel, $"{fieldName} is required");
            return true;
        }

        if (value.Length < minLength)
            return ActivateError(errorLabel, $"{fieldName} must be longer than {minLength - 1} characters");

        if (value.Length > maxLength)
            return ActivateError(errorLabel, $"{fieldName} must not exceed {maxLength} characters");

        return true;
    }

    /// <summary>
    /// Validates a numeric field, enforcing rules for null, zero, negative values, and boundaries.
    /// </summary>
    protected bool ValidateNumericField<T>(
        string input,
        Label errorLabel,
        string fieldName,
        bool required,
        bool allowZero,
        bool allowNegative,
        Func<string, T> parser,
        out T? result,
        T? minValue = null,
        T? maxValue = null) where T : struct, IComparable<T>
    {
        result = null;

        if (string.IsNullOrWhiteSpace(input))
        {
            if (required)
                return ActivateError(errorLabel, $"{fieldName} is required");
            return true;
        }

        try
        {
            T value = parser(input);
            result = value;

            if (!allowZero && value.CompareTo((T)Convert.ChangeType(0, typeof(T))) == 0)
                return ActivateError(errorLabel, $"{fieldName} cannot be zero");

            if (!allowNegative && value.CompareTo((T)Convert.ChangeType(0, typeof(T))) < 0)
                return ActivateError(errorLabel, $"{fieldName} cannot be negative");

            if (minValue.HasValue && value.CompareTo(minValue.Value) < 0)
                return ActivateError(errorLabel, $"{fieldName} must be at least {minValue.Value}");

            if (maxValue.HasValue && value.CompareTo(maxValue.Value) > 0)
                return ActivateError(errorLabel, $"{fieldName} must not exceed {maxValue.Value}");

            return true;
        }
        catch
        {
            return ActivateError(errorLabel, $"{fieldName} must be a valid number");
        }
    }

    /// <summary>
    /// Displays an error message on the specified label and returns false.
    /// </summary>
    protected bool ActivateError(Label label, string message)
    {
        label.Text = message;
        label.Visible = true;
        return false;
    }

    #endregion

    #region Label Event Handlers

    private void lblDelete_Click(object sender, EventArgs e) => Delete();

    private void lblClose_Click(object sender, EventArgs e) => Close();

    private void lblSave_Click(object sender, EventArgs e) => SaveItem();

    #endregion

    #region Validation Helpers

    /// <summary>
    /// Validates if an email address matches a standard email format.
    /// </summary>
    public bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }

    /// <summary>
    /// Validates if a phone number is in a common international or local format.
    /// </summary>
    public bool IsValidPhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return false;
        string pattern = @"^\+?[0-9\s\-()]{7,15}$";
        return Regex.IsMatch(phone, pattern);
    }

    #endregion

    #region Completion Events

    /// <summary>
    /// Triggers the successful completion event.
    /// </summary>
    protected void RaiseSuccessfulComplete() => OnSuccessfulComplete?.Invoke();

    /// <summary>
    /// Triggers the failure completion event.
    /// </summary>
    protected void RaiseFailedComplete() => OnFailedComplete?.Invoke();

    /// <summary>
    /// Triggers the cancel event.
    /// </summary>
    protected void RaiseCancel() => OnCancel?.Invoke();

    #endregion

    #region Image Handling

    /// <summary>
    /// Loads an image into a panel for preview, using the image's GUID and type.
    /// </summary>
    protected async Task LoadImage(Guid? guid, string? imageType, Panel displayPanel, Label imageLabel)
    {
        try
        {
            // If no GUID is provided, notify the user and exit.
            if (!guid.HasValue)
            {
                imageLabel.Text = "No Image Selected";
                return;
            }

            // Attempt to retrieve the image using the GUID and type.
            var image = await Global.GetImage(guid.Value, imageType);

            // If retrieval fails, update label to indicate the image could not be loaded.
            if (image == null)
            {
                imageLabel.Text = "Unable To Load Image";
                return;
            }

            // Clear any existing image preview in the panel.
            displayPanel.Controls.Clear();

            // Add a PictureBox with the retrieved image to the panel.
            displayPanel.Controls.Add(new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = new Bitmap(image)
            });

            // Update label to show successful load state.
            imageLabel.Text = "Upload";

            // Force redraw of the panel to reflect changes.
            displayPanel.Invalidate();
        }
        catch (Exception ex)
        {
            // On failure (e.g., bad image format, stream issue), notify user and log the error.
            imageLabel.Text = "Image Not Found";
            //Console.WriteLine($"Image loading error: {ex.Message}");
        }
    }

    #endregion

    #region Utility Mapping

    /// <summary>
    /// Links provided close, save, and delete labels to the internal handlers.
    /// </summary>
    protected virtual void MapActionButtons(Label close, Label save, Label delete)
    {
        closeLabel = close;
        saveLabel = save;
        deleteLabel = delete;
        PrepareActionButtons();
    }

    #endregion

    #region Standard Save/Delete Logic

    /// <summary>
    /// Executes a standard deletion process for an entity.
    /// If the entity is referenced (e.g., via foreign keys), the user is prompted to mark it as inactive instead.
    /// This method handles both soft and hard deletes with user confirmation and displays feedback dialogs accordingly.
    /// </summary>
    /// <typeparam name="T">The type of the entity being deleted.</typeparam>
    /// <param name="fetchFunc">A delegate to fetch the entity by ID.</param>
    /// <param name="isReferencedFunc">A delegate to determine if the entity is referenced elsewhere.</param>
    /// <param name="markInactive">A delegate to soft-delete (mark inactive) the entity.</param>
    /// <param name="removeFunc">A delegate to hard-delete the entity from the context.</param>
    /// <param name="entityLabel">A friendly name for the entity used in dialogs (e.g., "Category").</param>
    /// <returns>True if the item was hard-deleted; false otherwise.</returns>
    protected bool StandardDelete<T>(
        Func<int, T?> fetchFunc,
        Func<int, bool> isReferencedFunc,
        Action<T> markInactive,
        Action<T> removeFunc,
        string entityLabel = "Item")
    {
        if (id == null)
        {
            MessageBox.Show("Cannot delete: ID is null");
            Dispose();
            return false;
        }

        // Attempt to retrieve the entity by ID
        var entity = fetchFunc(id.Value);
        if (entity == null)
        {
            MessageBox.Show($"{entityLabel} with ID {id} not found");
            Dispose();
            return false;
        }

        // Confirm deletion with the user
        var confirmResult = MessageBox.Show(
            $"Are you sure you want to delete this {entityLabel}?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirmResult != DialogResult.Yes)
            return false;

        // Handle soft delete if entity is in use
        if (isReferencedFunc(id.Value))
        {
            var result = MessageBox.Show(
                $"{entityLabel} is in use. Mark as inactive instead?",
                $"{entityLabel} in use",
                MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                try
                {
                    markInactive(entity);
                    context.SaveChanges();
                    RaiseSuccessfulComplete();
                }
                catch (Exception ex)
                {
                    Global.DisplayReportErrorDialog(ex);
                    RaiseFailedComplete();
                }
            }

            Dispose();
            return false;
        }

        // Perform hard delete
        try
        {
            removeFunc(entity);
            context.SaveChanges();
            RaiseSuccessfulComplete();
        }
        catch (Exception ex)
        {
            Global.DisplayReportErrorDialog(ex);
            RaiseFailedComplete();
        }

        Dispose();
        return true;
    }


    /// <summary>
    /// Performs a standard save workflow after validation, either adding or updating the entity.
    /// </summary>
    protected async Task<bool> StandardSave<T>(
        Func<bool> validateFunc,
        Action mapFormToEntity,
        Action<T> addFunc,
        Action<T> updateFunc,
        T entity,
        int id,
        string entityLabel)
    {
        if (!validateFunc()) return false;

        try
        {
            mapFormToEntity();

            if (id == 0) addFunc(entity);
            else updateFunc(entity);

            var rows = context.SaveChanges();

            if (rows > 0)
            {
                MessageBox.Show($"{entityLabel} {(FormViewType == ViewType.ADD ? "added" : "updated")} successfully",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RaiseSuccessfulComplete();
                Close();
            }
            else
            {
                MessageBox.Show("Please try again.", "No Changes Saved", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return true;
        }
        catch (Exception ex)
        {
            Global.DisplayReportErrorDialog(ex);
            RaiseFailedComplete();
            return false;
        }
    }

    /// <summary>
    /// Executes a standard save operation with asynchronous validation logic.
    /// This is useful when validation involves database or network operations.
    /// Handles both Add and Edit scenarios, saves changes to the database, and raises appropriate feedback events.
    /// </summary>
    /// <typeparam name="T">The type of the entity being saved.</typeparam>
    /// <param name="validateFuncAsync">A delegate that asynchronously validates the form data.</param>
    /// <param name="mapFormToEntity">A delegate to map UI data into the entity object.</param>
    /// <param name="addFunc">A delegate that adds the entity to the context in Add mode.</param>
    /// <param name="updateFunc">A delegate that updates the entity in the context in Edit mode.</param>
    /// <param name="entity">The entity instance to be saved.</param>
    /// <param name="id">The entity ID; 0 indicates a new entity (Add).</param>
    /// <param name="entityLabel">A friendly name used in confirmation dialogs and messages.</param>
    /// <param name="userId">The user performing the operation. Included for extensibility (e.g., audit logs).</param>
    /// <returns>A task that resolves to true if the save succeeded; otherwise, false.</returns>
    protected async Task<bool> StandardSaveAsync<T>(
        Func<Task<bool>> validateFuncAsync,
        Action mapFormToEntity,
        Action<T> addFunc,
        Action<T> updateFunc,
        T entity,
        int id,
        string entityLabel,
        int userId)
    {
        if (!await validateFuncAsync()) return false;

        try
        {
            mapFormToEntity();

            if (id == 0) addFunc(entity);
            else updateFunc(entity);

            var rows = context.SaveChanges();

            if (rows > 0)
            {
                MessageBox.Show(
                    $"{entityLabel} {(FormViewType == ViewType.ADD ? "added" : "updated")} successfully",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                RaiseSuccessfulComplete();
                Close();
            }
            else
            {
                MessageBox.Show("Please try again.", "No Changes Saved", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return true;
        }
        catch (Exception ex)
        {
            Global.DisplayReportErrorDialog(ex);
            RaiseFailedComplete();
            return false;
        }
    }

    #endregion
}