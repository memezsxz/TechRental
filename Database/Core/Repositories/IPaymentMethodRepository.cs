using Database.Core.Domain;

namespace Database.Core.Repositories
{
    public interface IPaymentMethodRepository : IRepository<PaymentMethod>, IStatus
    {
    }
}