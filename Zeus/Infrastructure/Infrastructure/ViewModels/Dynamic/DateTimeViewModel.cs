using System;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.ViewModels.Dynamic
{
    /// <summary>
    /// Defines the View Model for a <see cref="DateTime" /> value for use within a <see cref="DynamicViewModel" />.
    /// </summary>
    [Serializable]
    public class DateTimeViewModel : ObjectViewModel<DateTime>
    {
        /// <summary>
        /// A <see cref="DateTime" /> value.
        /// </summary>
        [Bindable]
        [DataType(DataType.DateTime)]
        public override DateTime Value { get; set; }
    }
}