using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.ViewModels.Dynamic
{
    /// <summary>
    /// Defines the View Model for a <see cref="string" /> value for use within a <see cref="DynamicViewModel" />.
    /// </summary>
    [Serializable]
    public class StringViewModel : ObjectViewModel<string>
    {
        /// <summary>
        /// A <see cref="string" /> value.
        /// </summary>
        [Bindable]
        public override string Value { get; set; }
    }
}