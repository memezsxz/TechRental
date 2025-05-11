using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    internal class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(RentalDBContext context, int? userId) : base(context, userId)
        {
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }
    }
}