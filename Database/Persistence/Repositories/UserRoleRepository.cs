using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for managing UserRole entities.
    /// </summary>
    internal class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        #region Constructor

        public UserRoleRepository(RentalDBContext context, int userId)
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

        #region IUserRoleRepository Implementation

        /// <inheritdoc/>
        public Dictionary<int, string> GetAllByName()
        {
            return context.UserRoles.ToDictionary(ur => ur.Id, ur => ur.RoleName);
        }

        /// <inheritdoc/>
        public string getRoleNameByID(int id)
        {
            return context.UserRoles.FirstOrDefault(r => r.Id == id)?.RoleName ?? string.Empty;
        }

        #endregion
    }
}