using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using System.ComponentModel.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.ViewModels
{
    /// <summary>
    /// A view model for use in constructing a widget selector in right hand sidebars
    /// </summary>
    public class WidgetSelectorViewModel
    {
        public WidgetSelectorViewModel()
        {
            WidgetDataContexts = new List<string>();
            AvailableWidgetNames = new List<string>();
        }

        /// <summary>
        /// The list of available data contexts for widgets to be rendered in. These might include the current user, the current site, etc.
        /// </summary>
        public List<string> WidgetDataContexts { get; set; }

        /// <summary>
        /// The current data context for widgets to be rendered in. This might be the current user, the current site, etc.
        /// </summary>
        public string CurrentWidgetDataContext { get; set; }

        /// <summary>
        /// The current context for choosing widgets. This might be the 'type' of dashboard, for instance
        /// </summary>
        public string WidgetContext { get; set; }

        /// <summary>
        /// The current set of available widgets that users can choose to add
        /// </summary>
        public List<string> AvailableWidgetNames { get; set; }
    }
}
