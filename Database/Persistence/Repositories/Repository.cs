using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.Core.Repositories;
using Database.Persistence;
using Database.Search;
using Microsoft.EntityFrameworkCore;

namespace Database.Persistence.Repositories;

internal partial class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{

    protected readonly RentalDBContext context;
    public int UserId { get; set; }

    public Repository(RentalDBContext context, int userId)
    {
        this.context = context;
        this.UserId = userId;
    }

    #region Main


    public  TEntity? Get(int id)
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

    public  IEnumerable<TEntity> GetAll()
    {
        return context.Set<TEntity>().ToList();
    }

    public  PaginatedResult GetAll(int pageNumber, int pageSize)
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
        foreach (TEntity entity in entities) Add(entity);
    }

    public void Remove(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        foreach (TEntity entity in entities) Remove(entity);
    }

    public void Update(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
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
        await Task.CompletedTask; 
    }

    public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
    {
        context.Set<TEntity>().RemoveRange(entities);
        await Task.CompletedTask;
    }

    public async Task UpdateAsync(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
        await Task.CompletedTask;
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

    public virtual IEnumerable<AuditLog> GetAuditLogsFromTrackedChanges()
    {
        var entries = context.ChangeTracker.Entries<TEntity>()
            .Where(e => e.State != EntityState.Unchanged && e.State != EntityState.Detached && e.State != EntityState.Added);

        //Console.WriteLine($"Count from get in repo {entries.Count()}");

        foreach (var entry in entries)
        {
            var before = new Dictionary<string, object>();
            var after = new Dictionary<string, object>();

            switch (entry.State)
            {
                case EntityState.Added:
                    foreach (var prop in entry.Properties)
                    {
                        if (ShouldIgnoreProperty(prop.Metadata.Name))
                            continue;

                        after[prop.Metadata.Name] = prop.CurrentValue ?? "null";
                    }
                    break;

                case EntityState.Deleted:
                    foreach (var prop in entry.Properties)
                    {
                        if (ShouldIgnoreProperty(prop.Metadata.Name))
                            continue;

                        before[prop.Metadata.Name] = prop.OriginalValue ?? "null";
                    }
                    break;

                case EntityState.Modified:
                    foreach (var prop in entry.Properties)
                    {
                        if (ShouldIgnoreProperty(prop.Metadata.Name))
                            continue;

                        if (prop.IsModified && !object.Equals(prop.OriginalValue, prop.CurrentValue))
                        {
                            before[prop.Metadata.Name] = prop.OriginalValue ?? "null";
                            after[prop.Metadata.Name] = prop.CurrentValue ?? "null";
                        }
                    }
                    break;
            }

            // Skip if no changes were captured
            if (entry.State == EntityState.Modified && before.Count == 0)
                continue;

            yield return new AuditLog
            {
                ActionType = entry.State.ToString(),
                AffectedRecordKey = entry.Property("Id").CurrentValue?.ToString()
                                 ?? entry.Property("Id").OriginalValue?.ToString()
                                 ?? "0",
                DataBeforeAction = before.Count > 0 ? JsonSerializer.Serialize(before) : null,
                DataAfterAction = after.Count > 0 ? JsonSerializer.Serialize(after) : null,
                Timestamp = DateTime.Now,
                SourceEntity = typeof(TEntity).Name,
                Source = "FormsApp",
                UserId = UserId
            };
        }
    }
    protected virtual bool ShouldIgnoreProperty(string propertyName)
    {
        // Default: Don't ignore anything (can be overridden per repository)
        return false;
    }
}
