using Database.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsApp.views.dialogs;
public abstract class BaseViewEditDeleteForm : Form
{
    protected int? id;
    protected UnitOfWork context = new UnitOfWork(new RentalDBContext());

    public enum ViewType
    {
        ADD,  EDIT, DELETE
    }

    public ViewType FormViewType { get; private set; }

    public event Action UnsuccessfulComplete;
    public event Action OnFailedComplete;
    public event Action OnCancel;

    public abstract void Delete();

    protected BaseViewEditDeleteForm(ViewType viewType)
    {
        this.FormViewType = viewType;
    }

    protected BaseViewEditDeleteForm(int id) : this(BaseViewEditDeleteForm.ViewType.EDIT)
    {
        this.id = id;
    }
}
