using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    [Table("RentalRequestStatus")]
    [Index("StatusName", Name = "UQ__RentalRe__501B37530BA236BB", IsUnique = true)]
    public partial class RentalRequestStatus
    {
        public RentalRequestStatus()
        {
            RentalRequests = new HashSet<RentalRequest>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("status_name")]
        [StringLength(50)]
        public string StatusName { get; set; } = null!;

        [InverseProperty("Status")]
        public virtual ICollection<RentalRequest> RentalRequests { get; set; }
    }
}
