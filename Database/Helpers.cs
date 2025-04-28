using Database.Core.Repositories;
using Database.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public static class Helpers
    {
        public static object GetRepositoryForType(Type entityType)
        {
            var _unitOfWork = new UnitOfWork();
            var unitOfWorkType = typeof(UnitOfWork);
            var properties = unitOfWorkType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var repoInstance = prop.GetValue(_unitOfWork);
                if (repoInstance == null) continue;

                var interfaces = repoInstance.GetType().GetInterfaces();

                foreach (var iface in interfaces)
                {
                    if (!iface.IsGenericType) continue;

                    var genericDef = iface.GetGenericTypeDefinition();
                    var genericArgs = iface.GetGenericArguments();

                    if ((genericDef == typeof(IRepository<>)
                         || genericDef.Name.Contains("Repository"))
                        && genericArgs.Length == 1
                        && genericArgs[0] == entityType)
                    {
                        return repoInstance;
                    }
                }

                // Also check non-generic interfaces like IEquipmentRepository
                if (interfaces.Any(i => i.Name.Contains(entityType.Name)))
                {
                    return repoInstance;
                }
            }

            return null;
        }

    }
}
