using Database.Core.Domain;

namespace Database.Core.Repositories
{
    public interface IUserRoleRepository : IRepository<UserRole>, IStatus
    {
    }
}