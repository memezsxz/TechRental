using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for managing SystemErrorLog entities.
    /// </summary>
    internal class SystemErrorLogRepository : Repository<SystemErrorLog>, ISystemErrorLogRepository
    {
        #region Constructor

        public SystemErrorLogRepository(RentalDBContext context, int userId)
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

        #region Metadata Overrides

        /// <inheritdoc/>
        public override Dictionary<string, string> GetEntityColumnsWithTypes()
        {
            var d = base.GetEntityColumnsWithTypes();

            if (d.ContainsKey("UserId"))
                d["UserId"] = "Int32";

            return d;
        }

        /// <inheritdoc/>
        public override IQueryable<object> SelectViewColumns(IQueryable query)
        {
            return query.Cast<SystemErrorLog>().Select(e => new
            {
                Id = e.Id,
                UserId = e.UserId,
                ErrorSource = e.ErrorSource,
                Timestamp = e.Timestamp,
            }).Cast<object>();
        }

        #endregion
    }
}