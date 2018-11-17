using System;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used on an enumerable Grid property to indicate the action that will return the next page.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PagedAttribute : Attribute
    {

        /// <summary>
        /// Local property for Page Size
        /// </summary>
        private object _size;

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
        /// Allows scrolling when user scrolls to bottom of page.
        /// </summary>
        /// <remarks>
        /// Ensure that your table is displayed at the very end of the page and all the buttons are situated above the table.
        /// </remarks>
        public bool LoadOnScroll { get; set; }

        /// <summary>
        /// The maximum number of items to display per page. 
        /// </summary>
        /// <remarks>Property must be type of integer or type of string referencing a dependent property.</remarks>
        /// <exception cref="ArgumentNullException">Is thrown if property is not found or value is null.</exception>
        public object Size
        {
            get { return _size; }
            set
            {
                // Invalid if not type of int or string with value.
                if (!(value != null && (value is int || (value is string && !string.IsNullOrEmpty(value as string)))))
                {
                    throw new ArgumentNullException(
                        "Size must be type of integer or type of string referencing a dependent property that is an integer.");
                }
                _size = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedAttribute" /> class.
        /// </summary>
        /// <param name="action">The controller action that will return the next page based on the page metadata.</param>
        public PagedAttribute(string action)
        {
            Action = action;
        }
    }
}
