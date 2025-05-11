using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Database.Core.Domain;

namespace Database.Core.Repositories
{
    public interface IUserRepository : IRepository<User>, IStatus
    {
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
        Task<IEnumerable<User>> GetUsersAsync(string? searchString, string? roleFilter, SortOption? sortBy);

        Task<PaginatedResult> GetUsersAsync(int pageNumber, int pageSize, string? searchString, string? roleFilter,
            IUserRepository.SortOption? sortBy);
        Task<User?> GetUserWithRoleAsync(int id);
        Task<User?> GetUserWithProfileAsync(int id);
        Task<bool> UserExistsAsync(int id);
        public User? GetUserWithProfile(int id);
        public User? GetUserByEmail(string email);
    }

    public static class EnumExtensions
    {
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