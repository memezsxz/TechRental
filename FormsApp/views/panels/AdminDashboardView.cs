using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization;
using Microsoft.EntityFrameworkCore;
using System.Windows.Forms.DataVisualization.Charting;
using Database.Persistence;

namespace FormsApp.views.panels
{
    public partial class AdminDashboardView : UserControl
    {
        public AdminDashboardView()
        {
            InitializeComponent();
        }

        private void admin_dashboard_Load(object sender, EventArgs e)
        {
            CreateCharts();
        }


        private void CreateCharts()
        {
            var startOfWeek = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek + 1); // Monday
            var endOfWeek = startOfWeek.AddDays(7);

            using (var context = new RentalDBContext())
            {
                var data = context.RentalRequests
                    .Where(r => r.StartDate >= startOfWeek && r.StartDate < endOfWeek)
                    .GroupBy(r => r.Equipment.Category.Name)
                    .Select(g => new
                    {
                        Category = g.Key,
                        Count = g.Count()
                    })
                    .ToList();

                chart1.Series.Clear();
                chart1.Titles.Clear();
                chart1.Titles.Add("This Week’s Sales");
                //MessageBox.Show($"Data Count: {data.Count}");

                Series series = new Series
                {
                    Name = "Sales",
                    IsValueShownAsLabel = true,
                    ChartType = SeriesChartType.Pie
                };
       

                chart1.Series.Add(series);
                chart1.Series["Sales"]["PieLabelStyle"] = "Outside";
                chart1.Series["Sales"]["PieLineColor"] = "Black";
                chart1.Legends[0].Enabled = true;
                foreach (var item in data)
                {
                    series.Points.AddXY(item.Category, item.Count);
                }
            }

        }
    }
}
