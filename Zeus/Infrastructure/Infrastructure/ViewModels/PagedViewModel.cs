using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.ViewModels
{
    /// <summary>
    /// Contains paging information managed by Infrastructure.
    /// </summary>
    /// <remarks>
    /// When implementing, just implement the HasMorePages() method. Do not modify any of the properties managed by Infrastructure.
    /// Implement and include:
    /// - Properties user supplied for retrieving the first page
    /// - If mainframe pagiong, properties necessary for mainframe to retrieve next page
    /// </remarks>
    [Serialized]
    [Serializable]
    public abstract class PagedViewModel
    {
        // TODO: look at making these properties all Internal and in the GridHelper interact with them (so RHEA Grid templates will use grid helper to update them?)

        /// <summary>
        /// Row number.
        /// </summary>
        public int Row { get; set; }

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
        public virtual bool HasMorePages()
        {
            return Total > 0 && Row < Total;
        }
    }
}
