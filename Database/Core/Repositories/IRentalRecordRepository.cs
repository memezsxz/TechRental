using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.ViewModels;

namespace Database.Core.Repositories
{
    public interface IRentalRecordRepository : IRepository<RentalRecord>, IToBeTracked
    {
        Task<List<QuarterEarnings>> GetQuarterEarningsByYearAsync(int year);
        Task<Dictionary<string, int>> GetWeeklyCategoryRentalDataAsync(int categoryLimit = 10);
        public RentalRecord GetWithDetails(int id);
        public RentalRecord GetWithDetailsByRentalRequest(int id);

    }
}
