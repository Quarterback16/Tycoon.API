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
    /// Defines the View Model for <see cref="GridController.Combination()" /> and <see cref="GridController.Combination(CombinationViewModel)" />.
    /// </summary>
    [DisplayName("Combination selection grid")]

    [Group("Grid Group")]
    [Group("Grid Group2")]
    [Group("Grid Group3")]
    [Button("Submit", "Grid Group", Primary=true)]
    [Button("Clear button", "Grid Group", Clear = true)]
    public class CombinationViewModel
    {
        [Display(GroupName = "Selected claims", Order = 1)]
        public ContentViewModel Selections { get; set; }

        /// <summary>
        /// The key of the selected <see cref="SingleClaims1" />.
        /// </summary>
        /// <remarks>
        /// Used because <see cref="SelectionTypeAttribute" /> of <see cref="SingleClaims1" /> is set to <see cref="SelectionType.Single" />.
        /// </remarks>
        [Selector(TargetProperty = "SingleClaims1")]
        [Hidden]
        [Bindable]
        public string SelectedSingleClaims1Key { get; set; }

        /// <summary>
        /// An enumerable of <see cref="ClaimViewModel" />.
        /// </summary>
        /// <remarks>
        /// Using <see cref="SelectionType.Multiple"/> so <see cref="ClaimViewModel.Selected" /> is used in each <see cref="ClaimViewModel" /> for storing the selections.
        /// </remarks>
        [Display(Name = "First single selection", Order = 2, GroupName = "Grid Group")]
        [SelectionType(SelectionType.Single)]
        [DataType(CustomDataType.Grid)]
        [Bindable]
        [SkipLink]
        public IEnumerable<ClaimViewModel> SingleClaims1 { get; set; }

        /// <summary>
        /// The key of the selected <see cref="SingleClaims1" />.
        /// </summary>
        /// <remarks>
        /// Used because <see cref="SelectionTypeAttribute" /> of <see cref="SingleClaims2" /> is set to <see cref="SelectionType.Single" />.
        /// </remarks>
        [Selector(TargetProperty = "SingleClaims2")]
        [Hidden]
        [Bindable]
        public string SelectedSingleClaims2Key { get; set; }

        /// <summary>
        /// An enumerable of <see cref="ClaimViewModel" />.
        /// </summary>
        /// <remarks>
        /// Using <see cref="SelectionType.Multiple"/> so <see cref="ClaimViewModel.Selected" /> is used in each <see cref="ClaimViewModel" /> for storing the selections.
        /// </remarks>
        [Display(Name = "Second single selection", Order = 3, GroupName = "Grid Group2")]
        [SelectionType(SelectionType.Single)]
        [DataType(CustomDataType.Grid)]
        [Bindable]
        [SkipLink]
        public IEnumerable<ClaimViewModel> SingleClaims2 { get; set; }

        /// <summary>
        /// An enumerable of <see cref="ClaimViewModel" />.
        /// </summary>
        /// <remarks>
        /// Using <see cref="SelectionType.Multiple"/> so <see cref="ClaimViewModel.Selected" /> is used in each <see cref="ClaimViewModel" /> for storing the selections.
        /// </remarks>
        [Display(Name = "Multiple selection", Order = 4, GroupName = "Grid Group3")]
        [SelectionType(SelectionType.Multiple)]
        [DataType(CustomDataType.Grid)]
        [Bindable]
        [SkipLink]
        public IEnumerable<ClaimViewModel> MultipleClaims { get; set; }

        /// <summary>
        /// An enumerable of <see cref="ClaimViewModel" />.
        /// </summary>
        [Display(Name = "Display only", Order = 5, GroupName = "Grid Group3")]
        [DataType(CustomDataType.Grid)]
        [Bindable]
        [SkipLink]
        public IEnumerable<ClaimViewModel> DisplayClaims { get; set; }
    }
}