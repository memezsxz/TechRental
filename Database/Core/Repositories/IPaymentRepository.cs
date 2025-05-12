using Database.Core.Domain;

namespace Database.Core.Repositories
{
    /// <summary>
    /// Repository interface for managing <see cref="Payment"/> entities.
    /// Inherits standard CRUD operations from <see cref="IRepository{T}"/>.
    /// </summary>
    public interface IPaymentRepository : IRepository<Payment>
    {
    }
}