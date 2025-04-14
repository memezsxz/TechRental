using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsApp.views
{
    public partial class view_edit_inventory : Form
    {
        public view_edit_inventory()
        {
            InitializeComponent();
            Global.SizeAndCenterForm(this, 0.6f);
            CenterToScreen();
        }

        private void view_edit_inventory_Load(object sender, EventArgs e)
        {

        }
    }
}
