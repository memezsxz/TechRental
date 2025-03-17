namespace FormsApp.views.controls
{
    internal interface ISearch
    {
        /// <summary>
        /// Event to notify the form about the search result
        /// </summary>
            event Action<object> OnSearchCompleted;
    }
}