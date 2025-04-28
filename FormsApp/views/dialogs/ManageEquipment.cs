using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Database.Core.Domain;
using Database.Persistence;

namespace FormsApp.views.dialogs
{
    public partial class ManageEquipment : BaseViewEditDeleteForm
    {
        private Equipment item;

        public ManageEquipment(BaseViewEditDeleteForm.ViewType viewType) : base(viewType)
        {
        }

        public ManageEquipment(BaseViewEditDeleteForm.ViewType viewType, int id) : base(viewType)
        {
            InitializeComponent();
            this.id = id;
        }



        private void ManageEquipment_Load(object sender, EventArgs e)
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
                            Console.WriteLine("Cannot delete id null");
                            Close();
                            return;
                        }

                        InitializeForm();
                        PrepareForEdit();
                        break;
                    }
                case ViewType.DELETE:
                    {
                        if (id == null)
                        {
                            Console.WriteLine("Cannot delete id null");
                            Close();
                            return;
                        }
                        Delete();
                        break;
                    }
            }
        }


        private void PrepareForAdd()
        {
            lblSave.Text = "Add";
        }
        private void PrepareForEdit()
        {
            if (!FetchItem())
            {
                Close();
                return;
            }

            LoadItemInfo();

        }

        private void LoadItemInfo()
        {
            lblId.Text = item.Id.ToString();
            lblName.Text = item.Name;
            lblPrice.Text = item.RentalPricePerDay.ToString("C");
            ddlAvalability.SelectedValue = item.AvailabilityStatusId;
            ddlCondition.SelectedValue = item.ConditionStatusId;
            ddlCategory.SelectedValue = item.CategoryId;
            tbDescription.Text = item.Description;
            cbIsActive.Checked = item.IsActive ?? false;
        }

        public override void Delete()
        {
            if (!FetchItem())
            {
                Close();
                return;
            }

        }











        private bool FetchItem()
        {
            item = context.Equipment.Find(e => e.Id == id).FirstOrDefault();

            if (item == null)
            {
                MessageBox.Show($"Equipment with the id {id} not found");
                return false;
            }
            return true;
        }
















        private void InitializeForm()
        {
            LoadAvailabilityDropDownList();
            LoadConditionDropDownList();
            LoadCategoryDropDownList();
            Console.WriteLine("here");
        }

        private void LoadAvailabilityDropDownList()
        {
            ddlAvalability.DisplayMember = "Value";
            ddlAvalability.ValueMember = "Key";

            ddlAvalability.DataSource = new BindingSource(context.EquipmentAvailabilityStatuses.GetAllByName() , null) ;
        }

        private void LoadConditionDropDownList()
        {
            ddlCondition.DisplayMember = "Value";
            ddlCondition.ValueMember = "Key";

            ddlCondition.DataSource = new BindingSource(context.EquipmentConditionStatuses.GetAllByName(), null);
        }
        private void LoadCategoryDropDownList()
        {
            ddlCategory.DisplayMember = "Value";
            ddlCategory.ValueMember = "Key";

            ddlCategory.DataSource = new BindingSource(context.Categories.GetAllByName(), null);
        }
    }
}
