using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.Core.Repositories;

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

        public Dictionary<int, string> GetAllByName()
        {
            return RentalDBContext.Categories
                .ToDictionary(c => c.Id, c => c.Name);
        }
    }
}
