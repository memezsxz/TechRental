using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for managing AuditLog entities.
    /// </summary>
    internal class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
    {
        #region Constructor

        public AuditLogRepository(RentalDBContext context, int userId)
            : base(context, userId)
        {
        }

        #endregion

        #region Context Accessor

        /// <summary>
        /// Gets the current database context cast to <see cref="RentalDBContext"/>.
        /// </summary>
        public RentalDBContext RentalDBContext => context as RentalDBContext;

        #endregion

        #region View Projection

        /// <inheritdoc/>
        public override IQueryable<object> SelectViewColumns(IQueryable query)
        {
            query = query.Cast<AuditLog>().Select(a => new
            {
                Id = a.Id,
                UserId = a.UserId,
                ActionType = a.ActionType,
                Entity = a.SourceEntity,
                AffectedRecordKey = a.AffectedRecordKey,
                Source = a.Source,
                Timestamp = a.Timestamp,
            });

            return query.Cast<object>();
        }

        #endregion

        #region Metadata Overrides

        /// <inheritdoc/>
        public override Dictionary<string, string> GetEntityColumnsWithTypes()
        {
            var d = base.GetEntityColumnsWithTypes();

            // Explicitly set the type for UserId
            if (d.ContainsKey("UserId"))
                d["UserId"] = "Int32";

            return d;
        }

        #endregion 
    }
}
