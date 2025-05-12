using Database.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Interfaces
{
    /// <summary>
    /// Defines a contract for entities or repositories that support notification generation.
    /// Used to create notifications that should be sent to users (e.g., rental status updates, overdue alerts).
    /// </summary>
    public interface INotifiable
    {
        /// <summary>
        /// Returns the notifications generated for the entity based on recent changes.
        /// Typically called during SaveChanges operations to capture and dispatch user alerts.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Notification"/> instances representing actionable or pending messages to be inserted into the database.
        /// </returns>
        IEnumerable<Notification> GetPendingNotifications();
    }
}