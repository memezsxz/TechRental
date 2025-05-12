using Database.Core.Repositories;
using Database.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Database.Search
{
    /// <summary>
    /// Provides utility methods for resolving repositories dynamically at runtime.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Attempts to resolve the appropriate repository instance from <see cref="UnitOfWork"/>
        /// for the given entity type.
        /// </summary>
        /// <param name="entityType">The entity type for which a repository is required.</param>
        /// <returns>
        /// An instance of the repository that manages the specified entity type,
        /// or <c>null</c> if no matching repository is found.
        /// </returns>
        public static object GetRepositoryForType(Type entityType)
        {
            // Create a UnitOfWork instance to access repositories
            var _unitOfWork = new UnitOfWork();
            var unitOfWorkType = typeof(UnitOfWork);
            var properties = unitOfWorkType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Iterate over all public properties of UnitOfWork
            foreach (var prop in properties)
            {
                // Try to retrieve the actual repository instance
                var repoInstance = prop.GetValue(_unitOfWork);
                if (repoInstance == null) continue;

                // Get all interfaces implemented by the repository
                var interfaces = repoInstance.GetType().GetInterfaces();

                foreach (var iface in interfaces)
                {
                    // Skip non-generic interfaces here
                    if (!iface.IsGenericType) continue;

                    var genericDef = iface.GetGenericTypeDefinition();
                    var genericArgs = iface.GetGenericArguments();

                    // Match generic IRepository<T> or other interfaces containing the entity type
                    if ((genericDef == typeof(IRepository<>)
                         || genericDef.Name.Contains("Repository"))
                        && genericArgs.Length == 1
                        && genericArgs[0] == entityType)
                    {
                        return repoInstance;
                    }
                }

                // Fallback: match by entity name against custom repository interface name (e.g., IEquipmentRepository)
                if (interfaces.Any(i => i.Name.Contains(entityType.Name)))
                {
                    return repoInstance;
                }
            }

            // No matching repository found
            return null;
        }
    }
}
