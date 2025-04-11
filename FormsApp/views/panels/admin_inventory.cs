using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Database.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using Database;
using Database.Core.Domain;
using Database.Core.Repositories;
using System.Xml.Linq;
using FormsApp.views.controls;

namespace FormsApp.views.panels
{
    public partial class admin_inventory : UserControl
    {
        private UnitOfWork _context = new UnitOfWork(new RentalDBContext());
        private int totalPages = 0;

        private ISearch currentControl;
        private Type currentType = typeof(Equipment);

        public admin_inventory()
        {
            InitializeComponent();
            this.Paint += Global.Panel_Paint;
        }

        private void admin_inventory_Load(object sender, EventArgs e)
        {
            cbRecordsNum.DataSource = new BindingList<int>() { 10, 20, 30 };
            cbRecordsNum.SelectedIndex = 0;
            LoadColumnDropdown();

            AddUnboundColumn("Delete", "Delete", Color.Red);
            AddUnboundColumn("Edit", "Edit", Color.Blue);
        }

        private void LoadColumnDropdown()
        {
            Dictionary<string, string> columnInfo = new() { { "None", "" } };

            // Dynamically get repository for currentType
            object repo = Global.GetRepositoryForType(currentType);

            if (repo == null)
            {
                Console.WriteLine($"No repository found for type {currentType.Name}.");
                return;
            }

            // Look for GetEntityColumnsWithTypes method
            MethodInfo getColumnsMethod = repo.GetType().GetMethod("GetEntityColumnsWithTypes");
            if (getColumnsMethod != null)
            {
                var result = getColumnsMethod.Invoke(repo, null);
                if (result is Dictionary<string, string> columns)
                {
                    columnInfo = columnInfo
                        .Concat(columns)
                        .Where(kv => kv.Key != "Description")
                        .ToDictionary(kv => kv.Key, kv => kv.Value);

                    columnInfo = FormatColumnNames(columnInfo);
                    //Console.WriteLine(columnInfo);
                    cbColumn.DisplayMember = "Key";
                    cbColumn.ValueMember = "Value";
                    cbColumn.DataSource = new BindingSource(columnInfo, null);
                    cbColumn.SelectedIndex = 0;

                    dropdownColumns_SelectedIndexChanged(cbColumn, EventArgs.Empty);
                }
            }
            else
            {
                Console.WriteLine($"Repository for {currentType.Name} does not implement GetEntityColumnsWithTypes.");
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            currentControl.PageNumber = 1;
            currentControl.Apply();
        }



        private void btnReset_Click(object sender, EventArgs e)
        {
            cbColumn.SelectedIndex = 0;
        }

        private void dgvEquipment_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            new view_edit_inventory().ShowDialog();
        }


        private Dictionary<string, string> FormatColumnNames(Dictionary<string, string> columns)
        {
            Dictionary<string, string> newColumns = new Dictionary<string, string>();

            foreach (var column in columns)
            {
                string formattedKey = SplitPascalCase(column.Key);

                if (column.Key == "Id")
                {
                    formattedKey = "Id";
                }
                else if (column.Key.EndsWith("Id"))
                {
                    formattedKey = formattedKey.Replace(" Id", "");
                }

                newColumns.Add(formattedKey, column.Value);
            }

            return newColumns;
        }

        private void HandelRefDropDown(string entityName, ComboBox dropdown)
        {
            var property = typeof(UnitOfWork).GetProperties()
                .FirstOrDefault(p => p.Name.Equals(entityName, StringComparison.OrdinalIgnoreCase));

            if (property?.GetValue(_context) is IStatus statusRepository)
            {
                dropdown.DataSource = new BindingSource(statusRepository.GetAllByName(), null);
                dropdown.DisplayMember = "Value";
                dropdown.ValueMember = "Key";
            }
            else
            {
                flpSearch.Controls.RemoveAt(flpSearch.Controls.Count - 1);
                flpSearch.Controls.Add(new Panel());
                Console.WriteLine($"Repository '{entityName}' not found or does not implement IStatus.");
            }
        }

        private string SplitPascalCase(string input)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
                Regex.Replace(input, "([a-z])([A-Z])", "$1 $2"));
        }


        private void dropdownColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            Control control = CreateControlForType(cbColumn.SelectedValue?.ToString(), GetSelectedColumnName());

            if (control == null) return;

            //control.MinimumSize = new Size(300, 34);
            control.Dock = DockStyle.Fill;

            currentControl = control as ISearch;
            if (cbColumn.SelectedIndex == 0) currentControl.Apply();
            flpSearch.Controls.RemoveAt(flpSearch.Controls.Count - 1);
            flpSearch.Controls.Add(control);
        }

        private string GetSelectedColumnName() =>
            ((KeyValuePair<string, string>)cbColumn.SelectedItem).Key.Replace(" ", "");

        private Control CreateControlForType(string type, string col)
        {
            //if (cbColumn.SelectedIndex == 0 || string.IsNullOrEmpty(type))
            //    return ;

            ISearch control = type switch
            {
                "" => new GetAllControl(currentType),
                "DateTime" => new NumericFilterControl(currentType, col, typeof(DateTime)),
                "Decimal" => new NumericFilterControl(currentType, col, typeof(decimal)),
                "Int32" => new NumericFilterControl(currentType, col, typeof(int)),
                "Boolean" => new TextStatusFilterControl(currentType, TextStatusFilterControl.FilterType.Boolean,
                    col),
                "String" => new TextStatusFilterControl(currentType, TextStatusFilterControl.FilterType.String,
                    col),
                _ => new TextStatusFilterControl(currentType, TextStatusFilterControl.FilterType.Status,
                    col)
            };

            // Set page size if the control supports it

            control.PageSize = (int)cbRecordsNum.SelectedItem;
            control.OnSearchCompleted += HandleSearchResults;

            return control as UserControl;
        }

        private void HandleSearchResults(object result)
        {
            if (result == null) return;

            var resultType = result.GetType();

            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(PaginatedResult<>))
            {
                Type entityType = resultType.GetGenericArguments()[0];

                MethodInfo method = typeof(admin_inventory)
                    .GetMethod("UpdatePaginatedData", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                MethodInfo generic = method.MakeGenericMethod(entityType);
                generic.Invoke(this, new object[] { result });
            }
            else
            {
                Console.WriteLine("Invalid search result type.");
            }
        }

#pragma warning disable IDE0051 // Remove unused private members

        [UsedViaReflection]
        private void UpdatePaginatedData<T>(PaginatedResult<T> val) where T : class
        {
            totalPages = val.TotalPages;
            lblTotal.Text = $"Total Records: {val.TotalRecords}";
            lblPagPage.Text = $"Page {currentControl.PageNumber} of {totalPages}";
            dgvEquipment.DataSource = val.Data;
        }
#pragma warning restore IDE0051

        private Control CreateReferenceDropdown(string col, string entityName)
        {


            var dropdown = new ComboBox { Name = $"{col}_{entityName}", DropDownStyle = ComboBoxStyle.DropDownList };
            HandelRefDropDown(entityName, dropdown);
            return dropdown;
        }

        private void cbRecordsNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Console.WriteLine("value : " + (int)cbRecordsNum.SelectedValue);
            //Console.WriteLine("item : " + (int)cbRecordsNum.SelectedItem);
            if (currentControl != null)
            {
                currentControl.PageSize = (int)cbRecordsNum.SelectedItem;

                btnApply_Click(cbColumn, EventArgs.Empty);
            }
           
        }

        private void page_toggle_Click(object sender, EventArgs e)
        {
            if (sender is not Panel panel) return;

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


        private void AddUnboundColumn(string name, string value, Color textColor)
        {
            if (!dgvEquipment.Columns.Contains(name))
            {
                DataGridViewTextBoxColumn newColumn = new DataGridViewTextBoxColumn
                {
                    Name = name,
                    HeaderText = name,
                    ReadOnly = true
                };

                dgvEquipment.Columns.Insert(0, newColumn);
            }

            dgvEquipment.CellFormatting += (sender, e) =>
            {
                if (dgvEquipment.Columns[e.ColumnIndex].Name == name)
                {
                    e.Value = value; // Ensure the value is always set
                    e.CellStyle.ForeColor = textColor; // Apply color
                    e.CellStyle.Font = new Font(dgvEquipment.DefaultCellStyle.Font, FontStyle.Bold); // Optional bold
                    e.FormattingApplied = true;
                }
            };

        }
    }
}
