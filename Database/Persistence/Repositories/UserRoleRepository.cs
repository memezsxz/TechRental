using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    internal class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(RentalDBContext context, int? userId) : base(context, userId)
        {
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }

        public Dictionary<int, string> GetAllByName()
        {
            return context.UserRoles.ToDictionary(ur => ur.Id, ur => ur.RoleName);
        }

        public  string getRoleNameByID(int id)
        {
            return  context.UserRoles.FirstOrDefault(r => r.Id == id).RoleName.ToString();
        }
    }
}