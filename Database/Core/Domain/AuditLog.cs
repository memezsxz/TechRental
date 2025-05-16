using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    [Table("AuditLog")]
    public partial class AuditLog
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("action_type")]
        [StringLength(100)]
        public string ActionType { get; set; } = null!;
        [Column("timestamp", TypeName = "datetime")]
        public DateTime Timestamp { get; set; }
        [Column("data_before_action")]
        public string DataBeforeAction { get; set; } = null!;
        [Column("source_entity")]
        [StringLength(100)]
        public string SourceEntity { get; set; } = null!;
        [Column("data_after_action")]
        public string DataAfterAction { get; set; } = null!;
        [Column("affected_record_key")]
        [StringLength(100)]
        public string AffectedRecordKey { get; set; } = null!;
        [Column("source")]
        [StringLength(255)]
        public string Source { get; set; } = null!;

        [ForeignKey("UserId")]
        [InverseProperty("AuditLogs")]
        public virtual User User { get; set; } = null!;
    }
}
