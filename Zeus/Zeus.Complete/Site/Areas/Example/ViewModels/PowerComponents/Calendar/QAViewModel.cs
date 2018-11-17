using Employment.Web.Mvc.Area.Example.Types;
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
    /// Q & A view Model representing 'Q&A' category on calendar.
    /// </summary>
    [Button("Submit")]
    [Button("Delete Event", ActionForDependencyType.Visible, "Id", ComparisonType.NotEqualTo, null, SubmitType = BasicSubmitType.DeleteEvent)]
    [Button("Close", Cancel = true)]
    public class QAViewModel : ICategoryItemViewModel, ILayoutOverride
    {

        public QAViewModel()
        {
            this.Hidden = new LayoutType[] { LayoutType.TitleAndBreadcrumbs, LayoutType.LeftHandNavigation  };
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
        [Bindable]
        [System.ComponentModel.Description("Indicates whether this event is all day event.")]
        [Display(Name = "All Day event?")]
        public bool AllDay { get; set; }

         


        public IEnumerable<Infrastructure.Types.LayoutType> Hidden
        {
            get
            {
                return new LayoutType[] { LayoutType.LeftHandNavigation, LayoutType.TitleAndBreadcrumbs };
            }
            set
            {

            }
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
        [RequiredIf("Start", ComparisonType.NotEqualTo, null, ErrorMessage="End Date is required of Start Date is entered.")]
        [GreaterThanOrEqualTo("Start", ErrorMessage = "End Session must occur later than Start Session.")]
        public DateTime? End
        {
            get;
            set;
        }


        [Hidden] 
        public bool ModelStateIsValid { get; set; }

    }
}