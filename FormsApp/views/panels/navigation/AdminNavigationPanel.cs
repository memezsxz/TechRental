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
    public partial class AdminNavigationPanel : UserControl
    {
        private Panel view;
        public AdminNavigationPanel(Panel view)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.view = view;
            AttachClickEvent();

            NavigationItem_Click(lblDashboard, EventArgs.Empty);
        }

        void AttachClickEvent()
        {
            foreach (Control control in this.pnlPanel.Controls)
            {
                if (control is TableLayoutPanel panel)
                {
                    foreach (Control child in panel.Controls)
                    {
                        if (child == pnlIcon && child == lblBrand) continue;

                        Console.WriteLine($"{child.Name} : {child.GetType()}");

                        child.Click += NavigationItem_Click;
                    }
                }
            }
        }

        private UserControl? GetUserControlByPanelName(string panelName)
        {
            return panelName switch
            {
                "tlpDashboard" => new AdminDashboardView(),
                "tlpManage" => new AdminManagementView(),
                "tlpReports" => new AdminReportsView(),
                "tlpProfile" => new AdminProfileView(),
                _ => null
            };
        }

        private void NavigationItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine(sender.GetType());

            Panel clickedPanel = null;
            Label textLabel = null;

            if (sender is Label label)
            {
                textLabel = label;
                clickedPanel = label.Parent as Panel;
            }

            else if (sender is Panel icon && icon.Parent is Panel parentPanel)
            {
                clickedPanel = parentPanel;

                foreach (Control control in clickedPanel.Controls)
                {
                    if (control is not Label lbl) continue;

                    textLabel = lbl;
                    break;
                }
            }


            if (clickedPanel == null || textLabel == null) return;
            ResetAllLabelsToRegularFont();
            textLabel.Font = new Font(textLabel.Font.FontFamily, textLabel.Font.Size, FontStyle.Bold);

            UserControl? newView = GetUserControlByPanelName(clickedPanel.Name);

            if (newView != null) FillView(newView);
        }


        void FillView(UserControl panel)
        {
            panel.Dock = DockStyle.Fill;
            view.Controls.Clear();
            view.Controls.Add(panel);
        }


        private void ResetAllLabelsToRegularFont()
        {
            foreach (Control control in this.pnlPanel.Controls)
            {
                if (control is TableLayoutPanel panel)
                {
                    foreach (Control child in panel.Controls)
                    {

                        if (child is Label label && child != lblBrand)
                        {
                            label.Font = new Font(label.Font.FontFamily, label.Font.Size, FontStyle.Regular);
                        }
                    }
                }
            }
        }



    }

}
