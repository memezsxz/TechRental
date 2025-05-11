using Database.Core.Domain;
using Database.Core.Repositories;
using Database.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Database.Persistence.Repositories
{
    internal class RentalRequestRepository : Repository<RentalRequest>, IRentalRequestRepository
    {
        public RentalRequestRepository(RentalDBContext context) : base(context)
        {
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }


        private IQueryable<RentalRequest> GetWithDetails()
        {
            return RentalDBContext.RentalRequests
                .Include(r => r.Customer)
                .Include(r => r.Equipment)
                .Include(r => r.Status)
                .AsQueryable();
        }

        private IQueryable<RentalRequest> GetWithRecordDetails()
        {
            return GetWithDetails()
                .Include(r => r.RentalRecords)
                .AsQueryable();
        }

        public RentalRequest GetWithRecordDetails(int id)
        {
            return GetWithRecordDetails().Where(r => r.Id == id).FirstOrDefault();
        }

        #region View Models

        public async Task<ManagerDashboardStats> GetWeeklyDashboardStatsAsync()
        {
            DateTime today = DateTime.Today;
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime weekStart = today.AddDays(-diff);
            DateTime weekEnd = weekStart.AddDays(7);

            var weeklyRequests = await RentalDBContext.RentalRequests
                .Include(r => r.RentalRecords)
                .ThenInclude(rr => rr.ReturnCondition)
                .Where(r => r.StartDate >= weekStart && r.StartDate < weekEnd)
                .ToListAsync();

            var weeklyRecords = weeklyRequests.SelectMany(r => r.RentalRecords).ToList();

            return new ManagerDashboardStats
            {
                TodaysPickups = weeklyRecords.Count(r => r.PickupDate.Date == today),
                TotalRentals = weeklyRecords.Count,
                OngoingRentals = weeklyRecords.Count(r => r.ActualReturnDate == null),
                CompletedRentals = weeklyRecords.Count(r => r.ActualReturnDate != null),
                OverdueRentals = weeklyRequests
                    .Where(r => r.RentalRecords.Any(rr => rr.ActualReturnDate == null))
                    .Count(r => r.ReturnDate < today),
                DamagedReturns = weeklyRecords.Count(r =>
                    r.ReturnCondition != null &&
                    r.ReturnCondition.ConditionName.ToLower().Contains("damaged"))
            };
        }


        #endregion

        #region Search

        public override Dictionary<string, string> GetEntityColumnsWithTypes()
        {
            var d = base.GetEntityColumnsWithTypes();
            if (d.ContainsKey("EquipmentId")) d["EquipmentId"] = "Int32";
            if (d.ContainsKey("CustomerId")) d["CustomerId"] = "Int32";
            return d;
        }

        public override IQueryable<object> SelectViewColumns(IQueryable query)
        {
            query = query.Cast<RentalRequest>().Select(rr => new
            {
                Id = rr.Id,
                Equipment = rr.Equipment != null ? rr.Equipment.Id + " - " + rr.Equipment.Name : "",
                Customer = rr.Customer != null
                    ? rr.Customer.Id + " - " + rr.Customer.FirstName + " " + rr.Customer.LastName
                    : "",
                StartDate = rr.StartDate,
                EndDate = rr.ReturnDate,
                Status = rr.Status != null ? rr.Status.StatusName : ""
            });

            return query.Cast<object>();
        }

        #endregion

    }
}