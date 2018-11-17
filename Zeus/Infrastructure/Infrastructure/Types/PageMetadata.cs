using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.Types
{
    /// <summary>
    /// Contains paging information used by <see cref="Pageable{T}" />.
    /// </summary>
    /// <remarks>
    /// When implementing, just implement the HasMorePages() method. Do not modify any of the properties managed by Infrastructure.
    /// Implement and include:
    /// - Properties user supplied for retrieving the first page
    /// - If mainframe paging, properties necessary for mainframe to retrieve next page
    /// </remarks>
    [Serialized]
    [Serializable]
    public class PageMetadata
    {
        /// <summary>
        /// Page number.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Page size.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Name of the enumerable property to be paged.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Type of parent model that the enumerable property is contained within.
        /// </summary>
        public Type ModelType { get; set; }

        /// <summary>
        /// Total number of rows (if available).
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Indicates whether there are more pages.
        /// </summary>
        /// <remarks>
        /// Must be overridden for mainframe based paging.
        /// </remarks>
        /// <returns><c>true</c> if there are more pages; otherwise, <c>false</c>.</returns>
        public virtual bool HasMorePages()
        {
            return Total > 0 && ((PageNumber - 1) * PageSize) < Total;
        }
    }
}
