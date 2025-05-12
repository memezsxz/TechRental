using Database.Core.Domain;
using Database.Interfaces;

namespace Database.Core.Repositories
{
    /// <summary>
    /// Repository interface for managing <see cref="PaymentMethod"/> entities.
    /// Inherits standard CRUD operations from <see cref="IRepository{T}"/> and
    /// status retrieval capabilities from <see cref="IStatus"/>.
    /// </summary>

    public interface IPaymentMethodRepository : IRepository<PaymentMethod>, IStatus
    {
    }
}