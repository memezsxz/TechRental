using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database.Core.Domain
{
    [Table("RentalRequest")]
    public partial class RentalRequest
    {
        public RentalRequest()
        {
            Documents = new HashSet<Document>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("equipment_id")]
        public int EquipmentId { get; set; }
        [Column("customer_id")]
        public int CustomerId { get; set; }
        [Column("start_date", TypeName = "date")]
        public DateTime StartDate { get; set; }
        [Column("return_date", TypeName = "date")]
        public DateTime ReturnDate { get; set; }
        [Column("rental_per_day", TypeName = "decimal(10, 2)")]
        public decimal RentalPerDay { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("notes")]
        [StringLength(255)]
        public string? Notes { get; set; }
        [Column("created_at", TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        [Column("updated_at", TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("RentalRequests")]
        public virtual User Customer { get; set; } = null!;
        [ForeignKey("EquipmentId")]
        [InverseProperty("RentalRequests")]
        public virtual Equipment Equipment { get; set; } = null!;
        [ForeignKey("StatusId")]
        [InverseProperty("RentalRequests")]
        public virtual RentalRequestStatus Status { get; set; } = null!;
        [InverseProperty("RentalRequest")]
        public virtual RentalRecord? RentalRecord { get; set; }
        [InverseProperty("Rental")]
        public virtual ICollection<Document> Documents { get; set; }
    }
}
