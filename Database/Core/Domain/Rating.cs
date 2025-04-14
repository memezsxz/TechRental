using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    [Table("Rating")]
    public partial class Rating
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("note")]
        [StringLength(255)]
        public string? Note { get; set; }
        [Column("rate", TypeName = "decimal(5, 2)")]
        public decimal Rate { get; set; }
        [Column("time_date", TypeName = "datetime")]
        public DateTime? TimeDate { get; set; }
        [Column("user_id")]
        public int? UserId { get; set; }
        [Column("equipment_id")]
        public int? EquipmentId { get; set; }
        [Column("is_hidden")]
        public bool? IsHidden { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column("updated_at", TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("EquipmentId")]
        [InverseProperty("Ratings")]
        public virtual Equipment? Equipment { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("Ratings")]
        public virtual User? User { get; set; }
    }
}
