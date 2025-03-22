using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    [Table("UserRole")]
    [Index("RoleName", Name = "UQ__UserRole__783254B13102A65B", IsUnique = true)]
    public partial class UserRole
    {
        public UserRole()
        {
            Users = new HashSet<User>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("role_name")]
        [StringLength(50)]
        public string RoleName { get; set; } = null!;

        [InverseProperty("Role")]
        public virtual ICollection<User> Users { get; set; }
    }
}
