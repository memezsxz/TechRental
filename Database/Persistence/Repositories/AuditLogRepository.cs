using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    internal class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(RentalDBContext context, int? userId) : base(context, userId)
        {
        }

        public RentalDBContext RentalDBContext
        {
            get { return context as RentalDBContext; }
        }


        public override Dictionary<string, string> GetEntityColumnsWithTypes()
        {
            var d = base.GetEntityColumnsWithTypes();
            //d.Remove("DataBeforeAction");
            //d.Remove("DataAfterAction");
            if (d.ContainsKey("UserId")) d["UserId"] = "Int32";
            return d;
        }

        public override IQueryable<object> SelectViewColumns(IQueryable query)
        {
            query = query.Cast<AuditLog>().Select(a => new
            {
                Id = a.Id,
                UserId = a.UserId.Value,
                ActionType = a.ActionType,
                Entity = a.SourceEntity,
                AffectedRecordKey = a.AffectedRecordKey,
                Source = a.Source,
                Timestamp = a.Timestamp,
            });

            return query.Cast<object>();
        }

    }
}