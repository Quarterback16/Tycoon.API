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
    public class ExternalLinkAttribute : ContingentAttribute, ISplitButtonChild
    {
        /// <summary>
        /// The type ID of this attribute.
        /// </summary>
        /// <remarks>
        /// When <see cref="AttributeUsageAttribute.AllowMultiple" /> is true, <see cref="TypeId" /> must be overriden to return the current instance so <see cref="TypeDescriptor" /> can properly return multiple attributes of the same type.
        /// </remarks>
        public override object TypeId { get { return this; } }

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
        /// The URL of the external link.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Property parameters to pass.
        /// </summary>
        public string[] Parameters { get; set; }
        
        /// <summary>
        /// If specified, the user must belong to at least one of the Roles to see the link.
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.ExternalLinkAttribute" /> class.
        /// </summary>
        /// <param name="url">External URL.</param>
        /// <exception cref="ArgumentNullException">Thrown if <see>
        ///                                                       <cref>url</cref>
        ///                                                   </see>
        ///     is null or empty.</exception>
        public ExternalLinkAttribute(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            Url = url;
            Name = url;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.ExternalLinkAttribute" /> class.
        /// </summary>
        /// <param name="url">External URL.</param>
        /// <param name="name">Name of link.</param>
        /// <exception cref="ArgumentNullException">Thrown if <see>
        ///                                                       <cref>url</cref>
        ///                                                   </see>
        ///     is null or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <see>
        ///                                                       <cref>name</cref>
        ///                                                   </see>
        ///     is null or empty.</exception>
        public ExternalLinkAttribute(string url, string name)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            Url = url;
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.ExternalLinkAttribute" /> class.
        /// </summary>
        /// <param name="url">External URL.</param>
        /// <param name="dependencyType">The type of action to take if the dependency condition is met.</param>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="comparisonType">The comparison type to use.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        /// <exception cref="ArgumentNullException">Thrown if <see>
        ///                                                       <cref>url</cref>
        ///                                                   </see>
        ///     is null or empty.</exception>
        public ExternalLinkAttribute(string url, ActionForDependencyType dependencyType, string dependentProperty, ComparisonType comparisonType, object dependentValue) : base(dependencyType, dependentProperty, comparisonType, dependentValue)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            Url = url;
            Name = url;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.ExternalLinkAttribute" /> class.
        /// </summary>
        /// <param name="url">External URL.</param>
        /// <param name="name">Name of link.</param>
        /// <param name="dependencyType">The type of action to take if the dependency condition is met.</param>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="comparisonType">The comparison type to use.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        /// <exception cref="ArgumentNullException">Thrown if <see>
        ///                                                       <cref>url</cref>
        ///                                                   </see>
        ///     is null or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <see>
        ///                                                       <cref>name</cref>
        ///                                                   </see>
        ///     is null or empty.</exception>
        public ExternalLinkAttribute(string url, string name, ActionForDependencyType dependencyType, string dependentProperty, ComparisonType comparisonType, object dependentValue) : base(dependencyType, dependentProperty, comparisonType, dependentValue)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            Url = url;
            Name = name;
        }

        /// <summary>
        /// Implementation for the ISplitButtonChild interface
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public MvcHtmlString Render(HtmlHelper html)
        {
            return html.RenderExternalLink(this, html.ViewData.ModelMetadata.Properties, html.ViewData.Model);
        }

    }
}
