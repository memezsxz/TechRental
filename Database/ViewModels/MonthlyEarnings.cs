using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ViewModels
{
    /// <summary>
    /// Represents the total earnings for a specific month,
    /// typically used for reporting and dashboard visualizations.
    /// </summary>
    public class MonthlyEarnings
    {
        /// <summary>
        /// The name of the month (e.g., "January", "Feb", or "03").
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// The total earnings value for the specified month.
        /// </summary>
        public int Value { get; set; }
    }
}