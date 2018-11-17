using System.Collections;
using System.Collections.Generic;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines the methods and properties that are required for a Pageable collection.
    /// </summary>
    public interface IPageable : IList
    {
        /// <summary>
        /// Metadata used for retrieving the next page.
        /// </summary>
        PageMetadata Metadata { get; set; }
    }

    /// <summary>
    /// Defines the methods and properties that are required for a Pageable collection.
    /// </summary>
    /// <typeparam name="T">The type of item in the pageable collection.</typeparam>
    public interface IPageable<T> : IPageable, IList<T>
    {
        /// <summary>
        /// Gets the items from the collection within the bounds of the current page, which is defined by <see cref="PageMetadata"/>.
        /// </summary>
        /// <returns>The collection of items in the current page.</returns>
        IEnumerable<T> GetCurrentPage();
    }
}
