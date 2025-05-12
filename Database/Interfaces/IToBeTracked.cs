using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;

namespace Database.Interfaces
{
    /// <summary>
    /// Defines a contract for repositories that support audit tracking of entity changes.
    /// Implementing this interface enables the generation of <see cref="AuditLog"/> entries
    /// based on tracked modifications within the context.
    /// </summary>
    public interface IToBeTracked
    {
        /// <summary>
        /// Retrieves a collection of <see cref="AuditLog"/> entries representing
        /// the tracked changes for the associated entity/entities.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="AuditLog"/> records describing changes
        /// (e.g., inserts, updates, deletes) made during the current unit of work.
        /// </returns>
        IEnumerable<AuditLog> GetAuditLogsFromTrackedChanges();
    }
}