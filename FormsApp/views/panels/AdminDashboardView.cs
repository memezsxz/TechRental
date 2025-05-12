
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

        /// <summary>
        /// Label displayed when there are no weekly sales to show in the category chart.
        /// </summary>
        private Label lblNoSalesMessage;

        /// <summary>
        /// Label displayed when there is no data available for the selected quarter.
        /// </summary>
        private Label lblNoQuarterMessage;

        /// <summary>
        /// Label displayed when an image fails to load in the top equipment section.
        /// </summary>
        private Label lblImageMessage;

        /// <summary>
        /// Unit of Work instance used for database access and retrieval of dashboard data.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork = new UnitOfWork();

        /// <summary>
        /// Holds statistics for the top 5 most rented equipment items.
        /// This data is displayed in the top equipment section of the dashboard.
        /// </summary>
        private List<TopRentedEquipmentStats> top5EquipmentStats = new List<TopRentedEquipmentStats>();

        /// <summary>
        /// The current year, used to filter and display statistics related to that year.
        /// </summary>
        private int currentYear = DateTime.Now.Year;
        // private int currentYear = 2021; // Use for static testing purposes if needed

        /// <summary>
        /// The index of the currently displayed top rented equipment item in the carousel.
        /// </summary>
        private int currentTopIndex = -1;

        /// <summary>
        /// The current quarter of the year, derived from the current date.
        /// Used to determine how many quarters of data are available.
        /// </summary>
        private int currentQuarter = (DateTime.Now.Month - 1) / 3 + 1;
        // private int currentQuarter = 4; // Use for static testing purposes if needed

        /// <summary>
        /// Tracks the quarter currently selected for earnings chart display.
        /// -1 indicates no quarter selected yet.
        /// </summary>
        private int currentSelectedQuarter = -1;

        /// <summary>
        /// Stores earnings data grouped by quarter for the current year.
        /// Used to populate the earnings bar chart.
        /// </summary>
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
        /// Configures the initial state of the provided chart by clearing previous content,
        /// setting visibility, and optionally displaying a fallback message if no data is available.
        /// </summary>
        /// <param name="chart">The chart control to be configured.</param>
        /// <param name="noDataLabel">The label to show when no data is available.</param>
        /// <param name="title">The title of the chart.</param>
        /// <param name="hasData">Indicates whether data exists to populate the chart.</param>
        /// <param name="emptyMessage">Optional message shown if no data exists.</param>
        /// <returns>True if data is available and the chart should be rendered; false otherwise.</returns>
        private bool PrepareChart(
            Chart chart,
            Label noDataLabel,
            string title,
            bool hasData,
            string emptyMessage = "No data available")
        {
            // Clear existing content
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Titles.Clear();
            chart.Legends.Clear();

            // Toggle visibility based on whether we have data
            chart.Visible = hasData;
            noDataLabel.Visible = !hasData;

            if (!hasData)
            {
                noDataLabel.Text = emptyMessage;
                return false;
            }

            // Add a title and default legend if data is present
            chart.Titles.Add(title);
            chart.Legends.Add(new Legend());
            return true;
        }

        /// <summary>
        /// Renders a pie chart showing category-wise distribution of rentals.
        /// </summary>
        /// <param name="chart">The chart control where the pie chart will be rendered.</param>
        /// <param name="noDataLabel">Label shown if no data is available.</param>
        /// <param name="chartTitle">Title to be displayed on the chart.</param>
        /// <param name="data">Dictionary containing category names and their rental counts.</param>
        /// <param name="seriesName">Name of the data series (defaults to "Data").</param>
        private void RenderPieChart(
            Chart chart,
            Label noDataLabel,
            string chartTitle,
            Dictionary<string, int> data,
            string seriesName = "Data")
        {
            // Skip rendering if no data
            if (!PrepareChart(chart, noDataLabel, chartTitle, data.Any()))
                return;

            // Create a new chart area specifically for pie chart
            var chartArea = new ChartArea("PieArea");
            chart.ChartAreas.Add(chartArea);

            // Create and configure the data series
            var series = new Series(seriesName)
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true
            };

            // Improve readability of pie labels
            series["PieLabelStyle"] = "Outside";
            series["PieLineColor"] = "Black";

            // Add each data point to the pie chart
            foreach (var (label, value) in data)
            {
                series.Points.AddXY(label, value);
            }

            // Add the series to the chart and refresh
            chart.Series.Add(series);
            chart.Invalidate();
        }

        /// <summary>
        /// Renders a bar chart displaying earnings per month for a given quarter.
        /// </summary>
        /// <param name="chart">The chart control where the bar chart will be displayed.</param>
        /// <param name="noDataLabel">Label to show if no earnings data is available.</param>
        /// <param name="title">Title of the chart (e.g., "Quarter 1 Earnings").</param>
        /// <param name="data">A list of monthly earnings records.</param>
        private void RenderBarChart(
            Chart chart,
            Label noDataLabel,
            string title,
            List<MonthlyEarnings> data)
        {
            // Skip rendering if no data
            if (!PrepareChart(chart, noDataLabel, title, data.Any(), "No earnings for this quarter"))
                return;

            // Remove existing legends for a clean look
            chart.Legends.Clear();

            // Create and configure the chart area
            var chartArea = new ChartArea("MainArea")
            {
                AxisX =
                {
                    Title = "Month",
                    Interval = 1,
                    MajorGrid = { Enabled = false }
                },
                AxisY =
                {
                    Title = "Earnings",
                    Minimum = 0,
                    MajorGrid = { LineColor = Color.LightGray }
                }
            };
            chart.ChartAreas.Add(chartArea);

            // Create and configure the data series
            var series = new Series("Quarter Earnings")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true,
                IsXValueIndexed = true
            };

            // Add data points to the series
            foreach (var entry in data)
            {
                var point = new DataPoint
                {
                    AxisLabel = entry.Month,
                    YValues = new[] { (double)entry.Value }
                };
                series.Points.Add(point);
            }

            // Add series to chart and refresh
            chart.Series.Add(series);
            chart.Invalidate();
        }

        #endregion
    }
}

