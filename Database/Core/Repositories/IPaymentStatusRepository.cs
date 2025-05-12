using Database.Core.Domain;
using Database.Interfaces;

namespace Database.Core.Repositories
{
    /// <summary>
    /// Repository interface for managing <see cref="PaymentStatus"/> entities.
    /// Inherits standard CRUD operations from <see cref="IRepository{T}"/> and
    /// status listing capabilities from <see cref="IStatus"/>.
    /// </summary>

    public interface IPaymentStatusRepository : IRepository<PaymentStatus>, IStatus
    {
    }
}