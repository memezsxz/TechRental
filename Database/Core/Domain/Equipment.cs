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
            EquipmentRates = new HashSet<EquipmentRate>();
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
        public string? Description { get; set; }
        [Column("rental_price", TypeName = "decimal(10, 2)")]
        public decimal RentalPrice { get; set; }
        [Column("availability_status_id")]
        public int? AvailabilityStatusId { get; set; }
        [Column("condition_status_id")]
        public int? ConditionStatusId { get; set; }
        [Column("category_id")]
        public int? CategoryId { get; set; }
        [Column("is_active")]
        public bool? IsActive { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column("updated_at", TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("AvailabilityStatusId")]
        [InverseProperty("Equipment")]
        public virtual EquipmentAvailabilityStatus? AvailabilityStatus { get; set; }
        [ForeignKey("CategoryId")]
        [InverseProperty("Equipment")]
        public virtual Category? Category { get; set; }
        [ForeignKey("ConditionStatusId")]
        [InverseProperty("Equipment")]
        public virtual EquipmentConditionStatus? ConditionStatus { get; set; }
        [InverseProperty("Equipment")]
        public virtual ICollection<EquipmentRate> EquipmentRates { get; set; }
        [InverseProperty("Equipment")]
        public virtual ICollection<RentalRequest> RentalRequests { get; set; }
    }
}
