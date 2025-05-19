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
    /// <summary>
    /// Repository implementation for managing Category entities.
    /// </summary>
    internal class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        #region Constructor

        public CategoryRepository(RentalDBContext context, int userId)
            : base(context, userId)
        {
        }

        #endregion

        #region Context Accessor

        /// <summary>
        /// Gets the current database context cast to <see cref="RentalDBContext"/>.
        /// </summary>
        public RentalDBContext RentalDBContext => context as RentalDBContext;

        #endregion

        #region ICategoryRepository Implementation

        /// <inheritdoc/>
        public bool IsReferenced(int id)
        {
            return RentalDBContext.Equipment.Any(e => e.CategoryId == id);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(int id)
        {
            return await RentalDBContext.Categories.AnyAsync(c => c != null && c.Id == id);
        }

        #endregion

        #region IStatus Implementation (ICategoryRepository)

        /// <inheritdoc/>
        public Dictionary<int, string> GetAllByName()
        {
            return RentalDBContext.Categories
                .ToDictionary(c => c.Id, c => c.Name);
        }

        #endregion

        #region View Projection

        /// <inheritdoc/>
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

        #region Metadata Overrides

        /// <inheritdoc/>
        protected override bool ShouldIgnoreProperty(string propertyName)
        {
            return propertyName switch
            {
                nameof(Category.CreatedAt) => true,
                nameof(Category.UpdatedAt) => true,
                _ => base.ShouldIgnoreProperty(propertyName)
            };
        }

        #endregion
    }
}
