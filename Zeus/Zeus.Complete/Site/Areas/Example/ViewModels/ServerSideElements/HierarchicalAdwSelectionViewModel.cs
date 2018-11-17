using Employment.Web.Mvc.Area.Example.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;

namespace Employment.Web.Mvc.Area.Example.ViewModels.ServerSideElements
{
    [DisplayName("Hierarchical [AdwSelection] attribute example")]
    [Group("Occupations", Order = 2)]
    [Button("Submit", "Occupations")]
    [Button("Clear Button", "Occupations", Clear = true)]
    public class HierarchicalAdwSelectionViewModel
    {
        [Display(GroupName = "Occupations", Order = 0)]
        public ContentViewModel Introduction
        {
            get
            {
                var content = new ContentViewModel()
                    .AddParagraph(@"The AdwSelection attribute can be used to make selections from a hierarchical ADW code structure. The choices are determined at the SERVER and use ajax call to populate the choices")
                    ;
                return content;
            }
        }

        [Display(GroupName = "Occupations", Order = 1)]
        public ContentViewModel Selection { get; set; }

        [Display(GroupName = "Occupations", Order = 2, Name = "Occupation Level 1")]
        [AdwSelection(SelectionType.Single, AdwType.ListCode, "AZ1")]
        [Bindable]
        public string OccupationLevel1 { get; set; }

        [Display(GroupName = "Occupations", Order = 3, Name = "Occupation Level 2")]
        [AdwSelection(SelectionType.Single, AdwType.RelatedCode, "AZ12", DependentProperty = "OccupationLevel1", Dominant = true)]
        [Bindable]
        public string OccupationLevel2 { get; set; }

        [Display(GroupName = "Occupations", Order = 4, Name = "Occupation Level 3")]
        [AdwSelection(SelectionType.Single, AdwType.RelatedCode, "AZ24", DependentProperty = "OccupationLevel2", Dominant = true)]
        [Bindable]
        public string OccupationLevel3 { get; set; }
    }
}