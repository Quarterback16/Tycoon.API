using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Report
{
    [Group("Demonstrates new Autocomplete control.")]
    public class SmartAutocompleteViewModel
    {
        [Bindable]
        [Display(Description = "Organisation", GroupName = "Demonstrates new Autocomplete control.", Order = 1)]
        [SmartAutocomplete]
        public string OrganisationNewAutocomplete { get; set; }


        [Bindable]
        [Display(Description = "Age Selection (ADW)", GroupName = "Demonstrates new Autocomplete control.", Order = 2)]
        [AdwSelection(SelectionType.Single, AdwType.ListCode, "AGE")]
        public string AgeAdw { get; set; }


        [Bindable]
        [Display(Description = "Age Selection Related Code", GroupName = "Demonstrates new Autocomplete control.", Order = 3)]
        [AdwSelection(SelectionType.Single, AdwType.RelatedCode, "AGRA", DependentValue = "Y")]
        public string AgeRelated { get; set; }


        [Bindable]
        [Display(Description = "Organisation (AJAX)", GroupName = "Demonstrates new Autocomplete control.", Order = 4)]
        [AjaxSelection("GetData", Area = "Example", Controller = "Report")]
        public string OrganisationAjax { get; set; }
         
    }
}