using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Area.Example.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Grid
{
    /// <summary>
    /// Defines the View Model for <see cref="GridController.Paged()" /> and <see cref="GridController.Paged(PagedSetViewModel)" />.
    /// </summary>
    [DisplayName("Paged grid")]
    [Group("Paged Grid")]
    [Button("Submit", "Paged Grid")]
    public class MoreViewModel
    {
        /// <summary>
        /// The key of the selected <see cref="Claims" />.
        /// </summary>
        /// <remarks>
        /// Used because <see cref="SelectionTypeAttribute" /> of <see cref="Claims" /> is set to <see cref="SelectionType.Single" />.
        /// </remarks>
        [Selector]
        [Hidden]
        [Bindable]
        public string SelectedKey { get; set; }

        /// <summary>
        /// An enumerable of <see cref="ClaimViewModel" />.
        /// </summary>
        /// <remarks>
        /// Using <see cref="SelectionType.Single"/> so <see cref="SelectedKey" /> is used for storing the selection.
        /// </remarks>
        [SelectionType(SelectionType.Single)]
        [DataType(CustomDataType.Grid)]
        [Bindable]
        //[More("NextSequence", "PreviousSequence")]
        public IEnumerable<ClaimViewModel> MyMoreList { get; set; }

        [Bindable]
        [Hidden]
        public int NextSequence { get; set; }

        [Display(GroupName = "Selected claims")]
        public ContentViewModel Selections { get; set; }
    }
}