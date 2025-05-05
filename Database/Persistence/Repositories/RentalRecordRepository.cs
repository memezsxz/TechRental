using Database.Core.Domain;
using Database.Core.Repositories;
using Database.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Database.Persistence.Repositories
{
    internal class RentalRecordRepository : Repository<RentalRecord>, IRentalRecordRepository
    {
        public RentalRecordRepository(RentalDBContext context) : base(context)
        {
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }
        public async Task<List<QuarterEarnings>> GetQuarterEarningsByYearAsync(int year)
        {
            var records = await RentalDBContext.RentalRecords
                .Where(r => r.ActualReturnDate.HasValue && r.TotalCost.HasValue && r.ActualReturnDate.Value.Year == year)
                .ToListAsync();

            var earningsByMonth = records
                .GroupBy(r => r.ActualReturnDate.Value.Month)
                .ToDictionary(g => g.Key, g => g.Sum(r => (int)r.TotalCost.Value));

            var result = new List<QuarterEarnings>();

            for (int q = 1; q <= 4; q++)
            {
                var quarterData = new QuarterEarnings { Quarter = q };

                for (int i = 0; i < 3; i++)
                {
                    int month = (q - 1) * 3 + i + 1;
                    string label = new DateTime(year, month, 1).ToString("MMM");
                    int value = earningsByMonth.TryGetValue(month, out var total) ? total : 0;

                    quarterData.Data.Add(new MonthlyEarnings { Month = label, Value = value });
                }

                result.Add(quarterData);
            }

            return result;
        }

        public async Task<Dictionary<string, int>> GetWeeklyCategoryRentalDataAsync(int categoryLimit)
        {
            var startOfWeek = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);

            var rawData = await RentalDBContext.RentalRecords
                .Where(rr =>
                    rr.RentalRequest != null &&
                    rr.RentalRequest.Equipment != null &&
                    rr.RentalRequest.Equipment.Category != null)
                .GroupBy(rr => rr.RentalRequest.Equipment.Category.Name)
                .Select(g => new { Label = g.Key, Value = g.Count() })
                .OrderByDescending(g => g.Value)
                .ToListAsync();

            var top = rawData.Take(categoryLimit - 1).ToList();
            var others = rawData.Skip(categoryLimit - 1).ToList();

            if (others.Any())
            {
                top.Add(new { Label = "Others", Value = others.Sum(x => x.Value) });
            }

            return top.ToDictionary(x => x.Label, x => x.Value);
        }
    }
}
