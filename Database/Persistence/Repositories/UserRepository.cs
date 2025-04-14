using Database.Core.Domain;
using Database.Core.Repositories;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
            return context.Users.ToDictionary(u => u.Id, u => $"{u.Id} - {u.FirstName} {u.LastName}");
        }

        public override IQueryable<object> SelectViewColumns(IQueryable query)
        {
            query = query.Cast<User>().Select(u => new
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role != null ? u.Role.RoleName : "",
            });

            return query.Cast<object>();
        }
    }
}