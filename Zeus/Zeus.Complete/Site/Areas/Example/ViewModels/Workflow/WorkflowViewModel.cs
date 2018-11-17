using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;
using ReadOnlyAttribute = System.ComponentModel.ReadOnlyAttribute;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Workflow
{
    [DisplayName("Workflow")]
    [Group("Summary group", RowName = "1", RowType = GroupRowType.OneThird, Order = 1)]
    [Group("Details group", RowName = "1", RowType = GroupRowType.TwoThird, Order = 2, GroupType = GroupType.Logical)]
    [Link("Step 1", Action = "Step1", Controller = "Workflow", Area = "Example", Parameters = new[] { "ID" }, GroupName = "Summary group", PropertyNameForAjax = "Details", Order = 1)]
    [Link("Step 2", Action = "Step2", Controller = "Workflow", Area = "Example", Parameters = new[] { "ID" }, GroupName = "Summary group", PropertyNameForAjax = "Details", Order = 2)]
    public class WorkflowViewModel 
    {
        [Editable(false)]
        [VisibleIfNotEmpty]
        [Bindable]
        public long? ID { get; set; }

        [Display(Name = "Summary", Order = 1, GroupName = "Summary group")]
        [Bindable]
        public ContentViewModel Summary
        {
            get
            {
                if (summary == null)
                {
                    summary = new ContentViewModel().AddText("Summary.");
                }

                return summary;
            }
        }

        private ContentViewModel summary;

        [Display(Name = "Details", Order = 2, GroupName = "Details group")]
        [Bindable]
        [AjaxLoad("StepInfo", Parameters = new [] { "ID" })]
        public InheritanceViewModel Details { get; set; }
    }
}