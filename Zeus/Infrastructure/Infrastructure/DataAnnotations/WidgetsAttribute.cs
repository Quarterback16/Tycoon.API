using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate the widget context applied to a particular View Model. i.e. Which widgets to attach to it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class WidgetsAttribute : Attribute
    {
        /// <summary>
        /// A unique identifier for the set of selected/available widgets. Each "different" dashboard should have a different context
        /// </summary>
        public string WidgetContext { get; set; }

        /// <summary>
        /// Creates a new instance of a SwitcherAttribute and sets the checked and unchecked properties to default values of "Yes" and "No"
        /// </summary>
        public WidgetsAttribute(string widgetContext)
        {
            WidgetContext = widgetContext;
        }
    }
}
