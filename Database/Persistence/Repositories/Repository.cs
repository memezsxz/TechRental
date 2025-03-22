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

                        // Get the DbSet<> property name from the context (e.g., "Category" instead of "Category")
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

            var property = typeof(TEntity).GetProperty(columnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
                throw new ArgumentException($"Property '{columnName}' not found on {typeof(TEntity).Name}");

            if (!property.PropertyType.IsValueType && property.PropertyType != typeof(string))
            {
                var foreignEntityType = property.PropertyType;
                var idProperty = foreignEntityType.GetProperty("Id", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (idProperty != null)
                {
                    propertyAccess = Expression.Property(Expression.Property(parameter, property), idProperty);
                }
                else
                {
                    throw new ArgumentException($"Foreign entity '{foreignEntityType.Name}' does not contain an 'Id' column.");
                }
            }
            else
            {
                propertyAccess = Expression.Property(parameter, property);
            }

            Expression predicate;
            if (property.PropertyType == typeof(string))
            {
                var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var constant = Expression.Constant(value, typeof(string));
                predicate = Expression.Call(propertyAccess, method, constant);
            }
            else
            {
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

                if (Nullable.GetUnderlyingType(propertyAccess.Type) != null)
                {
                    var hasValueProperty = Expression.Property(propertyAccess, "HasValue");
                    var valueProperty = Expression.Property(propertyAccess, "Value");

                    predicate = Expression.AndAlso(
                        hasValueProperty,
                        Expression.Equal(valueProperty, constant)
                    );
                }
                else
                {
                    predicate = Expression.Equal(propertyAccess, constant);
                }
            }

            var lambda = Expression.Lambda<Func<TEntity, bool>>(predicate, parameter);
            var query = context.Set<TEntity>().Where(lambda);

            int totalRecords = query.Count();
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




        public PaginatedResult<TEntity> SearchByColumnOperation(string columnName, string value, int pageNumber, int pageSize, string comparisonOperator)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            Expression propertyAccess;

            var property = typeof(TEntity).GetProperty(columnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
                throw new ArgumentException($"Property '{columnName}' not found on {typeof(TEntity).Name}.");

            // Handle Foreign Key (Navigation Property)
            if (!property.PropertyType.IsValueType && property.PropertyType != typeof(string))
            {
                var foreignEntityType = property.PropertyType;
                var idProperty = foreignEntityType.GetProperty("Id", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (idProperty != null)
                {
                    propertyAccess = Expression.Property(Expression.Property(parameter, property), idProperty);
                }
                else
                {
                    throw new ArgumentException($"Foreign entity '{foreignEntityType.Name}' does not contain an 'Id' column.");
                }
            }
            else
            {
                propertyAccess = Expression.Property(parameter, property);
            }

            Expression predicate;
            Type propertyType = Nullable.GetUnderlyingType(propertyAccess.Type) ?? propertyAccess.Type;
            object typedValue;
            object typedValue2 = null;

            bool isBetween = comparisonOperator == "between";

            // Handle "between" operation
            if (isBetween)
            {
                var values = value.Split(',');
                if (values.Length != 2) throw new ArgumentException("Between filter requires two values separated by a comma.");

                typedValue = Convert.ChangeType(values[0], propertyType);
                typedValue2 = Convert.ChangeType(values[1], propertyType);
            }
            else
            {
                typedValue = Convert.ChangeType(value, propertyType);
            }

            var constant1 = Expression.Constant(typedValue, propertyType);
            var constant2 = isBetween ? Expression.Constant(typedValue2, propertyType) : null;

            // Handle Nullable<T> properties
            if (Nullable.GetUnderlyingType(propertyAccess.Type) != null)
            {
                var hasValueProperty = Expression.Property(propertyAccess, "HasValue");
                var valueProperty = Expression.Property(propertyAccess, "Value");

                if (isBetween)
                {
                    predicate = Expression.AndAlso(
                        hasValueProperty,
                        Expression.AndAlso(
                            Expression.GreaterThanOrEqual(valueProperty, constant1),
                            Expression.LessThanOrEqual(valueProperty, constant2)
                        )
                    );
                }
                else
                {
                    predicate = Expression.AndAlso(
                        hasValueProperty,
                        BuildComparisonExpression(valueProperty, constant1, comparisonOperator)
                    );
                }
            }
            else
            {
                predicate = isBetween
                    ? Expression.AndAlso(
                        Expression.GreaterThanOrEqual(propertyAccess, constant1),
                        Expression.LessThanOrEqual(propertyAccess, constant2)
                    )
                    : BuildComparisonExpression(propertyAccess, constant1, comparisonOperator);
            }

            var lambda = Expression.Lambda<Func<TEntity, bool>>(predicate, parameter);
            var query = context.Set<TEntity>().Where(lambda);

            int totalRecords = query.Count();
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


        private Expression BuildComparisonExpression(Expression left, Expression right, string op)
        {
            return op switch
            {
                "==" => Expression.Equal(left, right),
                ">=" => Expression.GreaterThanOrEqual(left, right),
                "<=" => Expression.LessThanOrEqual(left, right),
                ">" => Expression.GreaterThan(left, right),
                "<" => Expression.LessThan(left, right),
                _ => throw new ArgumentException($"Unsupported comparison operator: {op}")
            };
        }












        //public PaginatedResult<TEntity> SearchByColumn(string columnName, string value, int pageNumber, int pageSize)
        //{
        //    var property = GetProperty(columnName);
        //    var parameter = Expression.Parameter(typeof(TEntity), "x");
        //    var propertyAccess = GetPropertyAccessExpression(parameter, property);
        //    var predicate = BuildPredicate(property, propertyAccess, value);

        //    return ExecuteQuery(predicate, pageNumber, pageSize);
        //}

        private PropertyInfo GetProperty(string columnName)
        {
            var property = typeof(TEntity).GetProperty(columnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
                throw new ArgumentException($"Property '{columnName}' not found on {typeof(TEntity).Name}");

            return property;
        }

        private Expression GetPropertyAccessExpression(ParameterExpression parameter, PropertyInfo property)
        {
            // If property is a navigation property (complex type)
            if (!property.PropertyType.IsValueType && property.PropertyType != typeof(string))
            {
                var foreignEntityType = property.PropertyType;
                var idProperty = foreignEntityType.GetProperty("Id", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (idProperty != null)
                {
                    // Navigate to the foreign entity, then select "Id"
                    return Expression.Property(Expression.Property(parameter, property), idProperty);
                }
                else
                {
                    throw new ArgumentException($"Foreign entity '{foreignEntityType.Name}' does not contain an 'Id' column.");
                }
            }

            // If property is already an int foreign key (CategoryId, UserId), use it directly
            if (property.Name.EndsWith("Id") && property.PropertyType == typeof(int))
            {
                return Expression.Property(parameter, property);
            }

            // Otherwise, return normal property
            return Expression.Property(parameter, property);
        }

        private Expression BuildPredicate(PropertyInfo property, Expression propertyAccess, string value)
        {
            Console.WriteLine(property.PropertyType);

            if (property.PropertyType == typeof(string))
            {
                var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var v1 = Expression.Constant(value, typeof(string));
                return Expression.Call(propertyAccess, method, v1);
            }

            // Check if it's a foreign key property (e.g., "CategoryId", "UserId")
            bool isForeignKey = property.Name.EndsWith("Id") && property.PropertyType == typeof(int);

            var typedValue = ConvertValue(value, propertyAccess.Type);
            var constant = Expression.Constant(typedValue, Nullable.GetUnderlyingType(propertyAccess.Type) ?? propertyAccess.Type);

            if (isForeignKey)
            {
                return Expression.Equal(propertyAccess, constant);
            }

            // Handle Nullable<T> types properly
            if (Nullable.GetUnderlyingType(propertyAccess.Type) != null)
            {
                var hasValueProperty = Expression.Property(propertyAccess, "HasValue");
                var valueProperty = Expression.Property(propertyAccess, "Value");

                return Expression.AndAlso(hasValueProperty, Expression.Equal(valueProperty, constant));
            }

            return Expression.Equal(propertyAccess, constant);
        }

        private object ConvertValue(string value, Type targetType)
        {
            targetType = Nullable.GetUnderlyingType(targetType) ?? targetType;
            if (targetType == typeof(int)) return int.Parse(value);
            if (targetType == typeof(decimal)) return decimal.Parse(value);
            if (targetType == typeof(bool)) return bool.Parse(value);
            if (targetType == typeof(DateTime)) return DateTime.Parse(value);
            return value;
        }

        private PaginatedResult<TEntity> ExecuteQuery(Expression predicate, int pageNumber, int pageSize)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var lambda = Expression.Lambda<Func<TEntity, bool>>(predicate, parameter);

            // Ensure query remains IQueryable (EF Core compatible)
            var query = context.Set<TEntity>().Where(lambda);

            int totalRecords = query.Count(); // Keep Count() within IQueryable
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Apply pagination directly on IQueryable, avoiding early ToList()
            var data = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();  // Execute ToList() only at the final stage

            return new PaginatedResult<TEntity>
            {
                Data = data,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        }


    }
}
