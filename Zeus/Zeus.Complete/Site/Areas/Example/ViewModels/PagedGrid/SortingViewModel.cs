using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Employment.Web.Mvc.Area.Example.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Area.Example.ViewModels.PagedGrid
{
    /// <summary>
    /// Defines the view model for <see cref="PagedGridController.GridSorting(SortingViewModel,string)"/>.
    /// </summary>
    [DisplayName("Grid sorting.")]
    [Group("Selection", Order = 1)]
    [Group("Search Criteria", Order = 2)]
    [Group("Search Results", Order = 3)]
    [Button("Search", "Search Criteria")]
    [Button("Clear", "Search Criteria", Clear = true, Order = 1)]
    [Button("Submit", "Search Results")]
    public class SortingViewModel
    {

        /// <summary>
        /// Value of Selected Content.
        /// </summary>
        [Display(GroupName = "Selection")]
        public ContentViewModel SelectedContent { get {return new ContentViewModel().AddParagraph("paragraph") ;} set { } }

        /// <summary>
        /// Name property for Search Criteria.
        /// </summary>
        [Infrastructure.DataAnnotations.Bindable]
        [Display(Name = "Name", GroupName = "Search Criteria", Order = 1)]
        public string Name { get; set; }


        /// <summary>
        /// The keys of the selected <see cref="Results"/>.
        /// </summary>
        [Infrastructure.DataAnnotations.Bindable]
        [Hidden]
        [Selector]
        public long SelectedKeys { get; set; }


        /// <summary>
        /// Search Results Grid.
        /// </summary>
        /// <remarks>
        /// Size set in Paged attribute 60.
        /// </remarks>
        [Infrastructure.DataAnnotations.Bindable]
        [Display(Name = "Search Results", GroupName = "Search Results", Order = 2)]
        [SelectionType(SelectionType.Single)]
        [DataType(CustomDataType.Grid)]
        [Paged("NextPageSort", Size = 60)]
        public IPageable<GridSortingViewModel> Results { get; set; }

    }
}