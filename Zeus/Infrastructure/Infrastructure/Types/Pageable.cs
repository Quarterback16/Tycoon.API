using System.Collections.Generic;
using System.Linq;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.Types
{
    /// <summary>
    /// Defines a Pageable collection.
    /// </summary>
    /// <typeparam name="T">The type of item in the pageable collection.</typeparam>
    public class Pageable<T> : List<T>, IPageable<T>
    {
        /// <summary>
        /// Metadata used for retrieving the next page.
        /// </summary>
        public PageMetadata Metadata { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pageable{T}" /> class.
        /// </summary>
        public Pageable() : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pageable{T}" /> class.
        /// </summary>
        /// <param name="metadata">Metadata used for retrieving the next page.</param>
        public Pageable(PageMetadata metadata)
        {
            Metadata = metadata;
        }

        /// <summary>
        /// Gets the items from the collection within the bounds of the current page, which is defined by <see cref="PageMetadata"/>.
        /// </summary>
        /// <typeparam>The type of item in the pageable collection.
        ///     <name>T</name>
        /// </typeparam>
        /// <returns>The collection of items in the current page.</returns>
        public IEnumerable<T> GetCurrentPage()
        {
            if (Metadata != null)
            {
                return this.Count() > Metadata.PageSize ? this.AsEnumerable().Skip((Metadata.PageNumber - 1) * Metadata.PageSize).Take(Metadata.PageSize) : this;
            }

            return Enumerable.Empty<T>();
        }
    }
}
