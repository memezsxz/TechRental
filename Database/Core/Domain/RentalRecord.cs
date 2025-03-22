using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    [Table("RentalRecord")]
    public partial class RentalRecord
    {
        public RentalRecord()
        {
            Payments = new HashSet<Payment>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("rental_request_id")]
        public int? RentalRequestId { get; set; }
        [Column("equipment_name")]
        [StringLength(100)]
        public string? EquipmentName { get; set; }
        [Column("customer_name")]
        [StringLength(100)]
        public string? CustomerName { get; set; }
        [Column("customer_phone_number")]
        [StringLength(20)]
        public string? CustomerPhoneNumber { get; set; }
        [Column("customer_email")]
        [StringLength(100)]
        public string? CustomerEmail { get; set; }
        [Column("pickup_date", TypeName = "date")]
        public DateTime PickupDate { get; set; }
        [Column("actual_return_date", TypeName = "date")]
        public DateTime? ActualReturnDate { get; set; }
        [Column("return_condition_id")]
        public int? ReturnConditionId { get; set; }
        [Column("deposit", TypeName = "decimal(10, 2)")]
        public decimal? Deposit { get; set; }
        [Column("late_return_fees", TypeName = "decimal(10, 2)")]
        public decimal? LateReturnFees { get; set; }
        [Column("extra_charges", TypeName = "decimal(10, 2)")]
        public decimal? ExtraCharges { get; set; }
        [Column("extra_charge_description")]
        [StringLength(255)]
        public string? ExtraChargeDescription { get; set; }
        [Column("total_cost", TypeName = "decimal(10, 2)")]
        public decimal? TotalCost { get; set; }
        [Column("rental_fee", TypeName = "decimal(10, 2)")]
        public decimal? RentalFee { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column("updated_at", TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("RentalRequestId")]
        [InverseProperty("RentalRecords")]
        public virtual RentalRequest? RentalRequest { get; set; }
        [ForeignKey("ReturnConditionId")]
        [InverseProperty("RentalRecords")]
        public virtual ReturnConditionStatus? ReturnCondition { get; set; }
        [InverseProperty("RentalRecord")]
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
