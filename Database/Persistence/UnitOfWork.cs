using System.Reflection;
using Database.Core;
using Database.Core.Domain;
using Database.Core.Repositories;
using Database.Interfaces;
using Database.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Database.Persistence
{
    /// <summary>
    /// Coordinates repository access and manages tracking operations such as audit logging and notifications.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields

        /// <inheritdoc/>
        public int UserId { get; set; }

        private readonly RentalDBContext _context = new RentalDBContext();

        #endregion

        #region Constructors

        public UnitOfWork() { }

        public UnitOfWork(int userId)
        {
            UserId = userId;
        }

        #endregion

        #region Repository Properties

        private IAuditLogRepository _auditLogs;
        public IAuditLogRepository AuditLogs => _auditLogs ??= new AuditLogRepository(_context, UserId);

        private ICategoryRepository _categories;
        public ICategoryRepository Categories => _categories ??= new CategoryRepository(_context, UserId);

        private IDocumentRepository _documents;
        public IDocumentRepository Documents => _documents ??= new DocumentRepository(_context, UserId);

        private IEquipmentRepository _equipment;
        public IEquipmentRepository Equipment => _equipment ??= new EquipmentRepository(_context, UserId);

        private IEquipmentAvailabilityStatusRepository _equipmentAvailabilityStatuses;
        public IEquipmentAvailabilityStatusRepository EquipmentAvailabilityStatuses => _equipmentAvailabilityStatuses ??= new EquipmentAvailabilityStatusRepository(_context, UserId);

        private IEquipmentConditionStatusRepository _equipmentConditionStatuses;
        public IEquipmentConditionStatusRepository EquipmentConditionStatuses => _equipmentConditionStatuses ??= new EquipmentConditionStatusRepository(_context, UserId);

        private IFeedbackRepository _feedbacks;
        public IFeedbackRepository Feedbacks => _feedbacks ??= new FeedbackRepository(_context, UserId);

        private IImageRepository _images;
        public IImageRepository Images => _images ??= new ImageRepository(_context, UserId);

        private INotificationRepository _notifications;
        public INotificationRepository Notifications => _notifications ??= new NotificationRepository(_context, UserId);

        private INotificationTypeRepository _notificationTypes;
        public INotificationTypeRepository NotificationTypes => _notificationTypes ??= new NotificationTypeRepository(_context, UserId);

        private IPaymentRepository _payments;
        public IPaymentRepository Payments => _payments ??= new PaymentRepository(_context, UserId);

        private IPaymentMethodRepository _paymentMethods;
        public IPaymentMethodRepository PaymentMethods => _paymentMethods ??= new PaymentMethodRepository(_context, UserId);

        private IPaymentStatusRepository _paymentStatuses;
        public IPaymentStatusRepository PaymentStatuses => _paymentStatuses ??= new PaymentStatusRepository(_context, UserId);

        private IRentalRecordRepository _rentalRecords;
        public IRentalRecordRepository RentalRecords => _rentalRecords ??= new RentalRecordRepository(_context, UserId);

        private IRentalRequestRepository _rentalRequests;
        public IRentalRequestRepository RentalRequests => _rentalRequests ??= new RentalRequestRepository(_context, UserId);

        private IRentalRequestStatusRepository _rentalRequestStatuses;
        public IRentalRequestStatusRepository RentalRequestStatuses => _rentalRequestStatuses ??= new RentalRequestStatusRepository(_context, UserId);

        private IReturnConditionStatusRepository _returnConditionStatuses;
        public IReturnConditionStatusRepository ReturnConditionStatuses => _returnConditionStatuses ??= new ReturnConditionStatusRepository(_context, UserId);

        private ISystemErrorLogRepository _systemErrorLogs;
        public ISystemErrorLogRepository SystemErrorLogs => _systemErrorLogs ??= new SystemErrorLogRepository(_context, UserId);

        private IUserRepository _users;
        public IUserRepository Users => _users ??= new UserRepository(_context, UserId);

        private IUserRoleRepository _userRoles;
        public IUserRoleRepository UserRoles => _userRoles ??= new UserRoleRepository(_context, UserId);

        #endregion

        #region Save Methods

        /// <summary>
        /// Saves changes to the database after applying audit and notification tracking.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        /// <exception cref="DbUpdateException">Thrown when saving changes fails due to database update issues.</exception>
        public int SaveChanges()
        {
            TrackChanges();
            TrackNotifications();
            return _context.SaveChanges();
        }

        /// <summary>
        /// Asynchronously saves changes to the database.
        /// </summary>
        /// <returns>A task representing the asynchronous save operation, containing the number of state entries written.</returns>
        /// <exception cref="DbUpdateException">Thrown when saving changes fails due to database update issues.</exception>
        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        #endregion

        #region Tracking Logic

        /// <summary>
        /// Collects audit logs from all repositories implementing <see cref="IToBeTracked"/> and stores them in the context.
        /// </summary>
        private void TrackChanges()
        {
            var auditLogs = new List<AuditLog>();

            // Reflectively get all properties from the UnitOfWork that implement IToBeTracked and are not null
            var repoProps = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(p => typeof(IToBeTracked).IsAssignableFrom(p.PropertyType) && p.GetValue(this) != null);

            // For each matched repository, collect the audit logs from tracked entity changes
            foreach (var prop in repoProps)
            {
                if (prop.GetValue(this) is IToBeTracked trackedRepo)
                {
                    var logs = trackedRepo.GetAuditLogsFromTrackedChanges();
                    if (logs != null)
                        auditLogs.AddRange(logs); // Add all returned logs to the final list
                }
            }

            // Persist all collected audit logs into the DbContext
            foreach (var log in auditLogs)
            {
                _context.Set<AuditLog>().Add(log);
            }
        }

        #endregion

        #region Notification Logic

        /// <summary>
        /// Collects notifications from all repositories implementing <see cref="INotifiable"/> and stores them in the context.
        /// </summary>
        private void TrackNotifications()
        {
            var notifications = new List<Notification>();

            // Reflectively get all properties from the UnitOfWork that implement INotifiable and are not null
            var repoProps = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(p => typeof(INotifiable).IsAssignableFrom(p.PropertyType) && p.GetValue(this) != null);

            // For each matched repository, collect the pending notifications
            foreach (var prop in repoProps)
            {
                if (prop.GetValue(this) is INotifiable notifiableRepo)
                {
                    var notes = notifiableRepo.GetPendingNotifications();
                    if (notes != null)
                        notifications.AddRange(notes); // Add them to the collection
                }
            }

            // Persist all gathered notifications to the database context
            foreach (var note in notifications)
            {
                _context.Set<Notification>().Add(note);
            }
        }

        #endregion



        #region IDisposable

        /// <summary>
        /// Disposes the underlying database context.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if the context has already been disposed.</exception>
        public void Dispose()
        {
            _context.Dispose();
        }

        #endregion
    }
}
