using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate the view will be loaded via Ajax..
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AjaxLoadAttribute : Attribute
    {
        /// <summary>
        /// The controller action (defaults to controller in current context).
        /// </summary>
        public string Action { get; private set; }

        /// <summary>
        /// The controller (defaults to controller in current context).
        /// </summary>
        public string Controller { get; private set; }

        /// <summary>
        /// The area (defaults to area in current context).
        /// </summary>
        public string Area { get; private set; }

        /// <summary>
        /// Property parameters to pass.
        /// </summary>
        public string[] Parameters { get; set; }

        /// <summary>
        /// The name of the route to use when generating the link.
        /// </summary>
        /// <remarks>
        /// Necessary if your link is to an action in a faked area.
        /// </remarks>
        public string RouteName { get; set; }

        public AjaxLoadAttribute(string action) : this(action, null, null) { }

        public AjaxLoadAttribute(string action, string controller) : this(action, controller, null) { }

        public AjaxLoadAttribute(string action, string controller, string area)
        {
            Action = action;
            Controller = controller;
            Area = area;
        }
    }
}
