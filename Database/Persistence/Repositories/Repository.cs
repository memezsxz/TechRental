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

    #region Main
    public TEntity? Get(int id)
    {
        try
        {
            return context.Set<TEntity>().Find(id);

        }
        catch (Exception e)
        {
            return null;
        }
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

    public void Update(TEntity entity)
    {
        if (entity is IToBeTracked trackableEntity)
        {
            LogUpdate(trackableEntity.GenerateLogDetails());
        }

        context.Set<TEntity>().Update(entity);
    }
    private async Task LogUpdate(string details)
    {
        Console.WriteLine("Logged");
        //var log = new AuditLog
        //{
        //    //EntityName = typeof(TEntity).Name,
        //    //Action = "Update",
        //    //ActionTime = DateTime.UtcNow,
        //    //Details = details
        //};

        //await context.Set<AuditLog>().AddAsync(log);
    }

    #endregion

    #region Async

    public async Task<TEntity?> GetAsync(int id)
    {
        try
        {
            return await context.Set<TEntity>().FindAsync(id);

        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await context.Set<TEntity>().ToListAsync();
    }

    public async Task<PaginatedResult> GetAllAsync(int pageNumber, int pageSize)
    {
        IQueryable<object> query = context.Set<TEntity>();
        return await GetPaginatedResultAsync(query, pageNumber, pageSize);
    }
    public async Task AddAsync(TEntity entity)
    {
        await context.Set<TEntity>().AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await context.Set<TEntity>().AddRangeAsync(entities);
    }

    public async Task RemoveAsync(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
        await Task.CompletedTask; // Nothing async needed for Remove, but keeps the signature uniform
    }

    public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
    {
        context.Set<TEntity>().RemoveRange(entities);
        await Task.CompletedTask;
    }

    public async Task UpdateAsync(TEntity entity)
    {
        if (entity is IToBeTracked trackableEntity)
        {
            await LogUpdateAsync(trackableEntity.GenerateLogDetails());
        }

        context.Set<TEntity>().Update(entity);
        await Task.CompletedTask;
    }

    private async Task LogUpdateAsync(string details)
    {
        var log = new AuditLog
        {
            //EntityName = typeof(TEntity).Name,
            //Action = "Update",
            //ActionTime = DateTime.UtcNow,
            //Details = details
        };

        await context.Set<AuditLog>().AddAsync(log);
    }

    #endregion

    #region Columns

        public List<String> GetEntityColumnsReflection()
    {
        return typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name)
            .ToList();
    }


    #endregion
}