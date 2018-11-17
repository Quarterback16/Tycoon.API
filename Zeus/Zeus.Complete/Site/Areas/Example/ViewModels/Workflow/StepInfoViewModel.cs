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
    public class StepInfoViewModel : InheritanceViewModel, IWorkflow
    {
        public IndicatorType? Indicator { get; set; }

        [Display(GroupName="General Information")]
        public ContentViewModel Info { get { return new ContentViewModel().AddText("Information."); } }
    }
}