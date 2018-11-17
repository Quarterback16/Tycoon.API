using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.ViewModels.Dynamic
{
    /// <summary>
    /// Defines the View Model for a <see cref="bool" /> value for use within a <see cref="DynamicViewModel" />.
    /// </summary>
    [Serializable]
    public class BoolViewModel : ObjectViewModel<bool>
    {
        /// <summary>
        /// A <see cref="bool" /> value.
        /// </summary>
        [Bindable]
        public override bool Value { get; set; }
    }
}