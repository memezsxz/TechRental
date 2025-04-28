using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    [Table("Document")]
    public partial class Document
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("rental_id")]
        public int? RentalId { get; set; }
        [Column("file_name")]
        [StringLength(255)]
        public string FileName { get; set; } = null!;
        [Column("file_type")]
        [StringLength(50)]
        public string? FileType { get; set; }
        [Column("file_data")]
        public byte[]? FileData { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        [ForeignKey("RentalId")]
        [InverseProperty("Documents")]
        public virtual RentalRequest? Rental { get; set; }
    }
}
