using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Database.Core.Domain;
using DotNetEnv;

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

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Document> Documents { get; set; } = null!;
        public virtual DbSet<Equipment> Equipment { get; set; } = null!;
        public virtual DbSet<EquipmentAvailabilityStatus> EquipmentAvailabilityStatuses { get; set; } = null!;
        public virtual DbSet<EquipmentConditionStatus> EquipmentConditionStatuses { get; set; } = null!;
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; } = null!;
        public virtual DbSet<Log> Logs { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<NotificationType> NotificationTypes { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
        public virtual DbSet<PaymentStatus> PaymentStatuses { get; set; } = null!;
        public virtual DbSet<Rating> Ratings { get; set; } = null!;
        public virtual DbSet<RentalRecord> RentalRecords { get; set; } = null!;
        public virtual DbSet<RentalRequest> RentalRequests { get; set; } = null!;
        public virtual DbSet<RentalRequestStatus> RentalRequestStatuses { get; set; } = null!;
        public virtual DbSet<ReturnConditionStatus> ReturnConditionStatuses { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Load environment variables from .env file
                string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
                string customEnvFilePath = Path.Combine(projectRoot, "Config.env");
                //Console.WriteLine(customEnvFilePath);
                Env.Load(customEnvFilePath);

                // Retrieve credentials from environment variables
                string server = Env.GetString("DB_SERVER");
                string database = Env.GetString("DB_NAME");
                string user = Env.GetString("DB_USER");
                string password = Env.GetString("DB_PASSWORD");
                string encrypt = Env.GetString("DB_ENCRYPT", "False");
                string trustServerCert = Env.GetString("DB_TRUST_SERVER_CERT", "True");

                // Build the connection string
                string connectionString = $"Server={server};Database={database};User Id={user};Password={password};Encrypt={encrypt};TrustServerCertificate={trustServerCert};";

                // Configure the database connection
                optionsBuilder.UseSqlServer(connectionString);

                //optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=RentalDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
                    .HasConstraintName("FK__Document__rental__76969D2E");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Document__user_i__75A278F5");
            });

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.AvailabilityStatus)
                    .WithMany(p => p.Equipment)
                    .HasForeignKey(d => d.AvailabilityStatusId)
                    .HasConstraintName("FK__Equipment__avail__59FA5E80");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Equipment)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Equipment__categ__5BE2A6F2");

                entity.HasOne(d => d.ConditionStatus)
                    .WithMany(p => p.Equipment)
                    .HasForeignKey(d => d.ConditionStatusId)
                    .HasConstraintName("FK__Equipment__condi__5AEE82B9");
            });

            modelBuilder.Entity<ErrorLog>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ErrorLogs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__ErrorLog__user_i__04E4BC85");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Logs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Log__user_id__01142BA1");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsRead).HasDefaultValueSql("((0))");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.NotificationType)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.NotificationTypeId)
                    .HasConstraintName("FK__Notificat__notif__7B5B524B");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Notificat__user___7A672E12");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.PaymentDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .HasConstraintName("FK__Payment__payment__0A9D95DB");

                entity.HasOne(d => d.PaymentStatus)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.PaymentStatusId)
                    .HasConstraintName("FK__Payment__payment__0B91BA14");

                entity.HasOne(d => d.RentalRecord)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.RentalRecordId)
                    .HasConstraintName("FK__Payment__rental___09A971A2");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsHidden).HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.EquipmentId)
                    .HasConstraintName("FK__Equipment__equip__6FE99F9F");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Equipment__user___6EF57B66");
            });

            modelBuilder.Entity<RentalRecord>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.RentalRequest)
                    .WithMany(p => p.RentalRecords)
                    .HasForeignKey(d => d.RentalRequestId)
                    .HasConstraintName("FK__RentalRec__renta__68487DD7");

                entity.HasOne(d => d.ReturnCondition)
                    .WithMany(p => p.RentalRecords)
                    .HasForeignKey(d => d.ReturnConditionId)
                    .HasConstraintName("FK__RentalRec__retur__693CA210");
            });

            modelBuilder.Entity<RentalRequest>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.RentalRequests)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__RentalReq__custo__628FA481");

                entity.HasOne(d => d.Equipment)
                    .WithMany(p => p.RentalRequests)
                    .HasForeignKey(d => d.EquipmentId)
                    .HasConstraintName("FK__RentalReq__equip__619B8048");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.RentalRequests)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK__RentalReq__statu__6383C8BA");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Users__role_id__4F7CD00D");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
