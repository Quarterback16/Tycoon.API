using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.ViewModels.Dynamic
{
    /// <summary>
    /// Defines the View Model for a <see cref="string" /> value (label) for use within a <see cref="DynamicViewModel" />.
    /// </summary>
    [Serializable]
    public class LabelViewModel : ObjectViewModel<string>
    {
        /// <summary>
        /// Whether the label is hidden.
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// The property the label is for.
        /// </summary>
        public string ForProperty { get; set; }

        /// <summary>
        /// A <see cref="string" /> value (label).
        /// </summary>
        [Bindable]
        public override string Value { get; set; }
    }
}