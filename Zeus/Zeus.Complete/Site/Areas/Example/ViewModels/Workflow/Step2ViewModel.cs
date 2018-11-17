using System;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Workflow
{
    [Serializable]
    [Button("Submit", SubmitType = "Step2", GroupName = "Contact details")]
    public class Step2ViewModel : InheritanceViewModel, IWorkflow
    {
        public IndicatorType? Indicator { get; set; }

        [Bindable]
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address", Order = 2, GroupName = "Contact details")]
        public string EmailAddress { get; set; }



      


    }
}