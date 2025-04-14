using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    [Index("Email", Name = "UQ__Users__AB6E61646F63A786", IsUnique = true)]
    public partial class User
    {
        public User()
        {
            Documents = new HashSet<Document>();
            ErrorLogs = new HashSet<ErrorLog>();
            Logs = new HashSet<Log>();
            Notifications = new HashSet<Notification>();
            Ratings = new HashSet<Rating>();
            RentalRequests = new HashSet<RentalRequest>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("first_name")]
        [StringLength(50)]
        public string FirstName { get; set; } = null!;
        [Column("last_name")]
        [StringLength(50)]
        public string LastName { get; set; } = null!;
        [Column("email")]
        [StringLength(100)]
        public string Email { get; set; } = null!;
        [Column("role_id")]
        public int? RoleId { get; set; }
        [Column("is_active")]
        public bool? IsActive { get; set; }
        [Column("image_path")]
        [StringLength(255)]
        public string? ImagePath { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column("updated_at", TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("RoleId")]
        [InverseProperty("Users")]
        public virtual UserRole? Role { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Document> Documents { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ErrorLog> ErrorLogs { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Log> Logs { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Notification> Notifications { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Rating> Ratings { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<RentalRequest> RentalRequests { get; set; }
    }
}
