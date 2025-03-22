using Database.Core.Domain;

namespace Database.Core.Repositories
{
    public interface IReturnConditionStatusRepository : IRepository<ReturnConditionStatus> , IStatus
    {
    }
}