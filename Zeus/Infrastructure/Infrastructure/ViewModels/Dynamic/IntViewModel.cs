using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.ViewModels.Dynamic
{
    /// <summary>
    /// Defines the View Model for a <see cref="int" /> value for use within a <see cref="DynamicViewModel" />.
    /// </summary>
    [Serializable]
    public class IntViewModel : ObjectViewModel<int>
    {
        /// <summary>
        /// A <see cref="int" /> value.
        /// </summary>
        [Bindable]
        public override int Value { get; set; }
    }
}