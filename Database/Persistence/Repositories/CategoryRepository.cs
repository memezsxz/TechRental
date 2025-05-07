using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Database.Persistence.Repositories
{
    internal class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(RentalDBContext context) : base(context)
        {
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }


        #region Async
        public bool IsReferenced(int id)
        {
            return RentalDBContext.Equipment.Any(e => e.CategoryId == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await RentalDBContext.Categories.AnyAsync(c => c != null && c.Id == id);
        }


        #endregion

        #region IStatus

        public Dictionary<int, string> GetAllByName()
        {
            return RentalDBContext.Categories
                .ToDictionary(c => c.Id, c => c.Name);
        }

        public override IQueryable<object> SelectViewColumns(IQueryable query)
        {
            query = query.Cast<Category>().Select(c => new 
            {
                Id = c.Id,
                Name = c.Name,
                IsActive = c.IsActive ?? false
            });

            return query.Cast<object>();
        }

        #endregion
    }
}
