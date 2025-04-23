using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    internal class SystemErrorLogRepository : Repository<SystemErrorLog>, ISystemErrorLogRepository
    {
        public SystemErrorLogRepository(RentalDBContext context) : base(context)
        {
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }
    }
}