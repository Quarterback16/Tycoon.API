using System;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.ViewModels.Dynamic
{
    /// <summary>
    /// Defines the View Model for a <see cref="string" /> value (multiline) for use within a <see cref="DynamicViewModel" />.
    /// </summary>
    [Serializable]
    public class MultilineStringViewModel : ObjectViewModel<string>
    {
        /// <summary>
        /// A <see cref="string" /> value (multiline).
        /// </summary>
        [Bindable]
        [DataType(DataType.MultilineText)]
        public override string Value { get; set; }
    }
}