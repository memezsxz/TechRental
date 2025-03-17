using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    [Table("Notification")]
    public partial class Notification
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("user_id")]
        public int? UserId { get; set; }
        [Column("message_content")]
        [StringLength(255)]
        public string? MessageContent { get; set; }
        [Column("notification_type_id")]
        public int? NotificationTypeId { get; set; }
        [Column("status")]
        [StringLength(50)]
        public string? Status { get; set; }
        [Column("isRead")]
        public bool? IsRead { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column("updated_at", TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("NotificationTypeId")]
        [InverseProperty("Notifications")]
        public virtual NotificationType? NotificationType { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("Notifications")]
        public virtual User? User { get; set; }
    }
}
