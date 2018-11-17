using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.ViewModels.Dynamic
{
    /// <summary>
    /// Defines the View Model for a <see cref="SelectList" /> value for use within a <see cref="DynamicViewModel" />.
    /// </summary>
    [Serializable]
    public class SelectListViewModel : ObjectViewModel<IEnumerable<SelectListItem>>
    {
        /// <summary>
        /// A <see cref="SelectList" /> value.
        /// </summary>
        [Bindable]
        [SelectionType(SelectionType.Single)]
        public override IEnumerable<SelectListItem> Value { get; set; }
    }
}