using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Repositories;
using Database.Persistence.Repositories;

namespace Database.Core
{
    internal interface IUnitOfWork  : IDisposable
    {
        public ICategoryRepository Category { get; }
        public IDocumentRepository Document { get; }
        public IEquipmentRepository Equipment { get; }
        public IEquipmentAvailabilityStatusRepository EquipmentAvailabilityStatus { get; }
        public IEquipmentConditionStatusRepository EquipmentConditionStatus { get; }
        public IRatingRepository Rating { get; }
        public IErrorLogRepository ErrorLog { get; }
        public ILogRepository Log { get; }
        public INotificationRepository Notification { get; }
        public INotificationTypeRepository NotificationType { get; }
        public IPaymentRepository Payment { get; }
        public IPaymentMethodRepository PaymentMethod { get; }
        public IPaymentStatusRepository PaymentStatus { get; }
        public IRentalRecordRepository RentalRecord { get; }
        public IRentalRequestRepository RentalRequest { get; }
        public IRentalRequestStatusRepository RentalRequestStatus { get; }
        public IReturnConditionStatusRepository ReturnConditionStatus { get; }
        public IUserRepository User { get; }
        public IUserRoleRepository UserRole { get; }
        int Complete();
    }
}
