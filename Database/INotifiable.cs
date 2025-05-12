using Database.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public interface INotifiable
    {
        IEnumerable<Notification> GetPendingNotifications();

    }
}
