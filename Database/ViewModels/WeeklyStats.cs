using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ViewModels
{
    /// <summary>
    /// View model representing aggregated weekly rental statistics for dashboard summaries.
    /// </summary>
    public class WeeklyStats
    {
        /// <summary>
        /// The number of pickups scheduled or completed today.
        /// </summary>
        public int TodaysPickups { get; set; }

        /// <summary>
        /// The total number of rentals recorded in the system.
        /// </summary>
        public int TotalRentals { get; set; }

        /// <summary>
        /// The number of currently active (ongoing) rentals.
        /// </summary>
        public int OngoingRentals { get; set; }

        /// <summary>
        /// The number of rentals that were successfully completed.
        /// </summary>
        public int CompletedRentals { get; set; }

        /// <summary>
        /// The number of rentals that are overdue for return.
        /// </summary>
        public int OverdueRentals { get; set; }

        /// <summary>
        /// The number of returned rentals that were marked as damaged.
        /// </summary>
        public int DamagedReturns { get; set; }
    }
}