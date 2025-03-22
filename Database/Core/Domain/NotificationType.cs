using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    [Table("NotificationType")]
    [Index("TypeName", Name = "UQ__Notifica__543C4FD949E6ED34", IsUnique = true)]
    public partial class NotificationType
    {
        public NotificationType()
        {
            Notifications = new HashSet<Notification>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("type_name")]
        [StringLength(50)]
        public string TypeName { get; set; } = null!;

        [InverseProperty("NotificationType")]
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
