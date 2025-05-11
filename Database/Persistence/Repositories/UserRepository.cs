using Database.Core.Domain;
using Database.Core.Repositories;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

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

        public bool IsReferenced(int id)
        {
            return RentalDBContext.RentalRequests.Any(r => r.EquipmentId == id);
        }

        #region Async

        public async Task<IEnumerable<User>> GetUsersAsync(string? searchString, string? roleFilter, IUserRepository.SortOption? sortBy)
        {
            var query = RentalDBContext.Users.Include(u => u.Role).Where(u => u.IsActive == true).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                var lowerSearch = searchString.ToLower().Trim();
                query = query.Where(x =>
                    x.FirstName.ToLower().Contains(lowerSearch) ||
                    x.LastName.ToLower().Contains(lowerSearch) ||
                    (x.FirstName + " " + x.LastName).ToLower().Contains(lowerSearch));
            }

            if (!string.IsNullOrEmpty(roleFilter))
            {
                if (int.TryParse(roleFilter, out var roleId))
                    query = query.Where(x => x.RoleId == roleId);
            }

            query = sortBy switch
            {
                IUserRepository.SortOption.FirstNameAZ => query.OrderBy(x => x.FirstName),
                IUserRepository.SortOption.FirstNameZA => query.OrderByDescending(x => x.FirstName),
                IUserRepository.SortOption.RoleAZ => query.OrderBy(x => x.Role.RoleName),
                IUserRepository.SortOption.EmailAZ => query.OrderBy(x => x.Email),
                _ => query.OrderBy(x => x.Id)
            };

            return await query.ToListAsync();
        }


        public async Task<PaginatedResult> GetUsersAsync(int pageNumber, int pageSize, string? searchString, string? roleFilter, IUserRepository.SortOption? sortBy)
        {
            var query = RentalDBContext.Users.Include(u => u.Role).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x => x.FirstName.ToLower().Contains(searchString.ToLower()) ||
                                         x.LastName.ToLower().Contains(searchString.ToLower()));
            }

            if (!string.IsNullOrEmpty(roleFilter))
            {
                if (int.TryParse(roleFilter, out var roleId))
                    query = query.Where(x => x.RoleId == roleId);
            }

            query = sortBy switch
            {
                IUserRepository.SortOption.FirstNameAZ => query.OrderBy(x => x.FirstName),
                IUserRepository.SortOption.FirstNameZA => query.OrderByDescending(x => x.FirstName),
                IUserRepository.SortOption.RoleAZ => query.OrderBy(x => x.Role.RoleName),
                IUserRepository.SortOption.EmailAZ => query.OrderBy(x => x.Email),
                _ => query.OrderBy(x => x.Id)
            };

            return await GetPaginatedResultAsync(query, pageNumber, pageSize);
        }


        public async Task<User?> GetUserWithRoleAsync(int id)
        {
            return await RentalDBContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserWithProfileAsync(int id)
        {
            return await RentalDBContext.Users
                .Include(u => u.Role)
                .Include(u => u.Image)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> UserExistsAsync(int id)
        {
            return await RentalDBContext.Users.AnyAsync(e => e.Id == id);
        }



        #endregion
        public  User? GetUserWithProfile(int id)
        {
            return  RentalDBContext.Users
                .Include(u => u.Role)
                .Include(u => u.Image)
                .FirstOrDefault(u => u.Id == id);
        }



        #region IStatus

        public Dictionary<int, string> GetAllByName()
        {
            return context.Users.ToDictionary(u => u.Id, u => $"{u.Id} - {u.FirstName} {u.LastName}");
        }

        #endregion

        #region Search
        public override Dictionary<string, string> GetEntityColumnsWithTypes()
        {
            var d = base.GetEntityColumnsWithTypes();

            d.Remove("Image");

            foreach (var kv in d)
            {
                Console.WriteLine(kv);
            }
            return d;
        }

        public override IQueryable<object> SelectViewColumns(IQueryable query)
        {
            query = query.Cast<User>().Select(u => new
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Role = u.Role != null ? u.Role.RoleName : "",
            });

            return query.Cast<object>();
        }

        public User? GetUserByEmail(string email)
        {
            return  RentalDBContext.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
        }

        #endregion
    }
}