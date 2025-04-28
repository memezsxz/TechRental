using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Database.Core;
using Database.Core.Domain;
using Database.Core.Repositories;
using Database.Persistence;
using Database.Persistence.Repositories;

namespace Database.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RentalDBContext _context = new RentalDBContext();
        public UnitOfWork()
        {
        }


            private IAuditLogRepository _auditLogs;
            public IAuditLogRepository AuditLogs => _auditLogs ??= new AuditLogRepository(_context);

            private ICategoryRepository _categories;
            public ICategoryRepository Categories => _categories ??= new CategoryRepository(_context);

            private IDocumentRepository _documents;
            public IDocumentRepository Documents => _documents ??= new DocumentRepository(_context);

            private IEquipmentRepository _equipment;
            public IEquipmentRepository Equipment => _equipment ??= new EquipmentRepository(_context);

            private IEquipmentAvailabilityStatusRepository _equipmentAvailabilityStatuses;
            public IEquipmentAvailabilityStatusRepository EquipmentAvailabilityStatuses => _equipmentAvailabilityStatuses ??= new EquipmentAvailabilityStatusRepository(_context);

            private IEquipmentConditionStatusRepository _equipmentConditionStatuses;
            public IEquipmentConditionStatusRepository EquipmentConditionStatuses => _equipmentConditionStatuses ??= new EquipmentConditionStatusRepository(_context);

            private IFeedbackRepository _feedbacks;
            public IFeedbackRepository Feedbacks => _feedbacks ??= new FeedbackRepository(_context);

            private IImageRepository _images;
            public IImageRepository Images => _images ??= new ImageRepository(_context);

            private INotificationRepository _notifications;
            public INotificationRepository Notifications => _notifications ??= new NotificationRepository(_context);

            private INotificationTypeRepository _notificationTypes;
            public INotificationTypeRepository NotificationTypes => _notificationTypes ??= new NotificationTypeRepository(_context);

            private IPaymentRepository _payments;
            public IPaymentRepository Payments => _payments ??= new PaymentRepository(_context);

            private IPaymentMethodRepository _paymentMethods;
            public IPaymentMethodRepository PaymentMethods => _paymentMethods ??= new PaymentMethodRepository(_context);

            private IPaymentStatusRepository _paymentStatuses;
            public IPaymentStatusRepository PaymentStatuses => _paymentStatuses ??= new PaymentStatusRepository(_context);

            private IRentalRecordRepository _rentalRecords;
            public IRentalRecordRepository RentalRecords => _rentalRecords ??= new RentalRecordRepository(_context);

            private IRentalRequestRepository _rentalRequests;
            public IRentalRequestRepository RentalRequests => _rentalRequests ??= new RentalRequestRepository(_context);

            private IRentalRequestStatusRepository _rentalRequestStatuses;
            public IRentalRequestStatusRepository RentalRequestStatuses => _rentalRequestStatuses ??= new RentalRequestStatusRepository(_context);

            private IReturnConditionStatusRepository _returnConditionStatuses;
            public IReturnConditionStatusRepository ReturnConditionStatuses => _returnConditionStatuses ??= new ReturnConditionStatusRepository(_context);

            private ISystemErrorLogRepository _systemErrorLogs;
            public ISystemErrorLogRepository SystemErrorLogs => _systemErrorLogs ??= new SystemErrorLogRepository(_context);

            private IUserRepository _users;
            public IUserRepository Users => _users ??= new UserRepository(_context);

            private IUserRoleRepository _userRoles;
            public IUserRoleRepository UserRoles => _userRoles ??= new UserRoleRepository(_context);



    public int SaveChanges()
        {
            return _context.SaveChanges();
        }


        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
