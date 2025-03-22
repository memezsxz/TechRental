using Database.Core.Domain;

namespace Database.Core.Repositories
{
    public interface IUserRepository : IRepository<User>, IStatus
    {
    }
}