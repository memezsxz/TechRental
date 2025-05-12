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
    public interface ICategoryRepository : IRepository<Category>, IStatus, IToBeTracked
    {
        #region Main

        Task<bool> ExistsAsync(int id);
        public bool IsReferenced(int id);


        #endregion

    }
}
