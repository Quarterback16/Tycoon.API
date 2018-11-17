using System;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.ViewModels.Dynamic
{
    /// <summary>
    /// Defines the abstract View Model for a value for use within a <see cref="DynamicViewModel" />.
    /// </summary>
    [Serializable]
    public abstract class ObjectViewModel<T> : IObjectViewModel<T>
    {
        /// <summary>
        /// The property name.
        /// </summary>
        [Bindable]
        public string Name { get; set; }

        /// <summary>
        /// A value.
        /// </summary>
        [Bindable]
        public abstract T Value { get; set; }
    }
}