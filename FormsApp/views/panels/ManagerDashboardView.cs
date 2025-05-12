
using System.Windows.Forms.DataVisualization.Charting;
using Database.Persistence;
using Image = System.Drawing.Image;
using Database.ViewModels;
using Database.Core;
using Amazon.S3.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FormsApp.views.panels
{
    public partial class ManagerDashboardView : UserControl
    {
        #region Fields
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
        private void admin_dashboard_Load(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void pnlRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
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
        /// Loads stats and update the UI.
        /// </summary>
        private async Task LoadStats()
        {
            WeeklyStats stats = await _unitOfWork.RentalRequests.GetWeeklyDashboardStatsAsync();

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

