#nullable disable
using Database.Core.Domain;
using Database.Core.Repositories;

namespace WebApp.ViewModel
{
    public class ListUsersViewModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<UserRole> RolesList { get; set; }
        public IEnumerable<User> UsersList { get; set; }
        public string? SearchString { get; set; }
        public string? RoleFilter{ get; set; }
        public IUserRepository.SortOption? CurrentSort { get; set; }

        public IEnumerable<IUserRepository.SortOption> SortOptions { get; set; }

    }
}
