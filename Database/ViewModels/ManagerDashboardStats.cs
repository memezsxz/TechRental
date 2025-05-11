using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ViewModels
{
    public class ManagerDashboardStats
    {
        public int TodaysPickups { get; set; }
        public int TotalRentals { get; set; }
        public int OngoingRentals { get; set; }
        public int CompletedRentals { get; set; }
        public int OverdueRentals { get; set; }
        public int DamagedReturns { get; set; }

    }
}
