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
    /// Repository interface for managing <see cref="Image"/> entities.
    /// Inherits standard CRUD operations from <see cref="IRepository{T}"/>.
    /// </summary>

    public interface IImageRepository : IRepository<Image>
    {
    }
}
