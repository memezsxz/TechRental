using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ViewModels
{
    /// <summary>
    /// Represents aggregated earnings for a specific quarter of the year.
    /// Contains a list of monthly earnings data used for reporting and charting.
    /// </summary>
    public class QuarterEarnings
    {
        /// <summary>
        /// The quarter number (e.g., 1 for Q1, 2 for Q2, etc.).
        /// </summary>
        public int Quarter { get; set; }

        /// <summary>
        /// A list of <see cref="MonthlyEarnings"/> objects representing earnings per month in this quarter.
        /// </summary>
        public List<MonthlyEarnings> Data { get; set; } = new();
    }
}