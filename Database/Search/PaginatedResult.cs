using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Search
{
    /// <summary>
    /// Represents the result of a paginated query, including data and pagination metadata.
    /// </summary>
    public class PaginatedResult
    {
        /// <summary>
        /// The collection of data items returned for the current page.
        /// </summary>
        public IEnumerable Data { get; set; }

        /// <summary>
        /// The total number of records across all pages.
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// The total number of pages based on the page size and total records.
        /// </summary>
        public int TotalPages { get; set; }
    }
}