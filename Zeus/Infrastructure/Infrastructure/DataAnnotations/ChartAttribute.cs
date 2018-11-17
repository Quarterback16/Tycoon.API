using System;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate the property is a chart.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ChartAttribute : Attribute
    {
        /// <summary>
        /// The controller action that will return the next page based on the paged metadata.
        /// </summary>
        public string Action { get; protected set; }

        /// <summary>
        /// The controller (defaults to controller in current context).
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// The area (defaults to area in current context).
        /// </summary>
        public string Area { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartAttribute" /> class.
        /// </summary>
        /// <param name="action">The controller action that will return the chart data.</param>
        public ChartAttribute(string action)
        {
            Action = action;
        }
    }
}
