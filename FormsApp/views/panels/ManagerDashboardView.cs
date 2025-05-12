using System.Windows.Forms.DataVisualization.Charting;
using Database.Persistence;
using Image = System.Drawing.Image;
using Database.ViewModels;
using Database.Core;
using Amazon.S3.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FormsApp.views.panels
{
    /// <summary>
    /// Dashboard view control for managers, displaying key rental metrics and statistics.
    /// </summary>
    public partial class ManagerDashboardView : UserControl
    {
        #region Fields

        /// <summary>
        /// Unit of work for accessing the database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork = new UnitOfWork();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the dashboard view and prepares default labels.
        /// </summary>
        public ManagerDashboardView()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the form load event and initializes dashboard data.
        /// </summary>
        private async void admin_dashboard_Load(object sender, EventArgs e)
        {
            await RefreshData();
        }

        /// <summary>
        /// Handles refresh panel click to reload dashboard data.
        /// </summary>
        private async void pnlRefresh_Click(object sender, EventArgs e)
        {
            await RefreshData();
        }

        #endregion

        #region Data Loading

        /// <summary>
        /// Refreshes all charts and statistics on the dashboard.
        /// </summary>
        private async Task RefreshData()
        {
            await LoadStats();
        }

        /// <summary>
        /// Loads weekly statistics and updates UI labels.
        /// </summary>
        private async Task LoadStats()
        {
            WeeklyStats stats = await _unitOfWork.RentalRequests.GetWeeklyDashboardStatsAsync();

            // Update label controls with formatted stats
            lblTodaysPickups.Text = $"Today's Pickups: {stats.TodaysPickups}";
            lblTotalRentals.Text = $"Total Rentals: {stats.TotalRentals}";
            lblOngoing.Text = $"Ongoing Rentals: {stats.OngoingRentals}";
            lblCompleted.Text = $"Completed Rentals: {stats.CompletedRentals}";
            lblOverdue.Text = $"Overdue Rentals: {stats.OverdueRentals}";
            lblDamaged.Text = $"Damaged Equipment: {stats.DamagedReturns}";
        }

        #endregion
    }
}
