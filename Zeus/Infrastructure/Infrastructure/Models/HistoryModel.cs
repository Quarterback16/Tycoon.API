using System;
using System.Collections.Generic;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.Models
{
    /// <summary>
    /// Recent history model.
    /// </summary>
    [Serializable]
    public class HistoryModel
    {
        /// <summary>
        /// Gets or sets the type of the history.
        /// </summary>
        /// <value>The type of the history.</value>
        [Alias("HistoryType_RHC")]
        public HistoryType HistoryType { get; set; }

        /// <summary>
        /// Gets or sets the values necessary for loading the object.
        /// </summary>
        /// <value>The values necessary for loading the object as a dictionary of key value pairs.</value>
        [Alias("ObjectValues")]
        public IDictionary<string, object> Values { get; set; }

        /// <summary>
        /// Gets or sets the display name of the object.
        /// </summary>
        /// <value>The display name of the object.</value>
        [Alias("DisplayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the date the object was accessed.
        /// </summary>
        /// <value>the date the object was accessed.</value>
        [Alias("AccessedOn")]
        public DateTime DateAccessed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this recent history item is pinned.
        /// </summary>
        /// <value><c>true</c> if this instance is pinned; otherwise, <c>false</c>.</value>
        [Alias("IsPinned")]
        public bool IsPinned { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        [Alias("UserID")]
        public string Username { get; set; }
    }
}