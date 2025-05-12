using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ViewModels
{
    /// <summary>
    /// View model representing statistical data about a top-rented equipment item.
    /// Used for dashboard analytics and visual summaries.
    /// </summary>
    public class TopRentedEquipmentStats
    {
        /// <summary>
        /// The unique identifier of the equipment.
        /// </summary>
        public int EquipmentId { get; set; }

        /// <summary>
        /// The name or title of the equipment.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// The total number of times this equipment was rented.
        /// </summary>
        public int TotalRentals { get; set; }

        /// <summary>
        /// The total revenue generated from this equipment's rentals.
        /// </summary>
        public decimal TotalRevenue { get; set; }

        /// <summary>
        /// The average duration (in days) that this equipment was rented.
        /// </summary>
        public float AvgRentalDuration { get; set; }

        /// <summary>
        /// The average rating given to this equipment by customers.
        /// </summary>
        public float Rating { get; set; }

        /// <summary>
        /// The GUID used to locate the equipment's image (if available).
        /// </summary>
        public Guid? ImageGuid { get; set; }

        /// <summary>
        /// The MIME type of the equipment image (e.g., "image/jpeg", "image/png").
        /// </summary>
        public string ImageFormat { get; set; }
    }
}