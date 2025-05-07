using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ViewModels
{
    public class TopRentedEquipmentStats
    {
        public int EquipmentId { get; set; }
        public string Name { get; set; } = "";
        public int TotalRentals { get; set; }
        public decimal TotalRevenue { get; set; }
        public float AvgRentalDuration { get; set; }
        public float Rating { get; set; }
        public Guid? ImageGuid { get; set; }
        public string ImageFormat { get; set; }
    }
}
