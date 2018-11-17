using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.ViewModels.Dynamic
{
    /// <summary>
    /// Defines the View Model for a <see cref="double" /> value for use within a <see cref="DynamicViewModel" />.
    /// </summary>
    [Serializable]
    public class DoubleViewModel : ObjectViewModel<double>
    {
        /// <summary>
        /// A <see cref="double" /> value.
        /// </summary>
        [Bindable]
        public override double Value { get; set; }
    }
}