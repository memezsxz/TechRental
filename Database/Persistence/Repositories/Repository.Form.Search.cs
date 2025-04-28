using System.Linq.Expressions;
using System.Reflection;
using Database.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Database.Persistence.Repositories;

internal partial class Repository<TEntity>
{
    #region Metadata

    /// <summary>
    /// Retrieves all scalar and navigation properties for the current repository entity type,
    /// along with their corresponding data types or referenced entity types.
    ///
    /// For scalar properties, the method returns the underlying CLR type name.
    /// For foreign key properties, it attempts to resolve and return the name of the corresponding
    /// DbSet property in the DbContext. If no matching DbSet is found, the referenced entity class name is returned.
    ///
    /// This method is typically used for dynamically generating UI filters, column definitions,
    /// or performing type-aware operations on entity metadata.
    ///
    /// </summary>
    /// <returns>
    /// A dictionary where each key represents a property name of the entity, and each value
    /// represents its resolved type name (e.g., "String", "Int32", or "Category").
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the entity type <typeparamref name="TEntity"/> is not registered in the DbContext model,
    /// indicating a missing DbSet declaration in the context.
    /// </exception>
    public virtual Dictionary<string, string> GetEntityColumnsWithTypes()
    {
        var entityType = context.Model.FindEntityType(typeof(TEntity)); // get the entity from the context

        // if the entity is not found throw an error
        if (entityType == null)
            throw new InvalidOperationException(
                $"Entity type '{typeof(TEntity).Name}' was not found in the DbContext. " +
                $"Ensure a DbSet<{typeof(TEntity).Name}> exists in your context class."
            );

        // if the entity if found, create the properties dictionary 
        return entityType.GetProperties()
            .ToDictionary(p => p.Name, p =>
            {
                // Check if the property is a foreign key
                var foreignKey = p.GetContainingForeignKeys().FirstOrDefault();

                // if the property is not a foreign key return the actual data type and handle nullable types
                if (foreignKey == null) return Nullable.GetUnderlyingType(p.ClrType)?.Name ?? p.ClrType.Name;

                //else Get the referenced entity type
                var referencedEntityType = foreignKey.PrincipalEntityType.ClrType;

                // Get the DbSet<> property name from the context (e.g., "Category" instead of "Categories" )
                var dbSetName = context.GetType().GetProperties()
                    .FirstOrDefault(prop => prop.PropertyType == typeof(DbSet<>).MakeGenericType(referencedEntityType))
                    ?.Name;

                return dbSetName ?? referencedEntityType.Name; // Fallback to entity name if dbSet name is not available
            });
    }

    /// <summary>
    /// Retrieves a <see cref="PropertyInfo"/> for the given column name from the <typeparamref name="TEntity"/> type.
    /// The search is case-insensitive and includes only public instance properties.
    /// </summary>
    /// <param name="columnName">The name of the property to retrieve.</param>
    /// <returns>The matching <see cref="PropertyInfo"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown if the specified property is not found.</exception>
    private PropertyInfo GetProperty(string columnName)
    {
        var property = typeof(TEntity).GetProperty(
            columnName,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
        );

        if (property == null)
            throw new ArgumentException($"Property '{columnName}' not found on {typeof(TEntity).Name}");

        return property;
    }

    /// <summary>
    /// Constructs an expression to access a property on the entity, supporting scalar and navigation properties.
    /// For navigation properties, the expression accesses the nested foreign key "Id".
    /// </summary>
    /// <param name="parameter">The root parameter expression representing the entity (e.g., 'x').</param>
    /// <param name="property">The target property on the entity.</param>
    /// <returns>
    /// A property access <see cref="Expression"/> for use in dynamic queries and predicates.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when a navigation property does not contain an accessible "Id" property.
    /// </exception>
    private Expression GetPropertyAccessExpression(ParameterExpression parameter, PropertyInfo property)
    {
        // If it's a navigation property (reference type that is not string)
        if (!property.PropertyType.IsValueType && property.PropertyType != typeof(string))
        {
            var foreignEntityType = property.PropertyType;
            var idProperty = foreignEntityType.GetProperty("Id",
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            // Access x.Property.Id
            if (idProperty != null)
            {
                return Expression.Property(Expression.Property(parameter, property), idProperty);
            }

            throw new ArgumentException(
                $"Foreign entity '{foreignEntityType.Name}' does not contain an 'Id' column.");
        }

        // If it's already a foreign key (e.g., int CategoryId)
        if (property.Name.EndsWith("Id") && property.PropertyType == typeof(int))
        {
            return Expression.Property(parameter, property);
        }

        // For scalar properties like Name, Price, etc.
        return Expression.Property(parameter, property);
    }

    #endregion

    #region Extension Points

    /// <summary>
    /// Allows derived repositories to override which columns are selected in the result set.
    /// By default, returns the full entity.
    /// </summary>
    /// <param name="query">The <see cref="IQueryable"/> to project.</param>
    /// <returns>An <see cref="IQueryable"/> that selects specific view columns.</returns>
    public virtual IQueryable<object> SelectViewColumns(IQueryable query)
    {
        return query.Cast<TEntity>();
    }

    #endregion

    #region Search Methods

    /// <summary>
    /// Searches for entities where the specified column matches the given value.
    /// Supports dynamic filtering based on property name and value.
    /// </summary>
    /// <param name="columnName">The name of the column to filter on.</param>
    /// <param name="value">The value to compare against.</param>
    /// <param name="pageNumber">The page number for paginated results (1-based).</param>
    /// <param name="pageSize">The number of records per page.</param>
    /// <returns>
    /// A <see cref="PaginatedResult"/> containing the filtered, paginated result set.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the column name does not exist or the value cannot be parsed.
    /// </exception>
    public PaginatedResult SearchByColumn(string columnName, string value, int pageNumber, int pageSize,
        string comparisonOperator = "")
    {
        // Build the filter expression (e.g., x => x.Property == value or x.Property.Contains(value))
        var predicate = string.IsNullOrEmpty(comparisonOperator)
            ? BuildSearchPredicate(columnName, value)
            : BuildSearchPredicate(columnName, value, comparisonOperator);

        // Apply the filter to the DbSet query
        IQueryable<object> query = context.Set<TEntity>().Where(predicate);

        // Return paginated result
        return GetPaginatedResult(query, pageNumber, pageSize);
    }

    /// <summary>
    /// Searches for entities using a comparison operator (e.g., "==", ">", "between") on the specified column.
    /// Supports type-aware and nullable-safe comparisons with advanced operations.
    /// </summary>
    /// <param name="columnName">The name of the column to filter on.</param>
    /// <param name="value">
    /// The value or range to filter by. If using "between", provide two comma-separated values.
    /// </param>
    /// <param name="pageNumber">The page number for paginated results (1-based).</param>
    /// <param name="pageSize">The number of records per page.</param>
    /// <param name="comparisonOperator">
    /// A string indicating the comparison operator ("==", ">", "<", ">=", "<=", "between").
    /// </param>
    /// <returns>
    /// A <see cref="PaginatedResult"/> containing the filtered, paginated result set.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the column name is invalid, the value format is incorrect,
    /// or the comparison operator is not supported.
    /// </exception>
    public PaginatedResult SearchByColumnOperation(string columnName, string value, int pageNumber, int pageSize,
        string comparisonOperator)
    {
        // Build the predicate expression using the specified operator
        var predicate = BuildSearchPredicate(columnName, value, comparisonOperator);

        // Apply the predicate to the query
        IQueryable<object> query = context.Set<TEntity>().Where(predicate);

        // Return paginated result
        return GetPaginatedResult(query, pageNumber, pageSize);
    }

    #endregion

    #region Predicate Builders

    /// <summary>
    /// Builds a strongly-typed predicate expression used to filter entities based on a column,
    /// a string value, and a specified comparison operator. Supports string-based matching,
    /// nullable comparisons, and range filtering with "between".
    /// </summary>
    /// <param name="columnName">The name of the entity property to filter.</param>
    /// <param name="value">
    /// The value to filter by. For the "between" operator, this should be two comma-separated values.
    /// </param>
    /// <param name="comparisonOperator">
    /// A string representing the comparison operator. Supported: "==", "&gt;", "&lt;", "&gt;=", "&lt;=", "between".
    /// </param>
    /// <returns>
    /// A compiled <see cref="Expression{Func{TEntity, Boolean}}"/> that can be applied to a LINQ Where clause.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the column name is invalid, the value format is incorrect,
    /// or the comparison operator is unsupported.
    /// </exception>
    private Expression<Func<TEntity, bool>> BuildSearchPredicate(string columnName, string value,
        string comparisonOperator = "==")
    {
        // Declare the input parameter for the lambda (e.g., 'x')
        var parameter = Expression.Parameter(typeof(TEntity), "x");

        // Resolve the target property (e.g., x.Name)
        var property = GetProperty(columnName);

        // Build the access expression (handles both direct and navigation properties)
        var propertyAccess = GetPropertyAccessExpression(parameter, property);

        Expression predicate;

        // Use string.Contains() for default string equality comparisons
        if (property.PropertyType == typeof(string) && comparisonOperator == "==")
        {
            predicate = BuildStringPredicate(propertyAccess, value);
        }
        // Handle "between" operator for range-based comparisons
        else if (comparisonOperator == "between")
        {
            var values = value.Split(',');
            if (values.Length != 2)
                throw new ArgumentException("Between filter requires two values separated by a comma.");

            // Convert string values to the correct property type
            var lower = Convert.ChangeType(values[0],
                Nullable.GetUnderlyingType(propertyAccess.Type) ?? propertyAccess.Type);
            var upper = Convert.ChangeType(values[1],
                Nullable.GetUnderlyingType(propertyAccess.Type) ?? propertyAccess.Type);

            predicate = BuildBetweenPredicate(propertyAccess, lower, upper);
        }
        else
        {
            // Handle other operators (==, >, <, etc.)
            var typedValue = Convert.ChangeType(value,
                Nullable.GetUnderlyingType(propertyAccess.Type) ?? propertyAccess.Type);
            predicate = BuildTypedPredicate(propertyAccess, typedValue, comparisonOperator);
        }

        // Build and return the lambda: x => x.Property <op> value
        return Expression.Lambda<Func<TEntity, bool>>(predicate, parameter);
    }

    /// <summary>
    /// Builds a predicate that applies a <c>Contains</c> operation for string properties,
    /// enabling substring filtering functionality (e.g., x.Name.Contains("abc")).
    /// </summary>
    /// <param name="propertyAccess">The property access expression (e.g., x.Name).</param>
    /// <param name="value">The substring value to search for.</param>
    /// <returns>
    /// A <see cref="MethodCallExpression"/> representing the <c>Contains</c> method call on a string.
    /// </returns>
    private Expression BuildStringPredicate(Expression propertyAccess, string value)
    {
        // Resolve the string.Contains method
        var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });

        // Define the constant string value to search for
        var constant = Expression.Constant(value, typeof(string));

        // Return x.Property.Contains(value)
        return Expression.Call(propertyAccess, method!, constant);
    }

    /// <summary>
    /// Builds a comparison predicate for scalar or nullable entity properties,
    /// using a specified operator such as "==", "&gt;", or "&lt;".
    /// Handles nullable types by checking for value presence before comparison.
    /// </summary>
    /// <param name="propertyAccess">The property access expression (e.g., x.Quantity).</param>
    /// <param name="typedValue">The value to compare the property against, already converted to the correct type.</param>
    /// <param name="op">
    /// A string representing the comparison operator. Supported: "==", "&gt;", "&lt;", "&gt;=", "&lt;=", "!=".
    /// </param>
    /// <returns>
    /// A <see cref="BinaryExpression"/> or composite <see cref="Expression"/> supporting nullable comparisons.
    /// </returns>
    private Expression BuildTypedPredicate(Expression propertyAccess, object typedValue, string op)
    {
        // Resolve target type (unwrap Nullable<T> if needed)
        var targetType = Nullable.GetUnderlyingType(propertyAccess.Type) ?? propertyAccess.Type;

        // Create constant expression from value
        var constant = Expression.Constant(typedValue, targetType);

        // Handle nullable types using HasValue && Value
        if (Nullable.GetUnderlyingType(propertyAccess.Type) != null)
        {
            var hasValue = Expression.Property(propertyAccess, "HasValue");
            var valueProp = Expression.Property(propertyAccess, "Value");

            // Return: x.Property.HasValue && x.Property.Value <op> constant
            return Expression.AndAlso(
                hasValue,
                BuildComparisonExpression(valueProp, constant, op)
            );
        }

        // Return: x.Property <op> constant
        return BuildComparisonExpression(propertyAccess, constant, op);
    }

    /// <summary>
    /// Builds a range comparison predicate for a property using two boundary values.
    /// Supports nullable types by wrapping with HasValue check.
    /// </summary>
    /// <param name="propertyAccess">The property access expression (e.g., x.Price).</param>
    /// <param name="lower">The lower bound value for the comparison.</param>
    /// <param name="upper">The upper bound value for the comparison.</param>
    /// <returns>
    /// A composed <see cref="BinaryExpression"/> representing: (x.Property &gt;= lower &amp;&amp; x.Property &lt;= upper).
    /// </returns>
    private Expression BuildBetweenPredicate(Expression propertyAccess, object lower, object upper)
    {
        var type = Nullable.GetUnderlyingType(propertyAccess.Type) ?? propertyAccess.Type;

        // Create constant expressions for the bounds
        var const1 = Expression.Constant(lower, type);
        var const2 = Expression.Constant(upper, type);

        // Handle nullable value: x.Property.HasValue && x.Property.Value >= lower && <= upper
        if (Nullable.GetUnderlyingType(propertyAccess.Type) != null)
        {
            var hasValue = Expression.Property(propertyAccess, "HasValue");
            var value = Expression.Property(propertyAccess, "Value");

            return Expression.AndAlso(
                hasValue,
                Expression.AndAlso(
                    Expression.GreaterThanOrEqual(value, const1),
                    Expression.LessThanOrEqual(value, const2)
                )
            );
        }

        // Non-nullable: x.Property >= lower && x.Property <= upper
        return Expression.AndAlso(
            Expression.GreaterThanOrEqual(propertyAccess, const1),
            Expression.LessThanOrEqual(propertyAccess, const2)
        );
    }

    /// <summary>
    /// Constructs a binary expression for comparing two expressions based on the given operator string.
    /// Used internally by other predicate builders for scalar comparisons.
    /// </summary>
    /// <param name="left">The left-hand operand (typically a property access).</param>
    /// <param name="right">The right-hand operand (typically a constant).</param>
    /// <param name="op">
    /// A comparison operator string. Supported: "==", "!=", "&gt;", "&lt;", "&gt;=", "&lt;=".
    /// </param>
    /// <returns>
    /// A <see cref="BinaryExpression"/> corresponding to the specified operator.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if the operator is unsupported.</exception>
    private Expression BuildComparisonExpression(Expression left, Expression right, string op)
    {
        return op switch
        {
            "==" => Expression.Equal(left, right),
            "!=" => Expression.NotEqual(left, right),
            ">=" => Expression.GreaterThanOrEqual(left, right),
            "<=" => Expression.LessThanOrEqual(left, right),
            ">" => Expression.GreaterThan(left, right),
            "<" => Expression.LessThan(left, right),
            _ => throw new ArgumentException($"Unsupported comparison operator: {op}")
        };
    }

    #endregion

    #region Pagination
    /// <summary>
    /// Paginates the given query and returns a <see cref="PaginatedResult"/> with total records and pages.
    /// </summary>
    /// <param name="query">The filtered <see cref="IQueryable{T}"/> to paginate.</param>
    /// <param name="pageNumber">The page number (1-based).</param>
    /// <param name="pageSize">The number of records per page.</param>
    /// <returns>
    /// A <see cref="Task&lt;PaginatedResult&gt;"/> containing data, total records, and total pages.
    /// </returns>

    private async Task<PaginatedResult> GetPaginatedResultAsync(IQueryable<object> query, int pageNumber, int pageSize)
    {
        var totalRecords = await query.CountAsync();
        var records = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult
        {
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
            Data = records
        };
    }


    /// <summary>
    /// Paginates the given query and returns a <see cref="PaginatedResult"/> with total records and pages.
    /// Also applies projection via <see cref="SelectViewColumns"/> if overridden.
    /// </summary>
    /// <param name="query">The filtered <see cref="IQueryable{T}"/> to paginate.</param>
    /// <param name="pageNumber">The page number (1-based).</param>
    /// <param name="pageSize">The number of records per page.</param>
    /// <returns>
    /// A <see cref="PaginatedResult"/> containing data, total records, and total pages.
    /// </returns>
    private PaginatedResult GetPaginatedResult(IQueryable<object> query, int pageNumber, int pageSize)
    {
        // Apply optional projection (e.g., selecting only specific columns)
        var repo = Helpers.GetRepositoryForType(typeof(TEntity));
        if (repo is IRepository<TEntity> typedRepo)
        {
            query = typedRepo.SelectViewColumns(query);
        }

        // Apply paging (skip/take)
        var pagedQuery = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsQueryable();

        // Materialize the full filtered query for counting
        var queryList = query.ToList();

        int totalRecords = queryList.Any() ? queryList.Count() : 0;
        int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        // Return paginated results
        return new PaginatedResult
        {
            Data = pagedQuery.ToList(),
            TotalRecords = totalRecords,
            TotalPages = totalPages
        };
    }

    #endregion
}