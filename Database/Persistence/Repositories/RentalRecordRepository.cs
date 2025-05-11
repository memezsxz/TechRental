using Database.Core.Domain;
using Database.Core.Repositories;
using Database.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace Database.Persistence.Repositories
{
    internal class RentalRecordRepository : Repository<RentalRecord>, IRentalRecordRepository
    {
        public RentalRecordRepository(RentalDBContext context, int? userId) : base(context, userId)
        {
        }

        private IQueryable<RentalRecord> GetWithDetails()
        {
            return RentalDBContext.RentalRecords
                .Include(r => r.RentalRequest)
                .Include(r => r.Payments)
                .Include(r => r.ReturnCondition)
                .AsQueryable();
        }

        private IQueryable<RentalRecord> GetWithFeedBackDetails()
        {
            return GetWithDetails()
                .Include(r => r.Feedbacks)
                .AsQueryable();
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }


        public RentalRecord GetWithDetails(int id)
        {
            return GetWithDetails().FirstOrDefault(r => r.Id == id);
        }




        public RentalRecord GetWithDetailsByRentalRequest(int id)
        {
            return GetWithDetails().FirstOrDefault(r => r.RentalRequestId == id);
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


        #region Search

        public override Dictionary<string, string> GetEntityColumnsWithTypes()
        {
            var d = base.GetEntityColumnsWithTypes();
            d.Remove("ExtraChargeDescription");
            d["RentalRequestId"] = "Int32";
            return d;
        }

        public override IQueryable<object> SelectViewColumns(IQueryable query)
        {
            query = query.Cast<RentalRecord>().Select(r => new
            {
                Id = r.Id,
                RequestId = r.RentalRequestId, 
                EquipmentName =  r.EquipmentName,
                PickupDate = r.PickupDate.Date,
                ActualReturnDate = r.ActualReturnDate,
                ReturnCondition = r.ReturnCondition != null ? r.ReturnCondition.ConditionName : "",
                TotalCost = r.TotalCost,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
            });

            return query.Cast<object>();
        }

        #endregion

        protected override bool ShouldIgnoreProperty(string propertyName)
        {
            return propertyName switch
            {
                nameof(RentalRecord.Feedbacks) => true,
                nameof(RentalRecord.Payments) => true,
                nameof(RentalRecord.RentalRequest) => true,
                nameof(RentalRecord.ReturnCondition) => true,
                nameof(RentalRecord.UpdatedAt) => true,
                nameof(RentalRecord.CreatedAt) => true,
                _ => base.ShouldIgnoreProperty(propertyName)
            };
        }

    }
}
