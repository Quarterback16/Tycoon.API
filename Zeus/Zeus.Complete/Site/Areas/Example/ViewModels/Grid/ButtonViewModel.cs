using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Area.Example.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Grid
{
    /// <summary>
    /// Defines the View Model for <see cref="GridController.Button()" /> and <see cref="GridController.Button(ButtonViewModel)" />.
    /// </summary>
    [DisplayName("Inline button selection grid")]
    public class ButtonViewModel
    {
        /// <summary>
        /// An enumerable of <see cref="ClaimWithButtonsViewModel" />.
        /// </summary>
        [SelectionType(SelectionType.None)]
        [DataType(CustomDataType.Grid)]
        [Bindable]
        public IEnumerable<ClaimWithButtonsViewModel> Claims { get; set; }
    }
}