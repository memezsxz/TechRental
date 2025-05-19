using Database.Core.Domain;
using Database.Core.Repositories;

namespace Database.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for managing Document entities.
    /// </summary>
    internal class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        #region Constructor

        public DocumentRepository(RentalDBContext context, int userId)
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

        #region IEquipmentRepository Implementation

        /// <inheritdoc/>
        public List<Document> GetAllRequestDocuments(int requestId)
        {
            return context.Documents.Where(d => d.RentalId == requestId).ToList();
        }
        #endregion
    }
}