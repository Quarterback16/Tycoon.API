using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Workflow
{
    [Serializable]
    [Button("Submit", SubmitType = "Step1", GroupName = "User details")]
    public class Step1ViewModel : InheritanceViewModel, IWorkflow
    {
        public IndicatorType? Indicator { get; set; }

        [Bindable]
        [Display(GroupName = "User details", Order = 1, Name = "Name")]
        [Required]
        public string Name { get; set; }
        
        /// <summary>
        /// Occupation Level 1.
        /// </summary>
        [Display(GroupName = "User details", Order = 2, Name = "Broad occupation category")]
        [AdwSelection(SelectionType.Single, AdwType.ListCode, "AZ1")]
        [Bindable]
        [Row("one", RowType.Half)]
        public string OccupationLevel1 { get; set; }

        /// <summary>
        /// Occupation Level 2.
        /// </summary>
        [Display(GroupName = "User details", Order = 3, Name = "Specific occupation category")]
        [AdwSelection(SelectionType.Single, AdwType.RelatedCode, "AZ12", DependentProperty = "OccupationLevel1", Dominant = true)]
        [Bindable]
        [Row("one", RowType.Half)]
        public string OccupationLevel2 { get; set; }

        /// <summary>
        /// Occupation Level 3.
        /// </summary>
        [Display(GroupName = "User details", Order = 4, Name = "Your occupation")]
        [AdwSelection(SelectionType.Single, AdwType.RelatedCode, "AZ24", DependentProperty = "OccupationLevel2", Dominant = true)]
        [Bindable]
        [Required]
        [Row("two")]
        public string OccupationLevel3 { get; set; }

        [Hidden]
        [Bindable]
        public bool ShowPreferredContactMethod { get; set; }

        [VisibleIfTrue("ShowPreferredContactMethod")]
        [Bindable]
        [Required]
        [Display(GroupName = "User details", Order = 5, Name = "Preferred contact method")]
        [Selection(SelectionType.Single, new[] { "Email", "Post" }, new[] { "By email", "By posted letter" })]
        [System.ComponentModel.DefaultValue("Email")]
        public string PreferredContactMethod { get; set; }
    }
}