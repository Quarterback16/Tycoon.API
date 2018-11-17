using System;
using Employment.Web.Mvc.Infrastructure.Types;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate the history type to use on the current controller or action.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class HistoryAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The history type.
        /// </summary>
        public HistoryType HistoryType { get; set; }

        /// <summary>
        /// The key for accessing the history type in ViewData.
        /// </summary>
        public static readonly string ViewDataKey = "History.HistoryType";

        /// <summary>
        /// Multiple attribute differentiator
        /// </summary>
        public override object TypeId
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryAttribute" /> class.
        /// </summary>
        /// <param name="historyType">The history type.</param>
        public HistoryAttribute(HistoryType historyType)
        {
            HistoryType = historyType;
        }

        /// <summary>
        /// Called after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewDataDictionary data = filterContext.Controller.ViewData;
            if (data.ContainsKey(ViewDataKey))
            {
                (data[ViewDataKey] as List<HistoryType>).Add(HistoryType);
            }
            else
            {
                data[ViewDataKey] = new List<HistoryType>() { HistoryType };
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
