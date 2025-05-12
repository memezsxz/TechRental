using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    [Table("User")]
    [Index("Email", Name = "UQ__User__AB6E6164730F9299", IsUnique = true)]
    public partial class User
    {
        public User()
        {
            AuditLogs = new HashSet<AuditLog>();
            Feedbacks = new HashSet<Feedback>();
            Notifications = new HashSet<Notification>();
            RentalRequests = new HashSet<RentalRequest>();
            SystemErrorLogs = new HashSet<SystemErrorLog>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("first_name")]
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "First name must contain at least 3 characters.")]
        public string FirstName { get; set; } = null!;
        [Column("last_name")]
        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Last name must contain at least 3 characters.")]
        public string LastName { get; set; } = null!;
        [Column("email")]
        [StringLength(100)]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = null!;
        [Column("role_id")]
        [Required]
        public int? RoleId { get; set; }
        [Column("is_active")]
        public bool? IsActive { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column("updated_at", TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }
        [Column("image_id")]
        public int? ImageId { get; set; }
        [Column("phone_number")]
        [StringLength(20)]
        [Unicode(false)]
        public string? PhoneNumber { get; set; }

        [ForeignKey("ImageId")]
        [InverseProperty("Users")]
        public virtual Image? Image { get; set; }
        [ForeignKey("RoleId")]
        [InverseProperty("Users")]
        public virtual UserRole? Role { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AuditLog> AuditLogs { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Notification> Notifications { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<RentalRequest> RentalRequests { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SystemErrorLog> SystemErrorLogs { get; set; }
    }
}
