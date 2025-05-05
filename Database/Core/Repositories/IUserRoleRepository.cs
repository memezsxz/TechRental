using Database.Core.Domain;

namespace Database.Core.Repositories
{
    public interface IUserRoleRepository : IRepository<UserRole>, IStatus
    {
        public string getRoleNameByID(int id);
    }
}