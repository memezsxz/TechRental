using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ViewModels
{
    public  class QuarterEarnings
    {
        public int Quarter { get; set; }
        public List<MonthlyEarnings> Data { get; set; } = new();
    }
}
