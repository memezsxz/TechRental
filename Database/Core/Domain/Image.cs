using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    [Table("Image")]
    public partial class Image
    {
        public Image()
        {
            Equipment = new HashSet<Equipment>();
            Users = new HashSet<User>();
        }

        [Key]
        [Column("image_id")]
        public int ImageId { get; set; }
        [Column("image_name")]
        [StringLength(255)]
        public string ImageName { get; set; } = null!;
        [Column("image_type")]
        [StringLength(20)]
        public string? ImageType { get; set; } = null!;
        [Column("guid")]
        public Guid Guid { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        [InverseProperty("Image")]
        public virtual ICollection<Equipment> Equipment { get; set; }
        [InverseProperty("Image")]
        public virtual ICollection<User> Users { get; set; }
    }
}
