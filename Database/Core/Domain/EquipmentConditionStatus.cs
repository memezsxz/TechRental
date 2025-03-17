using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    [Table("EquipmentConditionStatus")]
    [Index("ConditionName", Name = "UQ__Equipmen__1A81D2F80667AFA7", IsUnique = true)]
    public partial class EquipmentConditionStatus
    {
        public EquipmentConditionStatus()
        {
            Equipment = new HashSet<Equipment>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("condition_name")]
        [StringLength(100)]
        public string ConditionName { get; set; } = null!;

        [InverseProperty("ConditionStatus")]
        public virtual ICollection<Equipment> Equipment { get; set; }
    }
}
