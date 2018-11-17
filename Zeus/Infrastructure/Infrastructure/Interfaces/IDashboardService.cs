using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Web.Mvc.Infrastructure.Interfaces
{
    /// <summary>
    /// The services that can be used to store data about a user's dashboard preferences.
    /// </summary>
    public interface IDashboardService
    {
        /// <summary>
        /// Returns a list of names of widgets that are 'open'. (i.e. Set with 'AddWidgetname' or 'SetWidgetLayout')
        /// </summary>
        /// <param name="widgetContext"></param>
        /// <returns></returns>
        IEnumerable<string> GetOpenWidgetNames(string widgetContext);

        /// <summary>
        /// Adds the given widget name to the given context. The specified widget is now 'open'.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="widgetContext"></param>
        void AddWidgetName(string name, string widgetContext);

        /// <summary>
        /// Removes the given widget name from the given context. The specified widget is now 'closed'.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="widgetContext"></param>
        void RemoveWidgetName(string name, string widgetContext);

        /// <summary>
        /// Sets the entire layout of widgets for the given context. Only the widgets in the given string will be open, all others will be closed.
        /// </summary>
        /// <param name="layout">A comma separated list of widget names</param>
        /// <param name="widgetContext"></param>
        void SetWidgetLayout(string layout, string widgetContext);

        /// <summary>
        /// Provides the currently set data context for displaying widget information.
        /// For example this might be "User" or "Office" depending on the scope of the information to be displayed in widgets
        /// </summary>
        string GetDataContext(string widgetContext);

        /// <summary>
        /// Sets the current data context in use within the given widget context
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="widgetContext"></param>
        void SetDataContext(string dataContext, string widgetContext);
    }
}
