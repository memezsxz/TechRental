using Database.Core.Domain;

namespace Database.Core.Repositories
{
    /// <summary>
    /// Repository interface for managing <see cref="Document"/> entities.
    /// Inherits basic CRUD operations from <see cref="IRepository{T}"/>
    /// </summary>

    public interface IDocumentRepository : IRepository<Document>
    {
        /// <summary>
        /// Retrieves all documents associated with a specific rental request.
        /// </summary>
        /// <param name="requestId">The ID of the rental request.</param>
        /// <returns>
        /// A list of <see cref="Document"/> objects linked to the specified rental request.
        /// </returns>
        public List<Document> GetAllRequestDocuments(int requestId);
    }
}