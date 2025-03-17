using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Core.Repositories
{
    public interface IStatus
    {
        public Dictionary<int, string> GetAllByName();
    }
}
