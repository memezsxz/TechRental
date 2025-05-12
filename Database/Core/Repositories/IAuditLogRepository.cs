using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;

namespace Database.Core.Repositories
{
    /// <summary>
    /// Repository interface for managing <see cref="AuditLog"/> entities.
    /// Inherits basic CRUD operations from <see cref="IRepository{T}"/>
    /// </summary>

    public interface IAuditLogRepository : IRepository<AuditLog>
    {
    }
}
