using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Area.Example.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Grid
{
    /// <summary>
    /// Defines the View Model properties for the <see cref="GridController.Display()" /> action.
    /// </summary>
    [DisplayName("Display only grid")]
    [Group("GridGroup")]
    public class DisplayViewModel
    {
        /// <summary>
        /// An enumerable of <see cref="ClaimViewModel" />.
        /// </summary>
        [Display(GroupName ="GridGroup")]
        [DataType(CustomDataType.Grid)]
        [Bindable]
        public IEnumerable<ClaimViewModel> Claims { get; set; }
    }
}