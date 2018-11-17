using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate the details of a button for display in a view.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class ButtonAttribute : ContingentAttribute, ISplitButtonChild
    {
        /// <summary>
        /// The submit type name for buttons.
        /// </summary>
        public static readonly string SubmitTypeName = "submitType";

        /// <summary>
        /// The type ID of this attribute.
        /// </summary>
        /// <remarks>
        /// When <see cref="AttributeUsageAttribute.AllowMultiple" /> is true, <see cref="TypeId" /> must be overriden to return the current instance so <see cref="TypeDescriptor" /> can properly return multiple attributes of the same type.
        /// </remarks>
        public override object TypeId { get { return this; } }

        /// <summary>
        /// Display name of button.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The submit type of the button which will come through to the action as a 'submitType' parameter or to be used by <see cref="ButtonHandlerAttribute" />.
        /// </summary>
        /// <remarks>
        /// On POST, this is the value that will be supplied to the action "string submitType" parameter or by the <see cref="ButtonHandlerAttribute" /> to determine the action that will handle the submit.
        /// </remarks>
        public string SubmitType { get; set; }
        
        /// <summary>
        /// The name of the group the button belongs to, if any.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// If set, this indicates that a button is a sub button within a split button.
        /// This property indicates the name of the top level button that this button lies under.
        /// </summary>
        public string SplitButtonParent { get; set; }

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
        /// Whether the button will reset the form to its initial values.
        /// </summary>
        public bool Reset { get; set; }

        /// <summary>
        /// Whether the button will clear the form fields along with any pre-set values.
        /// </summary>
        public bool Clear { get; set; }

        /// <summary>
        /// Whether the button is the primary button action.
        /// </summary>
        /// <remarks>
        /// Should not be used if <see cref="Cancel" /> is <c>true</c>.
        /// </remarks>
        public bool Primary { get; set; }

        /// <summary>
        /// Whether the button is the cancel button action.
        /// </summary>
        /// <remarks>
        /// Should not be used if <see cref="Primary" /> is <c>true</c>.
        /// </remarks>
        public bool Cancel { get; set; }

        /// <summary>
        /// Whether to skip client side validation when this button is selected.
        /// </summary>
        public bool SkipClientSideValidation { get; set; }

        /// <summary>
        /// If specified, the user must belong to at least one of the Roles to see the button.
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// Whether this button results in the downloading of a file.
        /// </summary>
        /// <remarks>
        /// If <c>true</c>, results in the UI blocking for preventing duplicate submits from timing out after a short time.
        /// </remarks>
        public bool ResultsInDownload { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.ButtonAttribute" /> class.
        /// </summary>
        /// <param name="name">Name of button.</param>
        /// <param name="groupName">Name of the group that button should be placed in.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is <c>null</c> or empty.</exception>
        public ButtonAttribute(string name, string groupName = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (! string.IsNullOrEmpty(groupName))
            {
                GroupName = groupName;
            }

            Name = name; 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.ButtonAttribute" /> class.
        /// </summary>
        /// <param name="name">Name of button.</param>
        /// <param name="dependencyType">The type of action to take if the dependency condition is met.</param>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="comparisonType">The comparison type to use.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is <c>null</c> or empty.</exception>
        public ButtonAttribute(string name, ActionForDependencyType dependencyType, string dependentProperty, ComparisonType comparisonType, object dependentValue) : base(dependencyType, dependentProperty, comparisonType, dependentValue)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.ButtonAttribute" /> class.
        /// </summary>
        /// <param name="name">Name of button.</param>
        /// <param name="groupName">Name of the group that button should be placed in.</param>
        /// <param name="dependencyType">The type of action to take if the dependency condition is met.</param>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="comparisonType">The comparison type to use.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is <c>null</c> or empty.</exception>
        public ButtonAttribute(string name, string groupName, ActionForDependencyType dependencyType, string dependentProperty, ComparisonType comparisonType, object dependentValue) : base(dependencyType, dependentProperty, comparisonType, dependentValue)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentNullException("groupName");
            }

            Name = name;
            GroupName = groupName;
        }

        /// <summary>
        /// Get the value for use in a button.
        /// </summary>
        /// <returns>The value.</returns>
        public string GetValue()
        {
            return (!string.IsNullOrEmpty(SubmitType)) ? SubmitType : Name;
        }

        /// <summary>
        /// 
        /// </summary>
        public MvcHtmlString Render(HtmlHelper html)
        {
            return Render(html, html.ViewData.Model);
        }

        /// <summary>
        /// 
        /// </summary>
        public MvcHtmlString Render(HtmlHelper html, object container)
        {
            return Render(html, container, html.ViewData.TemplateInfo.HtmlFieldPrefix);
        }

        /// <summary>
        /// 
        /// </summary>
        public MvcHtmlString Render(HtmlHelper html, object container, string fieldPrefix)
        {
            return Render(html, container, fieldPrefix, null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        public MvcHtmlString Render(HtmlHelper html, object container, string fieldPrefix, string property, object propertyValue)
        {
            MvcHtmlString result = MvcHtmlString.Empty;

            if (DependencyType == ActionForDependencyType.None)
            {
                result = html.Button(this);
            }
            else
            {

                var htmlAttributes = new Dictionary<string, object>();

                htmlAttributes.MergeCssClass("rhea-actionif");

                htmlAttributes.Add(HtmlDataType.FieldPrefix, fieldPrefix);
                htmlAttributes.Add(HtmlDataType.ActionForDependencyType, DependencyType);
                htmlAttributes.Add(HtmlDataType.DependentProperty, DependentProperty);
                htmlAttributes.Add(HtmlDataType.ComparisonType, ComparisonType);
                htmlAttributes.Add(HtmlDataType.DependentValue, DependentValue);
                htmlAttributes.Add(HtmlDataType.Type, "button");

                var conditionMet = !string.IsNullOrEmpty(property) ? IsConditionMet(property, propertyValue, container) : IsConditionMet(container);

                switch (DependencyType)
                {
                    case ActionForDependencyType.Enabled:
                        if (!conditionMet)
                        {
                            htmlAttributes.Add("disabled", "disabled");
                        }
                        break;
                    case ActionForDependencyType.Disabled:
                        if (conditionMet)
                        {
                            htmlAttributes.Add("disabled", "disabled");
                        }
                        break;
                    case ActionForDependencyType.Visible:
                        if (!conditionMet)
                        {
                            htmlAttributes.MergeCssClass("hidden");
                        }

                        break;
                    case ActionForDependencyType.Hidden:
                        if (conditionMet)
                        {
                            htmlAttributes.MergeCssClass("hidden");
                        }
                        break;
                }

                result = html.Button(this, htmlAttributes);
            }

            return result;
        }

    }
}
