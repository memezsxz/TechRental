using Database.Core.Domain;
using Database.Core.Repositories;
using Database.Search;
using Database.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Database.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for managing User entities with filtering, sorting, and profile access support.
    /// </summary>
    internal class UserRepository : Repository<User>, IUserRepository
    {
        #region Constructor

        public UserRepository(RentalDBContext context, int userId)
            : base(context, userId)
        {
        }

        #endregion

        #region Context Accessor

        /// <summary>
        /// Gets the current database context cast to <see cref="RentalDBContext"/>.
        /// </summary>
        public RentalDBContext RentalDBContext => context as RentalDBContext;

        #endregion

        #region IUserRepository Implementation


        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetUsersAsync(string? searchString, string? roleFilter, IUserRepository.SortOption? sortBy)
        {
            var query = RentalDBContext.Users
                .Include(u => u.Role)
                .Where(u => u.IsActive == true)
                .AsQueryable();

            // Search by name
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var lowerSearch = searchString.ToLower().Trim();
                query = query.Where(x =>
                    x.FirstName.ToLower().Contains(lowerSearch) ||
                    x.LastName.ToLower().Contains(lowerSearch) ||
                    (x.FirstName + " " + x.LastName).ToLower().Contains(lowerSearch));
            }

            // Filter by role
            if (!string.IsNullOrEmpty(roleFilter) && int.TryParse(roleFilter, out var roleId))
            {
                query = query.Where(x => x.RoleId == roleId);
            }

            // Apply sorting
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

        /// <inheritdoc/>
        public async Task<PaginatedResult> GetUsersAsync(int pageNumber, int pageSize, string? searchString, string? roleFilter, IUserRepository.SortOption? sortBy)
        {
            var query = RentalDBContext.Users.Include(u => u.Role).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                var lowerSearch = searchString.ToLower();
                query = query.Where(x => x.FirstName.ToLower().Contains(lowerSearch) || x.LastName.ToLower().Contains(lowerSearch));
            }

            if (!string.IsNullOrEmpty(roleFilter) && int.TryParse(roleFilter, out var roleId))
            {
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

        /// <inheritdoc/>
        public async Task<User?> GetUserWithRoleAsync(int id)
        {
            return await RentalDBContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <inheritdoc/>
        public async Task<User?> GetUserWithProfileAsync(int id)
        {
            return await RentalDBContext.Users
                .Include(u => u.Role)
                .Include(u => u.Image)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <inheritdoc/>
        public User? GetUserWithProfile(int id)
        {
            return RentalDBContext.Users
                .Include(u => u.Role)
                .Include(u => u.Image)
                .FirstOrDefault(u => u.Id == id);
        }

        /// <inheritdoc/>
        public async Task<bool> UserExistsAsync(int id)
        {
            return await RentalDBContext.Users.AnyAsync(e => e.Id == id);
        }

        /// <inheritdoc/>
        public Dictionary<int, string> GetAllByName()
        {
            return context.Users.ToDictionary(u => u.Id, u => $"{u.Id} - {u.FirstName} {u.LastName}");
        }

        /// <inheritdoc/>
        public User? GetUserByEmail(string email)
        {
            return RentalDBContext.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
        }

        #endregion

        #region Metadata Overrides

        /// <inheritdoc/>
        public override Dictionary<string, string> GetEntityColumnsWithTypes()
        {
            var d = base.GetEntityColumnsWithTypes();
            d.Remove("ImageId");

            return d;
        }

        /// <inheritdoc/>
        public override IQueryable<object> SelectViewColumns(IQueryable query)
        {
            return query.Cast<User>().Select(u => new
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Role = u.Role != null ? u.Role.RoleName : ""
            }).Cast<object>();
        }

        #endregion
    }
}
