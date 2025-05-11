using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    internal class PaymentMethodRepository : Repository<PaymentMethod>, IPaymentMethodRepository
    {
        public PaymentMethodRepository(RentalDBContext context, int? userId) : base(context, userId)
        {
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }

        public Dictionary<int, string> GetAllByName()
        {
            return context.PaymentMethods.ToDictionary(pm => pm.Id, pm => pm.MethodName);
        }
    }
}