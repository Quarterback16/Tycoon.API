using System;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate the menu details of Controller actions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MenuAttribute : Attribute
    {
        /// <summary>
        /// Name to appear in the menu.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Order in which the button should appear.
        /// </summary>
        /// <remarks>
        /// Defaults to <see cref="int.MaxValue" /> if not set.
        /// </remarks>
        public int Order
        {
            get
            {
                return order.HasValue ? order.Value : int.MaxValue;
            }
            set
            {
                order = value;
            }
        }

        private int? order;

        /// <summary>
        /// The parent Controller of the current action.
        /// </summary>
        public string ParentController { get; set; }

        /// <summary>
        /// The parent action of the current action.
        /// </summary>
        public string ParentAction { get; set; }

        /// <summary>
        /// The parent area of the current action.
        /// </summary>
        public string ParentArea { get; set; }

        /// <summary>
        /// Controller area
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Controller
        /// </summary>
        public string Controller { get; internal set; }

        /// <summary>
        /// Action
        /// </summary>
        public string Action { get; internal set; }

        /// <summary>
        /// Controller security
        /// </summary>
        public SecurityAttribute ControllerSecurity { get; internal set; }

        /// <summary>
        /// Action Security
        /// </summary>
        public SecurityAttribute ActionSecurity { get; internal set; }

        /// <summary>
        /// Parameters
        /// </summary>
        public string[] Parameters { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.MenuAttribute" /> class.
        /// </summary>
        /// <param name="name">Name to appear in the menu.</param>
        public MenuAttribute(string name)
        {
            Name = name;
        }
    }
}