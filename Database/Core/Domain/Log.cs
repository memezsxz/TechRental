using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    [Table("Log")]
    public partial class Log
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("user_id")]
        public int? UserId { get; set; }
        [Column("action_type")]
        [StringLength(100)]
        public string? ActionType { get; set; }
        [Column("expectations")]
        [StringLength(255)]
        public string? Expectations { get; set; }
        [Column("timestamp", TypeName = "datetime")]
        public DateTime? Timestamp { get; set; }
        [Column("data_before_action")]
        public string? DataBeforeAction { get; set; }
        [Column("source_entity")]
        [StringLength(100)]
        public string? SourceEntity { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Log")]
        public virtual User? User { get; set; }
    }
}
