using Employment.Web.Mvc.Area.Example.Models;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Employment.Web.Mvc.Area.Example.ViewModels.ServerSideElements
{

    [Group("Info", Order = 1)]
    [Group("Results")]
    public class JobSeekerSearchExampleViewModel
    {

        public JobSeekerSearchExampleViewModel()
        {

            JobseekerSearch = new Infrastructure.ViewModels.JobSeeker.JobseekerSearchViewModel() { };

            Info = new ContentViewModel().AddStrongText("To add Jobseeker Single line search, add the following in your view-model.").AddPreformatted(@"
            
        [Bindable]
        [Display()]
        public Employment.Web.Mvc.Infrastructure.ViewModels.JobSeeker.JobseekerSearchViewModel JobseekerSearch { get; set; }
            ");
        }


        [Bindable]
        [Display(GroupName = "Info")]
        public ContentViewModel Info { get; set; }


        [Bindable]
        [Display()]
        public Employment.Web.Mvc.Infrastructure.ViewModels.JobSeeker.JobseekerSearchViewModel JobseekerSearch { get; set; }


        [Bindable]
        [DataType(CustomDataType.Grid)]
        [Display(GroupName = "Results")]
        public IList<JobSeekerModelList> JobSeekerResults { get; set; }
    }
}