using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.Core.Repositories;
using Database.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Database.Core
{
    public interface IUnitOfWork  : IDisposable
    {
        int? UserId { get; set; }

        public IAuditLogRepository AuditLogs { get; }
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
