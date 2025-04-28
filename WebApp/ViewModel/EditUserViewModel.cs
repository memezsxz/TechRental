using Database.Core.Domain;

namespace WebApp.ViewModel
{
    public class EditUserViewModel
    {
        public User User { get; set; }

        public IEnumerable<UserRole>? RolesList { get; set; }
    }
}
