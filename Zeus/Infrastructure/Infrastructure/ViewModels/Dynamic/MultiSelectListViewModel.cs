using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.ViewModels.Dynamic
{
    /// <summary>
    /// Defines the View Model for a <see cref="MultiSelectList" /> value for use within a <see cref="DynamicViewModel" />.
    /// </summary>
    [Serializable]
    public class MultiSelectListViewModel : ObjectViewModel<IEnumerable<SelectListItem>>
    {
        /// <summary>
        /// A <see cref="MultiSelectList" /> value.
        /// </summary>
        [Bindable]
        [SelectionType(SelectionType.Multiple)]
        public override IEnumerable<SelectListItem> Value { get; set; }
    }
}