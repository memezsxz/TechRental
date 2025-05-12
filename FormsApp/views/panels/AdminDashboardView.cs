
using System.Windows.Forms.DataVisualization.Charting;
using Database.Persistence;
using Image = System.Drawing.Image;
using Database.ViewModels;
using Database.Core;
using Amazon.S3.Model;

namespace FormsApp.views.panels
{
    public partial class AdminDashboardView : UserControl
    {
        #region Fields
        private Label lblNoSalesMessage;
        private Label lblNoQuarterMessage;
        private Label lblImageMessage;

        private readonly IUnitOfWork _unitOfWork = new UnitOfWork();

        private List<TopRentedEquipmentStats> top5EquipmentStats = new List<TopRentedEquipmentStats>();

        int currentYear = DateTime.Now.Year;
        //int currentYear = 2021;

        private int currentTopIndex = -1;

        private int currentQuarter = (DateTime.Now.Month - 1) / 3 + 1;
        //private int currentQuarter = 4;
        private int currentSelectedQuarter = -1;

        private List<QuarterEarnings> QuarterData = new();
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes the dashboard view and prepares default labels.
        /// </summary>
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

            lblImageMessage = new Label
            {
                Text = "Could not load Image",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.Gray,
                Visible = false
            };

            pnlTopImage.Controls.Add(lblImageMessage);
            lblImageMessage.BringToFront();
        }

        #endregion

        #region Event Handlers
        private void admin_dashboard_Load(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void pnlRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        /// <summary>
        /// Handles user toggling between top rented equipment entries.
        /// </summary>
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

        /// <summary>
        /// Handles user toggling between quarter earnings charts.
        /// </summary>
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
        #endregion



        /// <summary>
        /// Loads stats and update the UI.
        /// </summary>
        private async Task LoadWeeklyStats()
        {
            WeeklyStats stats = await _unitOfWork.RentalRequests.GetWeeklyDashboardStatsAsync();

            lblTodaysPickups.Text = $"Today's Pickups: {stats.TodaysPickups}";
            lblTotalRentals.Text = $"Total Rentals: {stats.TotalRentals}";
            lblOngoing.Text = $"Ongoing Rentals: {stats.OngoingRentals}";
            lblCompleted.Text = $"Completed Rentals: {stats.CompletedRentals}";
            lblOverdue.Text = $"Overdue Rentals: {stats.OverdueRentals}";
            lblDamaged.Text = $"Damaged Equipment: {stats.DamagedReturns}";
        }


        #region Data Loading

        /// <summary>
        /// Refreshes all charts and statistics on the dashboard.
        /// </summary>
        private async Task RefreshData()
        {
            await LoadStats();
            await LoadCategoriesChart();
            await LoadWeeklyStats();
            await LoadQuarterData();
        }

        /// <summary>
        /// Loads top 5 rented equipment statistics and updates the view.
        /// </summary>
        private async Task LoadStats()
        {

            top5EquipmentStats = await _unitOfWork.Equipment.GetTop5RentedEquipmentStatsAsync();
            currentTopIndex = -1;
            HandleTopEquipmentsSwitching(pnlTopNext, null);
        }

        /// <summary>
        /// Loads pie chart data for weekly category-based rentals and updates the chart.
        /// </summary>
        private async Task LoadCategoriesChart()
        {
            var data = await _unitOfWork.RentalRecords.GetWeeklyCategoryRentalDataAsync();
            RenderPieChart(chartCategories, lblNoSalesMessage, "This Week's Rentals", data);
        }

        /// <summary>
        /// Loads earnings per quarter and updates the bar chart accordingly.
        /// </summary>
        private async Task LoadQuarterData()
        {
            QuarterData = await _unitOfWork.RentalRecords.GetQuarterEarningsByYearAsync(currentYear);
            if (currentSelectedQuarter == -1)
                currentSelectedQuarter = currentQuarter > 0 ? 0 : -1;

            if (currentSelectedQuarter < 0 || currentSelectedQuarter >= QuarterData.Count) return;

            var quarter = QuarterData[currentSelectedQuarter];
            RenderBarChart(chartQuarterEarnings, lblNoQuarterMessage, $"Quarter {quarter.Quarter} Earnings", quarter.Data);
        }
        #endregion

        #region UI Updates

        /// <summary>
        /// Updates the detail view for the currently selected top equipment item.
        /// </summary>
        private async Task UpdateTopItemData()
        {
            if (!top5EquipmentStats.Any()) return;

            if (!top5EquipmentStats.Any() || currentTopIndex < 0 || currentTopIndex >= top5EquipmentStats.Count) return;

            TopRentedEquipmentStats stats = top5EquipmentStats[currentTopIndex];

            UpdateTopItemLabels(stats);

            if (stats.ImageGuid.HasValue && !string.IsNullOrWhiteSpace(stats.ImageFormat))
                await LoadAndDisplayTopItemImage(stats);
            else
                ClearTopItemImage();
        }

        /// <summary>
        /// Updates the textual labels for the selected equipment item.
        /// </summary>
        private void UpdateTopItemLabels(TopRentedEquipmentStats stats)
        {
            lblTopNumber.Text = $"#{currentTopIndex + 1}";
            lblTopName.Text = $"{stats.EquipmentId} - {stats.Name}";
            lblTopTotalRental.Text = stats.TotalRentals.ToString();
            lblTopTotalRevenue.Text = $"{Math.Round(stats.TotalRevenue, 3):C}";
            lblTopAvgDuration.Text = $"{(int)stats.AvgRentalDuration} days";
            lblTopRating.Text = $"{Math.Round(stats.Rating, 1):F1}";
        }


        /// <summary>
        /// Asynchronously loads and displays the image of the selected equipment item.
        /// </summary>
        private async Task LoadAndDisplayTopItemImage(TopRentedEquipmentStats stats)
        {
            Global.LoadImage(stats.ImageGuid, stats.ImageFormat, pnlTopImage, lblImageMessage);

            lblImageMessage.Visible = pnlTopImage.BackgroundImage == null;
        }



        /// <summary>
        /// Clears the image preview area if no valid image is available.
        /// </summary>
        private void ClearTopItemImage()
        {
            pnlTopImage.Controls.Clear();
            pnlTopImage.BackgroundImage = null;
        }
        #endregion

        #region Chart Rendering

        /// <summary>
        /// Prepares the base chart configuration and handles empty data fallback.
        /// </summary>
        private bool PrepareChart(
            Chart chart,
            Label noDataLabel,
            string title,
            bool hasData,
            string emptyMessage = "No data available")
        {
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Titles.Clear();
            chart.Legends.Clear();
            chart.Visible = hasData;
            noDataLabel.Visible = !hasData;

            if (!hasData)
            {
                noDataLabel.Text = emptyMessage;
                return false;
            }

            chart.Titles.Add(title);
            chart.Legends.Add(new Legend());
            return true;
        }

        /// <summary>
        /// Renders a pie chart using category-count data.
        /// </summary>
        private void RenderPieChart(
            Chart chart,
            Label noDataLabel,
            string chartTitle,
            Dictionary<string, int> data,
            string seriesName = "Data")
        {
            if (!PrepareChart(chart, noDataLabel, chartTitle, data.Any()))
                return;

            var chartArea = new ChartArea("PieArea");
            chart.ChartAreas.Add(chartArea);

            var series = new Series(seriesName)
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true
            };

            series["PieLabelStyle"] = "Outside";
            series["PieLineColor"] = "Black";

            foreach (var (label, value) in data)
            {
                series.Points.AddXY(label, value);
            }

            chart.Series.Add(series);
            chart.Invalidate();
        }

        /// <summary>
        /// Renders a bar chart representing earnings per month.
        /// </summary>
        private void RenderBarChart(
            Chart chart,
            Label noDataLabel,
            string title,
            List<MonthlyEarnings> data)
        {
            if (!PrepareChart(chart, noDataLabel, title, data.Any(), "No earnings for this quarter"))
                return;

            chart.Legends.Clear();

            var chartArea = new ChartArea("MainArea");
            chartArea.AxisX.Title = "Month";
            chartArea.AxisY.Title = "Earnings";
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.Minimum = 0;

            chart.ChartAreas.Add(chartArea);

            var series = new Series("Quarter Earnings")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true,
                IsXValueIndexed = true
            };

            foreach (var entry in data)
            {
                var point = new DataPoint
                {
                    AxisLabel = entry.Month,
                    YValues = new[] { (double)entry.Value }
                };
                series.Points.Add(point);
            }

            chart.Series.Add(series);
            chart.Invalidate();
        }

        #endregion
    }
}

