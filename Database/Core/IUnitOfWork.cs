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
    /// <summary>
    /// Represents a unit of work that encapsulates all database repositories and coordinates save operations.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// The ID of the currently authenticated user, used for auditing or tracking purposes.
        /// </summary>
        int UserId { get; set; }

        #region Repository Properties

        /// <summary>
        /// Repository for audit log entities.
        /// </summary>
        IAuditLogRepository AuditLogs { get; }

        /// <summary>
        /// Repository for equipment categories.
        /// </summary>
        ICategoryRepository Categories { get; }

        /// <summary>
        /// Repository for uploaded documents.
        /// </summary>
        IDocumentRepository Documents { get; }

        /// <summary>
        /// Repository for managing equipment records.
        /// </summary>
        IEquipmentRepository Equipment { get; }

        /// <summary>
        /// Repository for managing equipment availability status (e.g., available, rented).
        /// </summary>
        IEquipmentAvailabilityStatusRepository EquipmentAvailabilityStatuses { get; }

        /// <summary>
        /// Repository for equipment condition status (e.g., good, damaged).
        /// </summary>
        IEquipmentConditionStatusRepository EquipmentConditionStatuses { get; }

        /// <summary>
        /// Repository for feedback records submitted by users.
        /// </summary>
        IFeedbackRepository Feedbacks { get; }

        /// <summary>
        /// Repository for image storage metadata.
        /// </summary>
        IImageRepository Images { get; }

        /// <summary>
        /// Repository for system-generated user notifications.
        /// </summary>
        INotificationRepository Notifications { get; }

        /// <summary>
        /// Repository for managing notification types.
        /// </summary>
        INotificationTypeRepository NotificationTypes { get; }

        /// <summary>
        /// Repository for handling payment transactions.
        /// </summary>
        IPaymentRepository Payments { get; }

        /// <summary>
        /// Repository for accepted payment methods.
        /// </summary>
        IPaymentMethodRepository PaymentMethods { get; }

        /// <summary>
        /// Repository for payment statuses (e.g., Paid, Pending).
        /// </summary>
        IPaymentStatusRepository PaymentStatuses { get; }

        /// <summary>
        /// Repository for completed rental records.
        /// </summary>
        IRentalRecordRepository RentalRecords { get; }

        /// <summary>
        /// Repository for rental request tracking and updates.
        /// </summary>
        IRentalRequestRepository RentalRequests { get; }

        /// <summary>
        /// Repository for rental request statuses (e.g., Approved, Rejected).
        /// </summary>
        IRentalRequestStatusRepository RentalRequestStatuses { get; }

        /// <summary>
        /// Repository for statuses representing the condition of returned items.
        /// </summary>
        IReturnConditionStatusRepository ReturnConditionStatuses { get; }

        /// <summary>
        /// Repository for logging system-level errors.
        /// </summary>
        ISystemErrorLogRepository SystemErrorLogs { get; }

        /// <summary>
        /// Repository for managing system users.
        /// </summary>
        IUserRepository Users { get; }

        /// <summary>
        /// Repository for managing user roles and permissions.
        /// </summary>
        IUserRoleRepository UserRoles { get; }

        #endregion

        #region Save Methods

        /// <summary>
        /// Commits all tracked changes to the database.
        /// </summary>
        /// <returns>The number of affected records.</returns>
        int SaveChanges();

        /// <summary>
        /// Asynchronously commits all tracked changes to the database.
        /// </summary>
        /// <returns>A task that resolves to the number of affected records.</returns>
        Task<int> SaveChangesAsync();

        #endregion
    }
}
