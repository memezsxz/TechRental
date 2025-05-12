using Database.Core.Domain;

namespace Database.Core.Repositories
{
    /// <summary>
    /// Repository interface for managing <see cref="Document"/> entities.
    /// Inherits basic CRUD operations from <see cref="IRepository{T}"/>
    /// </summary>

    public interface IDocumentRepository : IRepository<Document>
    {
    }
}