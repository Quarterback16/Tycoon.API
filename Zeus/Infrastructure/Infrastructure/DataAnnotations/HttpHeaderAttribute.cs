using System;
using System.Web.Mvc;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to add HTTP Headers to a Controller Action response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    internal class HttpHeaderAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets the name of the HTTP Header.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the HTTP Header.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHeaderAttribute" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public HttpHeaderAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Add HTTP header to filter context.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.AppendHeader(Name, Value);

            base.OnResultExecuted(filterContext);
        }
    }
}
