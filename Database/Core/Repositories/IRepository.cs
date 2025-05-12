using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.Persistence;
using Database.Search;

namespace Database.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {

        #region Fields
        public int? UserId { get; set; }

        #endregion


        #region Main

        TEntity? Get(int id);
        IEnumerable<TEntity> GetAll();
        PaginatedResult GetAll(int pageNumber, int pageSize);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);

        #endregion

        #region Async

        Task<TEntity> GetAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<PaginatedResult> GetAllAsync(int pageNumber, int pageSize);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task RemoveAsync(TEntity entity);
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        #endregion

        #region Search

        public List<String> GetEntityColumnsReflection();

        public Dictionary<string, string> GetEntityColumnsWithTypes();

        public PaginatedResult SearchByColumn(string columnName, string value, int pageNumber, int pageSize, string comparisonOperator = "");
        public IQueryable<object> SelectViewColumns(IQueryable query);

        #endregion

        //protected bool ShouldIgnoreProperty(string propertyName);

    }
}
