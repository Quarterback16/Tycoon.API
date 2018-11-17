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
    /// Represents a 'widget', a component that can be attached to dashboards, and intended for simple statistical views and links
    /// </summary>
    public class WidgetViewModel
    {
        #region static section

        /// <summary>
        /// The global collection of all registered widgets. Add your widget to this collection using the RegisterWidget call
        /// </summary>
        static private Dictionary<string, Dictionary<string, WidgetViewModel>> registeredWidgets = new Dictionary<string, Dictionary<string, WidgetViewModel>>();

        /// <summary>
        /// The special context name to use when you want your widget to appear in all contexts
        /// </summary>
        public const string ALL_CONTEXTS = "WidgetViewModel_All_Contexts";

        /// <summary>
        /// Registers a Widget to make it available for use in the application
        /// </summary>
        /// <param name="widget">The details required to render and display the widget</param>
        /// <param name="widgetContexts">The list of widget contexts in which this widget should be available</param>
        static public void RegisterWidget(WidgetViewModel widget, IEnumerable<string> widgetContexts) {
            foreach (string context in widgetContexts)
            {
                if (!registeredWidgets.ContainsKey(context)) registeredWidgets.Add(context, new Dictionary<string, WidgetViewModel>());
                registeredWidgets[context][widget.UniqueName] = widget;
            }
        }

        /// <summary>
        /// Gets all registered widgets for the given context. Note that this will also return widgets that have been added using the special ALL_CONTEXTS name
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        static public IEnumerable<WidgetViewModel> GetWidgetsForContext(string context)
        {
            var result = new List<WidgetViewModel>();
            if (registeredWidgets.ContainsKey(context)) result.AddRange(registeredWidgets[context].Values);
            if (registeredWidgets.ContainsKey(ALL_CONTEXTS)) result.AddRange(registeredWidgets[ALL_CONTEXTS].Values);
            return result;
        }

        /// <summary>
        /// Get the individual widget with the given name from the given context. Returns null if the widget has not been registered in the given context or the special ALL_CONTEXTS context.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        static public WidgetViewModel GetWidget(string name, string context)
        {
            foreach (string ctx in new string[] { context, ALL_CONTEXTS })
            {
                if (registeredWidgets.ContainsKey(ctx) && registeredWidgets[ctx].ContainsKey(name))
                {
                    return registeredWidgets[ctx][name];
                }
            }
            return null;
        }

        #endregion

        #region instance region

        /// <summary>
        /// The Action from which content for this widget will be fetched. This will typically be the location of an [AjaxOnly] action that returns a PartialView
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// The Controller containing this widget's Action
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// The Area containing this widget's Controller 
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// How to lay the widget out on a dashboard. i.e. it's size. Use Full for large widgetsd, Half, for regular widgets, and Third for small widgets.
        /// </summary>
        public GroupRowType PreferredRowType { get; set; }

        /// <summary>
        /// A name that describes the widget, and is globally unique. This name will also be used when displaying the widget
        /// </summary>
        public string UniqueName { get; private set; }

        public WidgetViewModel(string uniqueName, string controller, string action) : this(uniqueName, null, controller, action, GroupRowType.Half) {}
        public WidgetViewModel(string uniqueName, string area, string controller, string action) : this(uniqueName, area, controller, action, GroupRowType.Half) { }

        public WidgetViewModel(string uniqueName, string controller, string action, GroupRowType groupType) : this(uniqueName, null, controller, action, groupType) { }

        public WidgetViewModel(string uniqueName, string area, string controller, string action, GroupRowType groupType)
        {
            UniqueName = uniqueName;
            Area = area;
            Controller = controller;
            Action = action;
            PreferredRowType = groupType;
            if (uniqueName.IndexOf(',') >= 0) throw new ArgumentException("A widget name cannot contain a comma: ','", "uniqueName");
        }

        #endregion 
    }
}
