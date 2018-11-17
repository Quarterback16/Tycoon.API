using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Types.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Web.Mvc.Infrastructure.ViewModels.Calendar
{

    /// <summary>
    /// Represents a view model that displays calendar.
    /// </summary>
    public class CalendarViewModel : ILayoutOverride
    {


        public DefaultView DefaultView;

        /// <summary>
        /// Initialises a new instance of <see cref="CalendarViewModel"/>.
        /// </summary>
        public CalendarViewModel()
        {

        }

        /// <summary>
        /// Initialises a new instance of <see cref="CalendarViewModel"/>.
        /// </summary>
        /// <param name="defaultView">Default view for calendar. By default it will be set to 'Day' view.</param>
        public CalendarViewModel(DefaultView defaultView): this()
        {
            this.DefaultView = defaultView;
        }

        public IEnumerable<LayoutType> Hidden
        {
            get
            {
                return new LayoutType[] { LayoutType.RequiredFieldsMessage };
            }
            set
            {
                throw new NotImplementedException();
            }
        }



        /// <summary>
        /// Name of the Calendar
        /// </summary>
        public string CalendarName { get; set; }



        /// <summary>
        /// Categories i.e. appointment types for the calendar.
        /// </summary>
        public IList<CategoryViewModel> Categories { get; set; }
        //public IEnumerable<CategoryItemViewModel> CategoryItems { get; set; }


    }

     
     
}
