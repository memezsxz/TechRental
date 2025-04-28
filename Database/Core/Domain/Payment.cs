using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    [Table("Payment")]
    public partial class Payment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("rental_record_id")]
        public int? RentalRecordId { get; set; }
        [Column("transaction_id")]
        [StringLength(100)]
        public string? TransactionId { get; set; }
        [Column("amount", TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }
        [Column("payment_method_id")]
        public int? PaymentMethodId { get; set; }
        [Column("payment_status_id")]
        public int? PaymentStatusId { get; set; }
        [Column("payment_date", TypeName = "datetime")]
        public DateTime? PaymentDate { get; set; }

        [ForeignKey("PaymentMethodId")]
        [InverseProperty("Payments")]
        public virtual PaymentMethod? PaymentMethod { get; set; }
        [ForeignKey("PaymentStatusId")]
        [InverseProperty("Payments")]
        public virtual PaymentStatus? PaymentStatus { get; set; }
        [ForeignKey("RentalRecordId")]
        [InverseProperty("Payments")]
        public virtual RentalRecord? RentalRecord { get; set; }
    }
}
