using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.Exceptions
{
    /// <summary>
    /// The exception that is thrown when an attempt to render using View Models that use the <see cref="SelectorAttribute" /> are incorrectly configured.
    /// </summary>
    [Serializable]
    public class SelectorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectorException" /> class.
        /// </summary>
        public SelectorException(string message) : base(message) { }
    }
}
