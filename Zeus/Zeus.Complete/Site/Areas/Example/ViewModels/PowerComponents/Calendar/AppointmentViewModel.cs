using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Interfaces.Calendar;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Employment.Web.Mvc.Area.Example.ViewModels.Calendar
{ 

    /// <summary>
    /// Appointment View Model.
    /// </summary>
    [ViewModel]
    [Button("Submit")]
    //[Serializable]
    public class AppointmentViewModel : ILayoutOverride, ICategoryItemViewModel
    {

        public AppointmentViewModel()
        {
            this.Hidden = new LayoutType[] { LayoutType.TitleAndBreadcrumbs  };
        }


        [Bindable]
        public ContentViewModel Heading
        {
            get;
            set;
        }



        /// <summary>
        /// Details.
        /// </summary>
        [Bindable]
        [Required]
        public string Title { get; set; }


        [Hidden]
        [Bindable]
        public string Id { get; set; }



        /// <summary>
        /// Description of the Appointment.
        /// </summary>
        [Bindable]
        
        public string Description { get; set; }


        /// <summary>
        /// Indicates whether the event is allDay event.
        /// </summary>
        [Hidden]
        public bool AllDay { get; set; }


        public IEnumerable<Infrastructure.Types.LayoutType> Hidden
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        [Bindable]
        public DateTime? Start
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        
        [Bindable]
        public DateTime? End
        {
            get;
            set;
        }


        [Hidden] 
        public bool ModelStateIsValid { get; set; }
    }
}