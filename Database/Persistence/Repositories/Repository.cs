using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.Core.Repositories;
using Database.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Database.Persistence.Repositories;

internal partial class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly RentalDBContext context;

    public Repository(RentalDBContext context)
    {
        this.context = context;
    }

    public TEntity Get(int id)
    {
        return context.Set<TEntity>().Find(id);
    }

    public IEnumerable<TEntity> GetAll()
    {
        return context.Set<TEntity>().ToList();
    }

    public PaginatedResult GetAll(int pageNumber, int pageSize)
    {
        IQueryable<object> query = context.Set<TEntity>();

        return GetPaginatedResult(query, pageNumber, pageSize);
    }


    public IEnumerable<TEntity> Find(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
    {
        return context.Set<TEntity>().Where(predicate);
    }

    public TEntity SingleOrDefault(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
    {
        return context.Set<TEntity>().SingleOrDefault(predicate);
    }

    public void Add(TEntity entity)
    {
        context.Set<TEntity>().Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        context.Set<TEntity>().AddRange(entities);
    }

    public void Remove(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        context.Set<TEntity>().RemoveRange(entities);
    }

    public List<String> GetEntityColumnsReflection()
    {
        return typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name)
            .ToList();
    }
}