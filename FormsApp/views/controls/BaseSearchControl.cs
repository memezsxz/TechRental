using Database.Search;
using System.Reflection;


namespace FormsApp.views.controls;
/// <summary>
/// An abstract base class for search-related UserControls that encapsulates 
/// repository lookup, method binding, and search execution with paging support.
/// </summary>
public abstract class BaseSearchControl : UserControl
{
    #region Fields and Backing Stores

    private int _pageSize = 10;
    private int _pageNumber = 1;

    #endregion

    #region Protected Properties

    /// <summary>
    /// The target entity viewType for which the repository is being resolved.
    /// </summary>
    protected Type EntityType { get; set; }

    /// <summary>
    /// The property name to be used for filtering/searching.
    /// </summary>
    protected string PropertyName { get; set; }

    /// <summary>
    /// Cached repository instance used to invoke data access methods.
    /// </summary>
    protected object? Repository { get; private set; }

    /// <summary>
    /// Cached MethodInfo for the search method resolved via reflection.
    /// </summary>
    protected MethodInfo? SearchMethod { get; private set; }

    /// <summary>
    /// The name of the method to call on the repository (e.g. "SearchByColumn").
    /// </summary>
    protected string SearchMethodName { get; set; }

    /// <summary>
    /// The parameter types of the method to resolve (used for overload resolution).
    /// </summary>
    protected Type[] SearchMethodParams { get; set; }

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets or sets the number of items per page for pagination.
    /// Defaults to 10, with a lower bound of 1.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 10 : value;
    }

    /// <summary>
    /// Gets or sets the current page number for pagination.
    /// Defaults to 1, with a lower bound of 1.
    /// </summary>
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }
    #endregion

    #region Events
    /// <summary>
    /// Event triggered when a search operation completes and returns a paginated result.
    /// </summary>
    public event Action<PaginatedResult> OnSearchCompleted;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseSearchControl"/> class.
    /// Resolves the repository and binds the target method.
    /// </summary>
    /// <param name="entityType">The viewType of entity being searched.</param>
    /// <param name="propertyName">The property name to filter on.</param>
    /// <param name="searchMethodName">The method name to invoke on the repository.</param>
    /// <param name="searchMethodParams">Parameter types for overload resolution.</param>
    protected BaseSearchControl(Type entityType, string propertyName, string searchMethodName, Type[] searchMethodParams)
    {
        EntityType = entityType;
        PropertyName = propertyName;
        SearchMethodName = searchMethodName;
        SearchMethodParams = searchMethodParams;

        InitializeRepositoryAndMethod();
    }


    #endregion

    #region Repository Initialization

    /// <summary>
    /// Resolves the repository instance and binds the search method using reflection.
    /// Throws typed exceptions if resolution fails.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when no repository is found for the given viewType.</exception>
    /// <exception cref="MissingMethodException">Thrown when the target method cannot be resolved.</exception>
    private void InitializeRepositoryAndMethod()
    {
        Repository = Helpers.GetRepositoryForType(EntityType);

        if (Repository == null)
        {
            throw new InvalidOperationException($"No repository found for {EntityType.Name}.");
        }

        SearchMethod = SearchMethodParams == null
            ? Repository.GetType().GetMethod(SearchMethodName)
            : Repository.GetType().GetMethod(SearchMethodName, SearchMethodParams);

        if (SearchMethod == null)
        {
            throw new MissingMethodException($"Method '{SearchMethodName}' not found in {Repository.GetType().Name}.");
        }
    }

    #endregion

    #region Search Execution


    /// <summary>
    /// Invokes the cached search method on the repository using the provided parameters.
    /// </summary>
    /// <param name="methodParams">An array of parameters to pass to the search method.</param>
    protected void InvokeSearch(object[]? methodParams = null)
    {
        //if (Repository == null || SearchMethod == null)
        //{
        //    Console.WriteLine("Repository or search method not initialized.");
        //    return;
        //}

        // Invoke the search method with the provided parameters
        var result = SearchMethod.Invoke(Repository, methodParams);

        // Dispatch the result to the main UI logic
        RaiseSearchCompleted(result as PaginatedResult);
    }


    /// <summary>
    /// Raises the OnSearchCompleted event safely.
    /// </summary>
    /// <param name="result">The paginated result returned by the search method.</param>
    protected void RaiseSearchCompleted(PaginatedResult result)
    {
        OnSearchCompleted?.Invoke(result);
    }

    #endregion

    #region Abstract Members

    /// <summary>
    /// Abstract method that must be implemented by child controls to initiate a search operation.
    /// Typically wired to a UI action (e.g. "Apply" button).
    /// </summary>
    public abstract void Apply();
    #endregion
}
