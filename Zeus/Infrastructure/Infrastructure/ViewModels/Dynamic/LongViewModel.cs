using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.ViewModels.Dynamic
{
    /// <summary>
    /// Defines the View Model for a <see cref="long" /> value for use within a <see cref="DynamicViewModel" />.
    /// </summary>
    [Serializable]
    public class LongViewModel : ObjectViewModel<long>
    {
        /// <summary>
        /// A <see cref="long" /> value.
        /// </summary>
        [Bindable]
        public override long Value { get; set; }
    }
}