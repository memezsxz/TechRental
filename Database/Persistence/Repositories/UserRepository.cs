using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    internal class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(RentalDBContext context) : base(context)
        {
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }

        public Dictionary<int, string> GetAllByName()
        {
            return context.Users.ToDictionary(u => u.Id, u => $"{u.FirstName} {u.LastName}");
        }
    }
}