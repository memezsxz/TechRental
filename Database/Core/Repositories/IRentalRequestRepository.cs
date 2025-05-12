using Database.Core.Domain;
using Database.ViewModels;

namespace Database.Core.Repositories
{
    public interface IRentalRequestRepository : IRepository<RentalRequest>, IToBeTracked, INotifiable
    {
        public RentalRequest GetWithRecordDetails(int id);
        Task<WeeklyStats> GetWeeklyDashboardStatsAsync();
    }
}