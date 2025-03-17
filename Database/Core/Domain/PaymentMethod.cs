using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    [Table("PaymentMethod")]
    [Index("MethodName", Name = "UQ__PaymentM__2DA2FAEEFEAF910A", IsUnique = true)]
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            Payments = new HashSet<Payment>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("method_name")]
        [StringLength(100)]
        public string MethodName { get; set; } = null!;

        [InverseProperty("PaymentMethod")]
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
