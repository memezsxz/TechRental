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

        private Label lblNoSalesMessage;
        private Label lblNoQuarterMessage;

        private RentalDBContext context = new RentalDBContext();

        private List<TopRentedEquipmentStats> top5EquipmentStats = new List<TopRentedEquipmentStats>();

        int currentYear = DateTime.Now.Year;
        //int currentYear = 2021;

        private int currentTopIndex = -1;

        private int currentQuarter = (DateTime.Now.Month - 1) / 3 + 1;
        //private int currentQuarter = 4;
        private int currentSelectedQuarter = -1;

        private List<QuarterEarnings> QuarterData = new();

        private List<QuarterEarnings> GetQuarterData()
        {
            using var context = new RentalDBContext();

            var records = context.RentalRecords
                .Where(r => r.ActualReturnDate.HasValue &&
                            r.TotalCost.HasValue &&
                            r.ActualReturnDate.Value.Year == currentYear)
                .ToList();

            var earningsByMonth = records
                .GroupBy(r => r.ActualReturnDate.Value.Month)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(r => (int)r.TotalCost.Value)
                );

            var result = new List<QuarterEarnings>();

            for (int q = 1; q <= 4; q++)
            {
                var quarterData = new QuarterEarnings { Quarter = q };

                for (int i = 0; i < 3; i++)
                {
                    int month = (q - 1) * 3 + i + 1;
                    string label = new DateTime(currentYear, month, 1).ToString("MMM");
                    int value = earningsByMonth.TryGetValue(month, out var total) ? total : 0;

                    quarterData.Data.Add(new MonthlyEarnings { Month = label, Value = value });
                }

                result.Add(quarterData);
            }

            return result;
        }

        private void HandleQuarterEarningsSwitching(object sender, EventArgs e)
        {
            if (!QuarterData.Any()) return;

            if (sender == pnlQuarterNext)
            {
                currentSelectedQuarter = (currentSelectedQuarter + 1) % currentQuarter;
            }
            else if (sender == pnlQuarterPrevious)
            {
                currentSelectedQuarter = (currentSelectedQuarter - 1 + currentQuarter) % currentQuarter;
            }

            LoadQuarterData();
        }

        private void LoadQuarterData()
        {
            QuarterData = GetQuarterData();
            if (currentSelectedQuarter == -1)
                currentSelectedQuarter = currentQuarter > 0 ? 0 : -1;

            if (currentSelectedQuarter < 0 || currentSelectedQuarter >= QuarterData.Count) return;

            var quarter = QuarterData[currentSelectedQuarter];
            RenderBarChart(chartQuarterEarnings, lblNoQuarterMessage, $"Quarter {quarter.Quarter} Earnings", quarter.Data);
        }

        public AdminDashboardView()
        {
            InitializeComponent();
            lblNoSalesMessage = new Label
            {
                Text = "No Sales for the week",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.Gray,
                Visible = false
            };
            // Add it to the same parent container as the chart
            chartCategories.Parent.Controls.Add(lblNoSalesMessage);
            lblNoSalesMessage.BringToFront(); // ensure it's not hidden behind anything


            lblNoQuarterMessage = new Label
            {
                Text = "No earnings for this quarter",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.Gray,
                Visible = false
            };
            // Add it to the same parent container as the chart
            chartQuarterEarnings.Parent.Controls.Add(lblNoQuarterMessage);
            chartQuarterEarnings.BringToFront(); // ensure it's not hidden behind anything


        }

        private void admin_dashboard_Load(object sender, EventArgs e)
        {
            RefreshData();
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

            HandleTopEquipmentsSwitching(pnlTopNext, null);
        }
        private void LoadCategoriesChart()
        {

            var data = GetWeeklyCategoryRentalData();
            RenderPieChart(chartCategories, lblNoSalesMessage, "This Week's Rentals", data);

        }


        private List<(string Label, int Value)> GetWeeklyCategoryRentalData()
        {
            var startOfWeek = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek); // Sunday
            var endOfWeek = startOfWeek.AddDays(7); // Saturday (inclusive)

            using var context = new RentalDBContext();

            var data = context.RentalRecords
                .Where(rr =>
                        rr.RentalRequest != null &&
                        rr.RentalRequest.Equipment != null &&
                        rr.RentalRequest.Equipment.Category != null
                //&& rr.PickupDate >= startOfWeek && rr.PickupDate < endOfWeek // TODO Maryam: ask Ruqaya to add data for the current week starting Sunday
                )
                .GroupBy(rr => rr.RentalRequest.Equipment.Category.Name)
                .Select(g => new
                {
                    Label = g.Key,
                    Value = g.Count()
                })
                .OrderByDescending(g => g.Value)
                .AsEnumerable()
                .Select(g => (g.Label, g.Value))
                .ToList();

            return data;
        }

        private void RefreshData()
        {
            _ = LoadStats();
            LoadCategoriesChart();

            LoadQuarterData();
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

            if (!top5EquipmentStats.Any() || currentTopIndex < 0 || currentTopIndex >= top5EquipmentStats.Count) return;

            TopRentedEquipmentStats stats = top5EquipmentStats[currentTopIndex];

            lblTopNumber.Text = $"#{currentTopIndex + 1}";
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

            if (sender == pnlTopNext)
            {
                currentTopIndex = (currentTopIndex + 1) % top5EquipmentStats.Count;
            }
            else if (sender == pnlTopPrevios)

            {
                currentTopIndex = (currentTopIndex - 1 + top5EquipmentStats.Count) % top5EquipmentStats.Count;
            }
            UpdateTopItemData();
        }

        private void RenderPieChart(
            Chart chart,
            Label noDataLabel,
            string chartTitle,
            List<(string Label, int Value)> data,
            string seriesName = "Data",
            int categoryLimit = 10)
        {
            chart.Series.Clear();
            chart.Titles.Clear();
            chart.Visible = true;
            noDataLabel.Visible = false;

            if (!data.Any())
            {
                chart.Visible = false;
                noDataLabel.Text = "No data available";
                noDataLabel.Visible = true;
                return;
            }

            var top = data.Take(categoryLimit - 1).ToList();
            var others = data.Skip(categoryLimit - 1).ToList();
            if (others.Any())
            {
                top.Add(("Others", others.Sum(x => x.Value)));
            }

            var series = new Series
            {
                Name = seriesName,
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Pie
            };

            chart.Series.Add(series);
            chart.Series[seriesName]["PieLabelStyle"] = "Outside";
            chart.Series[seriesName]["PieLineColor"] = "Black";
            chart.Legends[0].Enabled = true;
            chart.Titles.Add(chartTitle);

            foreach (var item in top)
            {
                series.Points.AddXY(item.Label, item.Value);
            }

            chart.Invalidate();
        }
        private void RenderBarChart(
            Chart chart,
            Label noDataLabel,
            string title,
            List<MonthlyEarnings> data)
        {
            chart.Series.Clear();
            chart.Titles.Clear();
            chart.ChartAreas.Clear();
            chart.Visible = true;
            noDataLabel.Visible = false;

            if (!data.Any())
            {
                chart.Visible = false;
                noDataLabel.Text = "No earnings for this quarter";
                noDataLabel.Visible = true;
                return;
            }

            var chartArea = new ChartArea("MainArea");
            chart.ChartAreas.Add(chartArea);
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.Title = "Month";
            chartArea.AxisY.Title = "Earnings";
            chartArea.AxisY.Minimum = 0;

            chart.Titles.Add(title);
            chart.Legends.Clear();

            chart.Series.Clear();

            Series series = new Series("Quarter Earnings")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true,
                IsXValueIndexed = true
            };

            foreach (MonthlyEarnings me in data)
            {
                var point = new DataPoint();
                point.AxisLabel = me.Month;
                //point.YValues = new double[] { value };
                series.Points.Add(me.Value);
            }

            chart.Series.Add(series);

            chart.Invalidate();
        }

        private void pnlRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private class MonthlyEarnings
        {
            public string Month { get; set; }
            public int Value { get; set; }
        }

        private class QuarterEarnings
        {
            public int Quarter { get; set; }
            public List<MonthlyEarnings> Data { get; set; } = new();
        }

    }


}

