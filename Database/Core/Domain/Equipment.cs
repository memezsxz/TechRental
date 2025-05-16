using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    public partial class Equipment
    {
        public Equipment()
        {
            Feedbacks = new HashSet<Feedback>();
            RentalRequests = new HashSet<RentalRequest>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        [StringLength(100)]
        public string Name { get; set; } = null!;
        [Column("description")]
        [StringLength(255)]
        public string Description { get; set; } = null!;
        [Column("rental_price_per_day", TypeName = "decimal(10, 2)")]
        public decimal RentalPricePerDay { get; set; }
        [Column("availability_status_id")]
        public int AvailabilityStatusId { get; set; }
        [Column("condition_status_id")]
        public int ConditionStatusId { get; set; }
        [Column("category_id")]
        public int CategoryId { get; set; }
        [Column("is_active")]
        public bool? IsActive { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column("updated_at", TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }
        [Column("image_id")]
        public int? ImageId { get; set; }

        [ForeignKey("AvailabilityStatusId")]
        [InverseProperty("Equipment")]
        public virtual EquipmentAvailabilityStatus AvailabilityStatus { get; set; } = null!;
        [ForeignKey("CategoryId")]
        [InverseProperty("Equipment")]
        public virtual Category Category { get; set; } = null!;
        [ForeignKey("ConditionStatusId")]
        [InverseProperty("Equipment")]
        public virtual EquipmentConditionStatus ConditionStatus { get; set; } = null!;
        [ForeignKey("ImageId")]
        [InverseProperty("Equipment")]
        public virtual Image? Image { get; set; }
        [InverseProperty("Equipment")]
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        [InverseProperty("Equipment")]
        public virtual ICollection<RentalRequest> RentalRequests { get; set; }
    }
}
