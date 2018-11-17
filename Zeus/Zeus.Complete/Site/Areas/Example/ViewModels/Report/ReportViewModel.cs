using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Report
{
    [DisplayName("Reports")]
    [Group("Select Report", Order = 1)]
    public class ReportViewModel
    {
        [Display(GroupName = "Select Report")]
        public ContentViewModel SelectionContent { get; set; }
    }
}