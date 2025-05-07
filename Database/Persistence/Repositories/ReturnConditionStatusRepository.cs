using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    internal class ReturnConditionStatusRepository : Repository<ReturnConditionStatus>, IReturnConditionStatusRepository
    {
        public ReturnConditionStatusRepository(RentalDBContext context) : base(context)
        {
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }
        public Dictionary<int, string> GetAllByName()
        {
            return context.ReturnConditionStatuses.ToDictionary(rrs => rrs.Id, rrs => rrs.ConditionName);
        }
    }
}