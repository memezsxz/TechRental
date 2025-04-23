using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    [Table("PaymentStatus")]
    [Index("StatusName", Name = "UQ__PaymentS__501B3753922B4357", IsUnique = true)]
    public partial class PaymentStatus
    {
        public PaymentStatus()
        {
            Payments = new HashSet<Payment>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("status_name")]
        [StringLength(50)]
        public string StatusName { get; set; } = null!;

        [InverseProperty("PaymentStatus")]
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
