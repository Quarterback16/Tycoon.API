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
    /// Defines the View Model for <see cref="GridController.Multiple()" /> and <see cref="GridController.Multiple(MultipleViewModel)" />.
    /// </summary>
    [DisplayName("Multiple selection grid")]
    [Group("Grids")]
    [Button("Submit", "Grids")]
    [Button("ParentModelButtonEnabled1","Grids", ActionForDependencyType.Enabled, "IsEnabled1", ComparisonType.EqualTo, true, Order = 1)]
    [Button("ParentModelButtonDisabled1","Grids", ActionForDependencyType.Disabled, "IsEnabled1", ComparisonType.EqualTo, true, Order = 2)]
    [Button("ParentModelButtonVisible1","Grids", ActionForDependencyType.Visible, "IsEnabled1", ComparisonType.EqualTo, true, Order = 3)]
    [Button("ParentModelButtonHidden1","Grids", ActionForDependencyType.Hidden, "IsEnabled1", ComparisonType.EqualTo, true, Order = 4)]
    [Link("ParentModelLink1", ActionForDependencyType.Enabled, "IsEnabled1", ComparisonType.EqualTo, true, Order = 5)]
    public class MultipleViewModel
    {
        [Bindable]
        [Display(GroupName = "Grids")]
        public bool IsEnabled1 { get; set; }

        /// <summary>
        /// An enumerable of <see cref="ClaimViewModel" />.
        /// </summary>
        /// <remarks>
        /// Using <see cref="SelectionType.Multiple"/> so <see cref="ClaimViewModel.Selected" /> is used in each <see cref="ClaimViewModel" /> for storing the selections.
        /// </remarks>
        [Display(GroupName = "Grids")]
        [SelectionType(SelectionType.Multiple)]
        [DataType(CustomDataType.Grid)]
        [Bindable]
        [Button("Inline1a","Grids", ActionForDependencyType.Enabled, "IsEnabled1", ComparisonType.EqualTo, true)]
        [Button("Inline1b", "Grids", ActionForDependencyType.Enabled, "IsEnabled1", ComparisonType.EqualTo, true)]
        [Link("Inline1c",  ActionForDependencyType.Enabled, "IsEnabled1", ComparisonType.EqualTo, true)]
        public IEnumerable<ClaimViewModel> Claims { get; set; }

        [Display(GroupName = "Grids")]
        public ContentViewModel Selections { get; set; }
    }
}