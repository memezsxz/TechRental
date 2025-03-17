using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    [Table("ErrorLog")]
    public partial class ErrorLog
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("error_message")]
        public string ErrorMessage { get; set; } = null!;
        [Column("error_severity")]
        [StringLength(50)]
        public string? ErrorSeverity { get; set; }
        [Column("error_state")]
        [StringLength(50)]
        public string? ErrorState { get; set; }
        [Column("user_id")]
        public int? UserId { get; set; }
        [Column("timestamp", TypeName = "datetime")]
        public DateTime? Timestamp { get; set; }
        [Column("source_procedure")]
        [StringLength(255)]
        public string? SourceProcedure { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("ErrorLogs")]
        public virtual User? User { get; set; }
    }
}
