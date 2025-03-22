using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    internal class RentalRequestStatusRepository : Repository<RentalRequestStatus>, IRentalRequestStatusRepository
    {
        public RentalRequestStatusRepository(RentalDBContext context) : base(context)
        {
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }
    }
}