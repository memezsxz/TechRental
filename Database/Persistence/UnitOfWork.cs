using System;
using System.Collections.Generic;
using System.Linq;
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
            Category = new CategoryRepository(_context);
            Document = new DocumentRepository(_context);
            Equipment = new EquipmentRepository(_context);
            EquipmentAvailabilityStatus = new EquipmentAvailabilityStatusRepository(_context);
            EquipmentConditionStatus = new EquipmentConditionStatusRepository(_context);
            EquipmentRate = new EquipmentRateRepository(_context);
            ErrorLog = new ErrorLogRepository(_context);
            Log = new LogRepository(_context);
            Notification = new NotificationRepository(_context);
            NotificationType = new NotificationTypeRepository(_context);
            Payment = new PaymentRepository(_context);
            PaymentMethod = new PaymentMethodRepository(_context);
            PaymentStatus = new PaymentStatusRepository(_context);
            RentalRecord = new RentalRecordRepository(_context);
            RentalRequest = new RentalRequestRepository(_context);
            RentalRequestStatus = new RentalRequestStatusRepository(_context);
            ReturnConditionStatus = new ReturnConditionStatusRepository(_context);
            User = new UserRepository(_context);
            UserRole = new UserRoleRepository(_context);
        }

        public ICategoryRepository Category { get; private set; }
        public IDocumentRepository Document { get; private set; }
        public IEquipmentRepository Equipment { get; private set; }
        public IEquipmentAvailabilityStatusRepository EquipmentAvailabilityStatus { get; private set; }
        public IEquipmentConditionStatusRepository EquipmentConditionStatus { get; private set; }
        public IEquipmentRateRepository EquipmentRate { get; private set; }
        public IErrorLogRepository ErrorLog { get; private set; }
        public ILogRepository Log { get; private set; }
        public INotificationRepository Notification { get; private set; }
        public INotificationTypeRepository NotificationType { get; private set; }
        public IPaymentRepository Payment { get; private set; }
        public IPaymentMethodRepository PaymentMethod { get; private set; }
        public IPaymentStatusRepository PaymentStatus { get; private set; }
        public IRentalRecordRepository RentalRecord { get; private set; }
        public IRentalRequestRepository RentalRequest { get; private set; }
        public IRentalRequestStatusRepository RentalRequestStatus { get; private set; }
        public IReturnConditionStatusRepository ReturnConditionStatus { get; private set; }
        public IUserRepository User { get; private set; }
        public IUserRoleRepository UserRole { get; private set; }

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
