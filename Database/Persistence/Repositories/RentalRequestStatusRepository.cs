using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    internal class RentalRequestStatusRepository : Repository<RentalRequestStatus>, IRentalRequestStatusRepository
    {
        public RentalRequestStatusRepository(RentalDBContext context, int? userId) : base(context, userId)
        {
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }

        public Dictionary<int, string> GetAllByName()
        {
            return context.RentalRequestStatuses.ToDictionary(rrs => rrs.Id, rrs => rrs.StatusName);
        }
    }
}