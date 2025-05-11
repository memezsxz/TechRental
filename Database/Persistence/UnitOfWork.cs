using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Database.Core;
using Database.Core.Domain;
using Database.Core.Repositories;
using Database.Persistence;
using Database.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Database.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields
        public int? UserId { get; set; }

        #endregion

        private readonly RentalDBContext _context = new RentalDBContext();
        private readonly IUnitOfWork _unitOfWork;
        public UnitOfWork()
        {
        }

        public UnitOfWork(int userId)
        {
            UserId = userId;
        }
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



        public int SaveChanges()
        {
            TrackChanges();
            return _context.SaveChanges();
        }


        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        private void TrackChanges()
        {
            var auditLogs = new List<AuditLog>();

            var repoProps = GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(p =>
                    typeof(IToBeTracked).IsAssignableFrom(p.PropertyType) &&
                    p.GetValue(this) is not null);

            foreach (var prop in repoProps)
            {
                if (prop.GetValue(this) is IToBeTracked trackedRepo)
                {
                    var logs = trackedRepo.GetAuditLogsFromTrackedChanges();
                    //Console.WriteLine($"Audit logs collected from repo: {logs?.Count() ?? 0}");
                    if (logs != null)
                        auditLogs.AddRange(logs);
                }
            }

            foreach (var log in auditLogs)
            {
                _context.Set<AuditLog>().Add(log);

                //Console.WriteLine("B: " + log.DataBeforeAction);
                //Console.WriteLine("A: " + log.DataAfterAction);
            }

            //Console.WriteLine($"Audit logs collected: {auditLogs.Count}");

        }

        public void Dispose()
        {
            _context.Dispose();
        }


    }
}
