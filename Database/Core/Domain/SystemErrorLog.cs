using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    [Table("SystemErrorLog")]
    public partial class SystemErrorLog
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("error_message")]
        public string ErrorMessage { get; set; } = null!;
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("timestamp", TypeName = "datetime")]
        public DateTime Timestamp { get; set; }
        [Column("source_procedure")]
        [StringLength(255)]
        public string? SourceProcedure { get; set; }
        [Column("error_source")]
        [StringLength(255)]
        public string ErrorSource { get; set; } = null!;

        [ForeignKey("UserId")]
        [InverseProperty("SystemErrorLogs")]
        public virtual User User { get; set; } = null!;
    }
}
