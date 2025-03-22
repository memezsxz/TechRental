using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    internal class PaymentStatusRepository : Repository<PaymentStatus>, IPaymentStatusRepository
    {
        public PaymentStatusRepository(RentalDBContext context) : base(context)
        {
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }
        public Dictionary<int, string> GetAllByName()
        {
            return context.PaymentStatuses.ToDictionary(ps => ps.Id, ps => ps.StatusName);
        }
    }
}