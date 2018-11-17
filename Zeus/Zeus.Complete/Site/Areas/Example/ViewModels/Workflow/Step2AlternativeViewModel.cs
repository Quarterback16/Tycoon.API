using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Workflow
{
    [Serializable]
    public class Step2AlternativeViewModel : StepViewModel, IWorkflow
    {
        public IndicatorType? Indicator { get; set; }

        [Bindable]
        [Display(Name = "Postal address", Order = 1, GroupName = "Postal contact details")]
        [Required]
        public string PostalAddress { get; set; }

        [Bindable]
        [Display(Name = "State", Order = 2, GroupName = "Postal contact details")]
        [Required(ErrorMessage = "A state is required")]
        [AdwSelection(SelectionType.Single, AdwType.ListCode, "STT")]
        [Row("Location", RowType.Third)]
        public string State { get; set; }

        [Bindable]
        [Display(Name = "Postcode", Order = 3, GroupName = "Postal contact details")]
        [Required(ErrorMessage = "A postcode is required")]
        [AdwSelection(SelectionType.Single, AdwType.ListCode, "PCC")]
        [Row("Location", RowType.Third)]
        public string Postcode { get; set; }

        [Bindable]
        [Display(Name = "Suburb", Order = 4, GroupName = "Postal contact details")]
        [Required(ErrorMessage = "A suburb is required")]
        [Row("Location", RowType.Third)]
        public string Suburb { get; set; }
    }
}