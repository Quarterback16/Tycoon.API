using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels.Geospatial;
using Employment.Web.Mvc.Infrastructure.ViewModels;

namespace Employment.Web.Mvc.Area.Example.ViewModels.PowerComponents
{
    [Group("Overview", RowType=GroupRowType.Half, RowName = "First")]
    [Group("Example", RowType = GroupRowType.Half, RowName = "First")]
    [Group("Example 2", RowType = GroupRowType.Full, RowName = "Second")]
    [Group("Code", RowType = GroupRowType.Full, RowName = "Third")]
    public class DateBasedContentViewModel
    {
        [Display(GroupName = "Overview")]
        public ContentViewModel Overview
        {
            get
            {
                return new ContentViewModel()
                    .AddParagraph("The 'DateBasedContent' attribute can be used to to create content sections that alter their content based on a selected date. The control uses a compact date selection view that makes it easy to select dates close to the current date. It will be useful for showing current information such as today's and tomorrow's appointments. ")
                    .AddParagraph("It requires the definition of an Ajax handler to produce content to be displayed in the content section. This definition must contain at least an action name, and optionally are and/or controller")
                ;
            }
        }

        [Display(GroupName = "Example")]
        [DateBasedContent(Area = "Example", Controller = "PowerComponents", Action = "HandlerForDateBasedContent")]
        public DateTime? TodaysDate { get; set; }

        [Display(GroupName = "Example 2")]
        [DateBasedContent(Action = "HandlerForDateBasedContent")]
        public DateTime? ExplicitlySetDate
        {
            get
            {
                return DateTime.Parse("2014-07-01");
            }
        }

        [Display(GroupName = "Code")]
        public ContentViewModel Code
        {
            get
            {
                return new ContentViewModel()
                    .AddPreformatted(@"
    // VIEW MODEL
    public class DateBasedContentViewModel
    {
        ...
        [Display(GroupName = ""Example"")]
        [DateBasedContent(Area = ""Example"", Controller = ""PowerComponents"", Action = ""HandlerForDateBasedContent"")]
        public DateTime? TodaysDate { get; set; }

        [Display(GroupName = ""Example 2"")]
        [DateBasedContent(Action = ""HandlerForDateBasedContent"")]
        public DateTime? ExplicitlySetDate
        {
            get
            {
                return DateTime.Parse(""2014-07-01"");
            }
        }
        ...
    }

    ...

    // CONTROLLER
    public class ServerSideElementsController
    {
        ...
        [AjaxOnly]
        public ActionResult HandlerForDateBasedContent(DateTime selectedDate)
        {
            var model = new ContentViewModel()
                    .AddText(""You have selected the date:"")
                    .AddTitle(selectedDate.ToString(""dd/MM/yyyy""))
            ;

            return PartialView(""EditorTemplates/ContentViewModel"", model);
        }
        ...
    }
")
                ;
            }
        }

    }
} 