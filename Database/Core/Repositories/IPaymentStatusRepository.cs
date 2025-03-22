using Database.Core.Domain;

namespace Database.Core.Repositories
{
    public interface IPaymentStatusRepository : IRepository<PaymentStatus>, IStatus
    {
    }
}