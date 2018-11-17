using System;

namespace Employment.Web.Mvc.Infrastructure.Types
{
    /// <summary>
    /// Paged metadata used for retrieving the next page of history.
    /// </summary>
    [Serializable]
    public class HistoryPageMetadata : PageMetadata
    {
        /// <summary>
        /// Gets or sets the history type.
        /// </summary>
        /// <value>The type of the history.</value>
        public HistoryType HistoryType { get; set; }
    }
}
