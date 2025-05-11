using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;

namespace Database
{
    public interface IToBeTracked
    {
      IEnumerable<AuditLog> GetAuditLogsFromTrackedChanges();
    }

}
