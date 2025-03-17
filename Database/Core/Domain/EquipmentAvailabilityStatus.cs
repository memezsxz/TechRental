using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    [Table("EquipmentAvailabilityStatus")]
    [Index("StatusName", Name = "UQ__Equipmen__501B3753BE3CC58B", IsUnique = true)]
    public partial class EquipmentAvailabilityStatus
    {
        public EquipmentAvailabilityStatus()
        {
            Equipment = new HashSet<Equipment>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("status_name")]
        [StringLength(100)]
        public string StatusName { get; set; } = null!;

        [InverseProperty("AvailabilityStatus")]
        public virtual ICollection<Equipment> Equipment { get; set; }
    }
}
