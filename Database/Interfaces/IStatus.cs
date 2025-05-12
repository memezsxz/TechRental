namespace Database.Interfaces
{
    /// <summary>
    /// Defines a contract for entities or repositories that expose a list of predefined statuses.
    /// Used to populate dropdowns or filters for status selection (e.g., rental status, condition).
    /// </summary>
    public interface IStatus
    {
        /// <summary>
        /// Retrieves a dictionary of status values, where the key is the status ID
        /// and the value is the human-readable name.
        /// </summary>
        /// <returns>
        /// A dictionary mapping status IDs to their display names.
        /// </returns>
        Dictionary<int, string> GetAllByName();
    }
}