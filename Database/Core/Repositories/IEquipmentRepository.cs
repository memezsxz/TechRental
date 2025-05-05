using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.ViewModels;

namespace Database.Core.Repositories
{
    public interface IEquipmentRepository : IRepository<Equipment>, IStatus
    {
        Task<IEnumerable<Equipment>> GetAllWithDetailsAsync();
        Task<Equipment?> GetByIdWithDetailsAsync(int id);
        Task<Equipment?> GetEquipmentWithImageAsync(int id);
        Task<bool> EquipmentExistsAsync(int id);

        Task<List<TopRentedEquipmentStats>> GetTop5RentedEquipmentStatsAsync();

    }
}
