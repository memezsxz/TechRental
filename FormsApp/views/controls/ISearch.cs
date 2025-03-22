namespace FormsApp.views.controls
{
    internal interface ISearch
    {
        /// <summary>
        /// Event to notify the form about the search result
        /// </summary>
            event Action<object> OnSearchCompleted;
        int PageSize { get; set; }
        int PageNumber { get; set; }
        void Apply();
    }
}