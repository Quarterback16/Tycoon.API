using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using Employment.Web.Mvc.Infrastructure.ViewModels.Calendar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Calendar
{
    /// <summary>
    /// Represents view model of Calendar Index.
    /// </summary>

    [Group("Overview", Order = 1)]
    [Group("Calendar", Order = 2)]
    [ViewModel]
    public class CalendarIndexViewModel
    {
        /*
        [Display(GroupName = "Overview", Order = 1)]
        public ContentViewModel Overview
        {
            get
            {
                return new ContentViewModel().AddText("Appointments");
            }
        }*/

        /// <summary>
        /// Property that displays Calendar.
        /// </summary>
        [Display(GroupName = "Calendar", Order = 2)]
        public CalendarViewModel Calendar { get; set; }
    }
}