using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Database.Core.Domain;

namespace Database.Persistence
{
    public partial class RentalDBContext : DbContext
    {
        public RentalDBContext()
        {
        }

        public RentalDBContext(DbContextOptions<RentalDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Document> Documents { get; set; } = null!;
        public virtual DbSet<Equipment> Equipment { get; set; } = null!;
        public virtual DbSet<EquipmentAvailabilityStatus> EquipmentAvailabilityStatuses { get; set; } = null!;
        public virtual DbSet<EquipmentConditionStatus> EquipmentConditionStatuses { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<NotificationType> NotificationTypes { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
        public virtual DbSet<PaymentStatus> PaymentStatuses { get; set; } = null!;
        public virtual DbSet<RentalRecord> RentalRecords { get; set; } = null!;
        public virtual DbSet<RentalRequest> RentalRequests { get; set; } = null!;
        public virtual DbSet<RentalRequestStatus> RentalRequestStatuses { get; set; } = null!;
        public virtual DbSet<ReturnConditionStatus> ReturnConditionStatuses { get; set; } = null!;
        public virtual DbSet<SystemErrorLog> SystemErrorLogs { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=reboot08.com,1450;Database=RentalDB;User Id=sa;Password='caliber,willpower,enjoyably,ending,giggling,P5';Encrypt=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AuditLogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AuditLog__user_i__6754599E");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Rental)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.RentalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Document__rental__00200768");
            });

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.AvailabilityStatus)
                    .WithMany(p => p.Equipment)
                    .HasForeignKey(d => d.AvailabilityStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Equipment__avail__45F365D3");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Equipment)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Equipment__categ__47DBAE45");

                entity.HasOne(d => d.ConditionStatus)
                    .WithMany(p => p.Equipment)
                    .HasForeignKey(d => d.ConditionStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Equipment__condi__46E78A0C");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Equipment)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK__Equipment__image__4BAC3F29");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsHidden).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.EquipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Feedback__equipm__6D0D32F4");

                entity.HasOne(d => d.RentalRecord)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.RentalRecordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Feedback_RentalRecord");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Feedback__user_i__6C190EBB");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsRead).HasDefaultValueSql("((0))");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.NotificationType)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.NotificationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Notificat__notif__73BA3083");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Notificat__user___72C60C4A");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.PaymentDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Payment__payment__0A9D95DB");

                entity.HasOne(d => d.PaymentStatus)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.PaymentStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Payment__payment__0B91BA14");

                entity.HasOne(d => d.RentalRecord)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.RentalRecordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Payment__rental___09A971A2");
            });

            modelBuilder.Entity<RentalRecord>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.RentalRequest)
                    .WithOne(p => p.RentalRecord)
                    .HasForeignKey<RentalRecord>(d => d.RentalRequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RentalRec__renta__03F0984C");

                entity.HasOne(d => d.ReturnCondition)
                    .WithMany(p => p.RentalRecords)
                    .HasForeignKey(d => d.ReturnConditionId)
                    .HasConstraintName("FK__RentalRec__retur__04E4BC85");
            });

            modelBuilder.Entity<RentalRequest>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.RentalRequests)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RentalReq__custo__7A672E12");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.RentalRequests)
                    .HasForeignKey(d => d.EquipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RentalReq__equip__797309D9");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.RentalRequests)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RentalReq__statu__7B5B524B");
            });

            modelBuilder.Entity<SystemErrorLog>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SystemErrorLogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SystemErr__user___0F624AF8");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK__User__image_id__6477ECF3");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User__role_id__60A75C0F");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
