using System;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.ViewModels.Dynamic
{
    /// <summary>
    /// Defines the View Model for a <see cref="DateTime" /> value (date only) for use within a <see cref="DynamicViewModel" />.
    /// </summary>
    [Serializable]
    public class DateViewModel : ObjectViewModel<DateTime>
    {
        /// <summary>
        /// A <see cref="DateTime" /> value (date only).
        /// </summary>
        [Bindable]
        [DataType(DataType.Date)]
        public override DateTime Value { get; set; }
    }
}