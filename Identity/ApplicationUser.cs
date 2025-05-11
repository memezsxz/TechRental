using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Identity
{
    public class ApplicationUser : IdentityUser
    {

        [Required]
        public string? FirstName { get; set; }


        [Required]
        public string? LastName { get; set; }

        public int? UserID { get; set; }

    }
}
