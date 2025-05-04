using System;
using System.Collections;
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
using Database.Core.Domain;
using Database.Persistence;
using Image = System.Drawing.Image;

namespace FormsApp.views.panels
{
    public partial class AdminDashboardView : UserControl
    {


        private RentalDBContext context = new RentalDBContext();

        private List<TopRentedEquipmentStats> top5EquipmentStats = new List<TopRentedEquipmentStats>();

        private int currentIndex = -1;
        public AdminDashboardView()
        {
            InitializeComponent();
        }

        private void admin_dashboard_Load(object sender, EventArgs e)
        {
            CreateCharts();
            LoadStats();
        }



        private async Task LoadStats()
        {
            var top5EquipmentIds = await context.RentalRecords
                .Where(r => r.RentalRequest != null && r.RentalRequest.EquipmentId != null)
                .GroupBy(r => r.RentalRequest.EquipmentId.Value)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => g.Key)
                .ToListAsync();

            top5EquipmentStats = await context.Equipment
                .Where(e => top5EquipmentIds.Contains(e.Id))
                .Select(e => new TopRentedEquipmentStats
                {
                    EquipmentId = e.Id,
                    Name = e.Name,

                    TotalRentals = e.RentalRequests
                        .SelectMany(rq => rq.RentalRecords)
                        .Count(),

                    TotalRevenue = e.RentalRequests
                        .SelectMany(rq => rq.RentalRecords)
                        .Sum(rr => (decimal?)rr.TotalCost) ?? 0,

                    AvgRentalDuration = e.RentalRequests
                        .SelectMany(rq => rq.RentalRecords)
                        .Any(rr => rr.PickupDate != null && rr.ActualReturnDate != null)
                        ? (float)(e.RentalRequests
                            .SelectMany(rq => rq.RentalRecords)
                            .Where(rr => rr.PickupDate != null && rr.ActualReturnDate != null)
                            .Average(rr => (int?)EF.Functions.DateDiffDay(rr.PickupDate, rr.ActualReturnDate.Value)) ?? 0)
                        : 0f,

                    Rating = e.Feedbacks
                        .Any(f => !f.IsHidden.HasValue || !f.IsHidden.Value)
                        ? (float)(e.Feedbacks
                            .Where(f => !f.IsHidden.HasValue || !f.IsHidden.Value)
                            .Average(f => (decimal?)f.Rate) ?? 0)
                        : 0f,
                    ImageGuid = e.Image.Guid ?? null

                })
                .ToListAsync();

            HandleTopEquipmentsSwitching(pnlNext, null);
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

        private void LoadCategoriesChart()
        {

        }

        private class TopRentedEquipmentStats
        {
            public int EquipmentId { get; set; }
            public string Name { get; set; } = "";
            public int TotalRentals { get; set; }
            public decimal TotalRevenue { get; set; }
            public float AvgRentalDuration { get; set; }
            public float Rating { get; set; }
            public Guid? ImageGuid { get; set; }
        }

        private async Task UpdateTopItemData()
        {
            if (!top5EquipmentStats.Any()) return;

            if (!top5EquipmentStats.Any() || currentIndex < 0 || currentIndex >= top5EquipmentStats.Count) return;

            TopRentedEquipmentStats stats = top5EquipmentStats[currentIndex];

            lblTopNumber.Text = $"#{currentIndex + 1}";
            lblTopName.Text = $"{stats.EquipmentId} - {stats.Name}";
            lblTopTotalRental.Text = stats.TotalRentals.ToString();
            lblTopTotalRevenue.Text = $"{Math.Round(stats.TotalRevenue, 3):C}";
            lblTopAvgDuration.Text = $"{(int)stats.AvgRentalDuration} days";
            lblTopRating.Text = $"{Math.Round(stats.Rating, 1):F1}";


            if (stats.ImageGuid.HasValue)
            {
                var image = await S3Uploader.GetImageByGuidAsync(stats.ImageGuid.Value.ToString());

                if (image != null)
                {
                    pnlTopImage.BackgroundImage = new Bitmap(image);
                    pnlTopImage.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            else
            {
                pnlTopImage.BackgroundImage = null;
            }
        }


        private void HandleTopEquipmentsSwitching(object sender, EventArgs e)
        {
            if (!top5EquipmentStats.Any()) return;

            if (sender == pnlNext)
            {
                currentIndex = (currentIndex + 1) % top5EquipmentStats.Count;
            }
            else if (sender == pnlPrevios)

            {
                currentIndex = (currentIndex - 1 + top5EquipmentStats.Count) % top5EquipmentStats.Count;
            }
            UpdateTopItemData();
        }

    }


}

