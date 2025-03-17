using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    [Table("ReturnConditionStatus")]
    [Index("ConditionName", Name = "UQ__ReturnCo__1A81D2F827980B50", IsUnique = true)]
    public partial class ReturnConditionStatus
    {
        public ReturnConditionStatus()
        {
            RentalRecords = new HashSet<RentalRecord>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("condition_name")]
        [StringLength(100)]
        public string ConditionName { get; set; } = null!;

        [InverseProperty("ReturnCondition")]
        public virtual ICollection<RentalRecord> RentalRecords { get; set; }
    }
}
