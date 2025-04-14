using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.Persistence;

namespace Database.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        PaginatedResult GetAll(int pageNumber, int pageSize);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        public List<String> GetEntityColumnsReflection();

        public Dictionary<string, string> GetEntityColumnsWithTypes();

        public PaginatedResult SearchByColumn(string columnName, string value, int pageNumber, int pageSize, string comparisonOperator = "");
        public IQueryable<object> SelectViewColumns(IQueryable query);
    }
}
