using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.Core.Repositories;
using Database.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Database.Core
{
    public interface IUnitOfWork  : IDisposable
    {
        //public ICategoryRepository Category { get; }
        //public IDocumentRepository Document { get; }
        //public IEquipmentRepository Equipment { get; }
        //public IEquipmentAvailabilityStatusRepository EquipmentAvailabilityStatus { get; }
        //public IEquipmentConditionStatusRepository EquipmentConditionStatus { get; }
        //public IRatingRepository Rating { get; }
        //public IErrorLogRepository ErrorLog { get; }
        //public ILogRepository Log { get; }
        //public INotificationRepository Notification { get; }
        //public INotificationTypeRepository NotificationType { get; }
        //public IPaymentRepository Payment { get; }
        //public IPaymentMethodRepository PaymentMethod { get; }
        //public IPaymentStatusRepository PaymentStatus { get; }
        //public IRentalRecordRepository RentalRecord { get; }
        //public IRentalRequestRepository RentalRequest { get; }
        //public IRentalRequestStatusRepository RentalRequestStatus { get; }
        //public IReturnConditionStatusRepository ReturnConditionStatus { get; }
        //public IUserRepository User { get; }
        //public IUserRoleRepository UserRole { get; }

        public  IAuditLogRepository AuditLogs { get; } //
        public ICategoryRepository Categories { get;}
        public  IDocumentRepository Documents { get;  }
        public  IEquipmentRepository Equipment { get;  }
        public  IEquipmentAvailabilityStatusRepository EquipmentAvailabilityStatuses { get;  }
        public  IEquipmentConditionStatusRepository EquipmentConditionStatuses { get;  }
        public  IFeedbackRepository Feedbacks { get;  } // 
        public  IImageRepository Images { get;  } // 
        public  INotificationRepository Notifications { get;  }
        public  INotificationTypeRepository NotificationTypes { get;  }
        public  IPaymentRepository Payments { get;  }
        public  IPaymentMethodRepository PaymentMethods { get;  }
        public  IPaymentStatusRepository PaymentStatuses { get;  }
        public  IRentalRecordRepository RentalRecords { get;  }
        public  IRentalRequestRepository RentalRequests { get;  }
        public  IRentalRequestStatusRepository RentalRequestStatuses { get;  }
        public  IReturnConditionStatusRepository ReturnConditionStatuses { get;  }
        public  ISystemErrorLogRepository SystemErrorLogs { get;  } //
        public  IUserRepository Users { get;  }
        public  IUserRoleRepository UserRoles { get;  }
        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
