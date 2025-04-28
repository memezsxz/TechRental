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
        private readonly RentalDBContext _context;
        public UnitOfWork(RentalDBContext context)
        {
            _context = context;
            AuditLogs = new AuditLogRepository(context);
            Categories = new CategoryRepository(_context);
            Documents = new DocumentRepository(_context);
            Equipment = new EquipmentRepository(_context);
            EquipmentAvailabilityStatuses = new EquipmentAvailabilityStatusRepository(_context);
            EquipmentConditionStatuses = new EquipmentConditionStatusRepository(_context);
            Feedbacks = new FeedbackRepository(_context);
            Images = new ImageRepository(_context);
            Notifications = new NotificationRepository(_context);
            NotificationTypes = new NotificationTypeRepository(_context);
            Payments = new PaymentRepository(_context);
            PaymentMethods = new PaymentMethodRepository(_context);
            PaymentStatuses = new PaymentStatusRepository(_context);
            RentalRecords = new RentalRecordRepository(_context);
            RentalRequests = new RentalRequestRepository(_context);
            RentalRequestStatuses = new RentalRequestStatusRepository(_context);
            ReturnConditionStatuses = new ReturnConditionStatusRepository(_context);
            SystemErrorLogs = new SystemErrorLogRepository(context);
            Users = new UserRepository(_context);
            UserRoles = new UserRoleRepository(_context);
        }

        public IAuditLogRepository AuditLogs { get; private set; } //
        public ICategoryRepository Categories { get; private set; }
        public IDocumentRepository Documents { get; private set; }
        public IEquipmentRepository Equipment { get; private set; }
        public IEquipmentAvailabilityStatusRepository EquipmentAvailabilityStatuses { get; private set; }
        public IEquipmentConditionStatusRepository EquipmentConditionStatuses { get; private set; }
        public IFeedbackRepository Feedbacks { get; private set; } // 
        public IImageRepository Images { get; private set; } // 
        public INotificationRepository Notifications { get; private set; }
        public INotificationTypeRepository NotificationTypes { get; private set; }
        public IPaymentRepository Payments { get; private set; }
        public IPaymentMethodRepository PaymentMethods { get; private set; }
        public IPaymentStatusRepository PaymentStatuses { get; private set; }
        public IRentalRecordRepository RentalRecords { get; private set; }
        public IRentalRequestRepository RentalRequests { get; private set; }
        public IRentalRequestStatusRepository RentalRequestStatuses { get; private set; }
        public IReturnConditionStatusRepository ReturnConditionStatuses { get; private set; }
        public ISystemErrorLogRepository SystemErrorLogs { get; private set; } //
        public IUserRepository Users { get; private set; }
        public IUserRoleRepository UserRoles { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
