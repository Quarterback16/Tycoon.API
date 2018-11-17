using System;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate that certain client-side behaviours should always be skipped.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SkipClientSideAttribute : Attribute
    {
        /// <summary>
        /// Whether to always skip the client-side unsaved changes prompt.
        /// </summary>
        public bool UnsavedChanges { get; set; }

        /// <summary>
        /// Whether to always skip the client-side validation.
        /// </summary>
        public bool Validation { get; set; }
    }
}
