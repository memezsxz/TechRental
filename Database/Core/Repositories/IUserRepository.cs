using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Database.Core.Domain;
using Database.Interfaces;
using Database.Search;

namespace Database.Core.Repositories
{
    /// <summary>
    /// Repository interface for managing <see cref="User"/> entities.
    /// Includes user-related queries, filtering, sorting, and profile retrieval.
    /// </summary>
    public interface IUserRepository : IRepository<User>, IStatus
    {
        /// <summary>
        /// Defines available options for sorting user listings.
        /// </summary>
        public enum SortOption
        {
            [Display(Name = "First Name (A-Z)")]
            FirstNameAZ,

            [Display(Name = "First Name (Z-A)")]
            FirstNameZA,

            [Display(Name = "Role (A-Z)")]
            RoleAZ,

            [Display(Name = "Email (A-Z)")]
            EmailAZ
        }

        /// <summary>
        /// Retrieves a filtered and sorted list of users (no paging).
        /// </summary>
        /// <param name="searchString">A name or email substring to search for.</param>
        /// <param name="roleFilter">Optional role to filter users by.</param>
        /// <param name="sortBy">Optional sort option.</param>
        /// <returns>A collection of matching <see cref="User"/> entities.</returns>
        Task<IEnumerable<User>> GetUsersAsync(string? searchString, string? roleFilter, SortOption? sortBy);

        /// <summary>
        /// Retrieves a paginated, filtered, and sorted list of users.
        /// </summary>
        /// <param name="pageNumber">The current page number (1-based).</param>
        /// <param name="pageSize">The number of records per page.</param>
        /// <param name="searchString">A name or email substring to search for.</param>
        /// <param name="roleFilter">Optional role to filter users by.</param>
        /// <param name="sortBy">Optional sort option.</param>
        /// <returns>A <see cref="PaginatedResult"/> containing user data and metadata.</returns>
        Task<PaginatedResult> GetUsersAsync(
            int pageNumber,
            int pageSize,
            string? searchString,
            string? roleFilter,
            SortOption? sortBy);

        /// <summary>
        /// Retrieves a user along with their assigned role.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>A <see cref="User"/> with role information or null if not found.</returns>
        Task<User?> GetUserWithRoleAsync(int id);

        /// <summary>
        /// Retrieves a user along with their profile details.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>A <see cref="User"/> with profile information or null if not found.</returns>
        Task<User?> GetUserWithProfileAsync(int id);

        /// <summary>
        /// Checks if a user exists by ID.
        /// </summary>
        /// <param name="id">The user ID to check.</param>
        /// <returns>True if the user exists; otherwise, false.</returns>
        Task<bool> UserExistsAsync(int id);

        /// <summary>
        /// Retrieves a user with full profile data by ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>A <see cref="User"/> instance or null if not found.</returns>
        User? GetUserWithProfile(int id);

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email to look up.</param>
        /// <returns>A <see cref="User"/> instance or null if not found.</returns>
        User? GetUserByEmail(string email);
    }

    /// <summary>
    /// Extension methods for working with enums, especially for retrieving display-friendly names.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Retrieves the display name defined by the <see cref="DisplayAttribute"/> on an enum member.
        /// Falls back to the enum value's string representation if no display name is found.
        /// </summary>
        /// <param name="enumValue">The enum value to retrieve the display name for.</param>
        /// <returns>The display name or the enum name as a fallback.</returns>
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                       .GetMember(enumValue.ToString())[0]
                       .GetCustomAttribute<DisplayAttribute>()?
                       .GetName()
                   ?? enumValue.ToString();
        }
    }
}
