using Database.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormsApp.views.dialogs;
public abstract class BaseViewEditDeleteForm : Form
{
    protected int? id;
    protected UnitOfWork context = new UnitOfWork();
    protected Label deleteLabel;
    protected Label closeLabel;
    protected Label saveLabel;
    public enum ViewType
    {
        ADD, EDIT, DELETE
    }

    public ViewType FormViewType { get; private set; }

    public event Action OnSuccessfulComplete;
    public event Action OnFailedComplete;
    public event Action OnCancel;
    public bool ShouldAutoClose { get; private set; } = false;


    protected BaseViewEditDeleteForm(ViewType viewType, int? id)
    {
        this.FormViewType = viewType;
        this.id = id;
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
                        MessageBox.Show("Cannot edit id null");
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
            case ViewType.DELETE:
                {
                    break;
                }
        }
    }

    protected abstract void InitializeForm();
    /// <summary>
    /// Prepares the form UI for editing an existing item entry by loading its data.
    /// </summary>

    protected abstract void PrepareForEdit();
    /// <summary>
    /// Prepares the form UI for adding a new item entry.
    /// </summary>
    protected abstract void PrepareForAdd();
    /// <summary>
    /// Loads item data into the form UI from the database using the provided ID.
    /// </summary>
    /// <returns>True if the item was found; false otherwise.</returns>
    protected abstract bool FetchItem();
    /// <summary>
    /// Saves the item to the database after validation and any image upload.
    /// </summary>
    protected abstract Task SaveItem();
    /// <summary>
    /// Deletes the item from the database or marks it inactive if it is currently referenced.
    /// Should be called without showing the form.
    /// </summary>
    public abstract void Delete();

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
            deleteLabel.Click += lblDelete_Click;
        }
    }


    void SetBorderColor(Label sender, PaintEventArgs e, Color color)
    {
        ControlPaint.DrawBorder(e.Graphics, sender.DisplayRectangle, color, ButtonBorderStyle.Solid);
    }

    protected bool ValidateTextLength(string value, Label errorLabel, string fieldName, bool required, int minLength, int maxLength)
    {
        value = value.Trim();

        if (string.IsNullOrWhiteSpace(value))
        {
            if (required)
            {
                return ActivateError(errorLabel, $"{fieldName} is required");
            }

            return true;
        }

        if (value.Length < minLength)
        {
            return ActivateError(errorLabel, $"{fieldName} must be longer than {minLength - 1} characters");
        }

        if (value.Length > maxLength)
        {
            return ActivateError(errorLabel, $"{fieldName} must not exceed {maxLength} characters");
        }

        return true;
    }


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
            {
                return ActivateError(errorLabel, $"{fieldName} is required");
            }

            return true;
        }

        try
        {
            T value = parser(input);
            result = value;

            if (!allowZero && value.CompareTo((T)Convert.ChangeType(0, typeof(T))) == 0)
            {
                return ActivateError(errorLabel, $"{fieldName} cannot be zero");
            }

            if (!allowNegative && value.CompareTo((T)Convert.ChangeType(0, typeof(T))) < 0)
            {
                return ActivateError(errorLabel, $"{fieldName} cannot be negative");
            }

            if (minValue.HasValue && value.CompareTo(minValue.Value) < 0)
            {
                return ActivateError(errorLabel, $"{fieldName} must be at least {minValue.Value}");
            }

            if (maxValue.HasValue && value.CompareTo(maxValue.Value) > 0)
            {
                return ActivateError(errorLabel, $"{fieldName} must not exceed {maxValue.Value}");
            }

            return true;
        }
        catch
        {
            return ActivateError(errorLabel, $"{fieldName} must be a valid number");
        }


    }
    protected bool ActivateError(Label label, string message)
    {
        label.Text = message;
        label.Visible = true;
        return false;
    }

    private void lblDelete_Click(object sender, EventArgs e)
    {
        Delete();
    }

    private void lblClose_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void lblSave_Click(object sender, EventArgs e)
    {
        SaveItem();
    }

    public bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;

        // Regular expression for basic email validation
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }
    public bool IsValidPhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return false;

        // Accepts formats like: +1234567890, 123-456-7890, (123) 456-7890
        string pattern = @"^\+?[0-9\s\-()]{7,15}$";
        return Regex.IsMatch(phone, pattern);
    }

    protected void RaiseSuccessfulComplete() => OnSuccessfulComplete?.Invoke();
    protected void RaiseFailedComplete() => OnFailedComplete?.Invoke();
    protected void RaiseCancel() => OnCancel?.Invoke();

}
