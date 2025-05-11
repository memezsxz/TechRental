using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    internal class SystemErrorLogRepository : Repository<SystemErrorLog>, ISystemErrorLogRepository
    {
        public SystemErrorLogRepository(RentalDBContext context, int? userId) : base(context, userId)
        {
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }

        #region Search

        public override Dictionary<string, string> GetEntityColumnsWithTypes()
        {
            var d = base.GetEntityColumnsWithTypes();

            if (d.ContainsKey("UserId")) d["UserId"] = "Int32";
            
            return d;
        }

        public override IQueryable<object> SelectViewColumns(IQueryable query)
        {
            query = query.Cast<SystemErrorLog>().Select(e => new
            {
                Id = e.Id,
                UserId = e.UserId,
                ErrorSource = e.ErrorSource,
                SourceProcedure = e.SourceProcedure,
                Timestamp = e.Timestamp,
            });

            return query.Cast<object>();
        }

        #endregion
    }
}