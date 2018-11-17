using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Area.Example.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;

namespace Employment.Web.Mvc.Area.Example.ViewModels.PagedGrid
{
    /// <summary>
    /// Defines the View Model for <see cref="PagedGridController.DummiesAll()" /> and <see cref="PagedGridController.DummiesAll(DummiesAllViewModel, string)" />.
    /// </summary>
    [DisplayName("All dummies data paged")]
    [Group("Selection", Order = 1)]
    [Group("Search criteria", Order = 2)]
    [Group("Search results", Order = 3)]
    [Button("Search", "Search criteria")]
    [Button("Submit", "Search results")]
    public class DummiesAllViewModel
    {
        /// <summary>
        /// Information about the selected item.
        /// </summary>
        [Display(GroupName = "Selection")]
        public ContentViewModel Content { get; set; }

        [Bindable]
        [Display(Name = "Starts with", GroupName = "Search criteria", Order = 1)]
        [StringLength(5)]
        public string StartsWith { get; set; }

        /// <summary>
        /// The key of the selected <see cref="Dummies" /> item.
        /// </summary>
        /// <remarks>
        /// Used because <see cref="SelectionTypeAttribute" /> of <see cref="Dummies" /> is set to <see cref="SelectionType.Single" />.
        /// </remarks>
        [Bindable]
        [Selector]
        [Hidden]
        public long SelectedKey { get; set; }

        /// <summary>
        /// An enumerable of <see cref="DummyViewModel" />.
        /// </summary>
        /// <remarks>
        /// Size setting in Paged attribute is optional when working with full set of data (if not set, the Infrastructure default will be used).
        /// Using <see cref="SelectionType.Single"/> so <see cref="SelectedKey" /> is used for storing the selection.
        /// </remarks>
        [Bindable]
        [Display(Name = "Claims", GroupName = "Search results", Order = 1)]
        [SelectionType(SelectionType.Single)]
        [DataType(CustomDataType.Grid)]
        [Paged("DummiesAllNextPage", Size = 40, LoadOnScroll = true)] // Size is optional when working with full set of data (if not set, the Infrastructure default will be used)
        public IPageable<DummyViewModel> Dummies { get; set; }
    }
}