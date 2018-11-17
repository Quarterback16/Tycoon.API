using System;
using System.ComponentModel;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate the details of a link for display in a view.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class LinkAttribute : ContingentAttribute, ISplitButtonChild
    {
        /// <summary>
        /// The type ID of this attribute.
        /// </summary>
        /// <remarks>
        /// When <see cref="AttributeUsageAttribute.AllowMultiple" /> is true, <see cref="TypeId" /> must be overriden to return the current instance so <see cref="TypeDescriptor" /> can properly return multiple attributes of the same type.
        /// </remarks>
        public override object TypeId { get { return this; } }

        /// <summary>
        /// The target property (which should a ViewModel) that will be populated via Ajax with this link Action when the link is selected.
        /// </summary>
        public string PropertyNameForAjax { get; set; }

        /// <summary>
        /// Name of link.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The name of the group the link belongs to, if any.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// If set, this indicates that a button is a sub button within a split button.
        /// This property indicates the name of the top level button that this button lies under.
        /// </summary>
        public string SplitButtonParent { get; set; }

        /// <summary>
        /// Order in which the link should appear.
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
        /// The controller action (defaults to controller in current context).
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// The controller (defaults to controller in current context).
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// The area (defaults to area in current context).
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Property parameters to pass.
        /// </summary>
        public string[] Parameters { get; set; }

        /// <summary>
        /// Whether the link is a cancel action (visual equivalent of a cancel button).
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Whether to skip client side validation when this link is selected.
        /// </summary>
        public bool SkipClientSideValidation { get; set; }

        /// <summary>
        /// If specified, the user must belong to at least one of the Roles to see the link.
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// The name of the route to use when generating the link.
        /// </summary>
        /// <remarks>
        /// Necessary if your link is to an action in a faked area.
        /// </remarks>
        public string RouteName { get; set; }

        /// <summary>
        /// Whether to open in new tab.
        /// </summary>
        /// <remarks>
        /// Necessary for displaying reports.
        /// </remarks>
        public bool OpensInNewTab { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.LinkAttribute" /> class.
        /// </summary>
        public LinkAttribute() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.LinkAttribute" /> class.
        /// </summary>
        /// <param name="name">Name of link.</param>
        /// <param name="groupName">Name of group.</param>
        public LinkAttribute(string name, string groupName = null)
        {
            Name = name;

            GroupName = groupName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.LinkAttribute" /> class.
        /// </summary>
        /// <param name="dependencyType">The type of action to take if the dependency condition is met.</param>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="comparisonType">The comparison type to use.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        public LinkAttribute(ActionForDependencyType dependencyType, string dependentProperty, ComparisonType comparisonType, object dependentValue) : base(dependencyType, dependentProperty, comparisonType, dependentValue) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.LinkAttribute" /> class.
        /// </summary>
        /// <param name="name">Name of link.</param>
        /// <param name="dependencyType">The type of action to take if the dependency condition is met.</param>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="comparisonType">The comparison type to use.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        public LinkAttribute(string name, ActionForDependencyType dependencyType, string dependentProperty, ComparisonType comparisonType, object dependentValue) : base(dependencyType, dependentProperty, comparisonType, dependentValue)
        {
            Name = name;
        }

        /// <summary>
        /// Implementation for the ISplitButtonChild interface
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public MvcHtmlString Render(HtmlHelper html)
        {
            return html.RenderLink(this, html.ViewData.ModelMetadata.Properties, html.ViewData.Model, true);
        }
    }
}
