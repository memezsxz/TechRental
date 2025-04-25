using Database.Core.Domain;

namespace WebApp.ViewModel
{
    public class ListUsersViewModel
    {

        public IEnumerable<UserRole> RolesList { get; set; }
        public IEnumerable<User> UsersList { get; set; }
        public string? SearchString { get; set; }
        public string? RoleFilter{ get; set; }

        public string SortBy { get; set; }


    }
}
