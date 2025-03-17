using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Database.Core.Domain;
using Database.Core.Repositories;
using Database.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Database.Persistence.Repositories
{
    internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class
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

        public PaginatedResult<TEntity> GetAll(int pageNumber, int pageSize)
        {
            var query = context.Set<TEntity>();

            int totalRecords = query.Count(); // Get total records count
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var data = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResult<TEntity>
            {
                Data = data,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
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

        public Dictionary<string, string> GetEntityColumnsWithTypes()
        {
            var entityType = context.Model.FindEntityType(typeof(TEntity));

            if (entityType == null)
                return new Dictionary<string, string>();

            return entityType.GetProperties()
                .ToDictionary(p => p.Name, p =>
                {
                    // Check if the property is a foreign key
                    var foreignKey = p.GetContainingForeignKeys().FirstOrDefault();

                    if (foreignKey != null)
                    {
                        // Get the referenced entity type
                        var referencedEntityType = foreignKey.PrincipalEntityType.ClrType;

                        // Get the DbSet<> property name from the context (e.g., "Categories" instead of "Category")
                        var dbSetName = context.GetType().GetProperties()
                            .FirstOrDefault(prop => prop.PropertyType == typeof(DbSet<>).MakeGenericType(referencedEntityType))
                            ?.Name;

                        return dbSetName ?? referencedEntityType.Name; // Fallback to entity name
                    }

                    // Otherwise, return the actual data type (handle nullable types)
                    return Nullable.GetUnderlyingType(p.ClrType)?.Name ?? p.ClrType.Name;
                });
        }

        public PaginatedResult<TEntity> SearchByColumn(string columnName, string value, int pageNumber, int pageSize)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");

            Expression propertyAccess;

            // Check if the columnName refers to a foreign key navigation property
            var property = typeof(TEntity).GetProperty(columnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null)
                throw new ArgumentException($"Property '{columnName}' not found on {typeof(TEntity).Name}");

            // If the property is a navigation property, get the "Id" column from it
            if (!property.PropertyType.IsValueType && property.PropertyType != typeof(string))
            {
                var foreignEntityType = property.PropertyType;
                var idProperty = foreignEntityType.GetProperty("Id", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (idProperty != null)
                {
                    // Navigate to the foreign entity, then select "Id"
                    propertyAccess = Expression.Property(Expression.Property(parameter, property), idProperty);
                }
                else
                {
                    throw new ArgumentException($"Foreign entity '{foreignEntityType.Name}' does not contain an 'Id' column.");
                }
            }
            else
            {
                // Regular property (not a navigation property)
                propertyAccess = Expression.Property(parameter, property);
            }

            // Convert the value to the correct type
            object typedValue;
            Type propertyType = Nullable.GetUnderlyingType(propertyAccess.Type) ?? propertyAccess.Type;

            if (propertyType == typeof(int))
                typedValue = int.Parse(value);
            else if (propertyType == typeof(decimal))
                typedValue = decimal.Parse(value);
            else if (propertyType == typeof(bool))
                typedValue = bool.Parse(value);
            else if (propertyType == typeof(DateTime))
                typedValue = DateTime.Parse(value);
            else
                typedValue = value;

            var constant = Expression.Constant(typedValue, propertyType);

            // Handle Nullable properties
            Expression equalExpression;
            if (Nullable.GetUnderlyingType(propertyAccess.Type) != null)
            {
                var hasValueProperty = Expression.Property(propertyAccess, "HasValue");
                var valueProperty = Expression.Property(propertyAccess, "Value");

                var condition = Expression.AndAlso(
                    hasValueProperty,
                    Expression.Equal(valueProperty, constant)
                );

                equalExpression = condition;
            }
            else
            {
                equalExpression = Expression.Equal(propertyAccess, constant);
            }

            var lambda = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
            var query = context.Set<TEntity>().Where(lambda);

            int totalRecords = query.Count(); // Get total records matching the filter
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var data = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResult<TEntity>
            {
                Data = data,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        }




    }
}
