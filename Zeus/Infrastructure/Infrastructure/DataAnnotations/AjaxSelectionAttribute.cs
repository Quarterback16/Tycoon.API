using System;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Extensions;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate the selection is populated by Ajax.
    /// </summary>
    /// <remarks>
    /// Only supports <see cref="SelectionType.Single" /> properties.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class AjaxSelectionAttribute : Attribute, IMetadataAware
    {
        /// <summary>
        /// The controller action that will handle the Ajax request.
        /// </summary>
        public string Action { get; private set; }
        
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
        /// The name of the route to use when generating the link.
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AjaxSelectionAttribute" /> class.
        /// </summary>
        /// <param name="action">The controller action that will handle the Ajax request.</param>
        public AjaxSelectionAttribute(string action)
        {
            Action = action;
        }

        /// <summary>
        /// On metadata created.
        /// </summary>
        /// <param name="metadata">Metadata.</param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            // If a multi select list is being created, the Select2 library requires that the script be attached to an input textbox instead of a select list
            // Unfortunately this means the rendering path for server side multi selects is entirely different to other select lists.
            if (metadata.ModelType == typeof(MultiSelectList))
            {
                metadata.TemplateHint = "MultiSelectListServerSide";
            }
            else if (metadata.ModelType.IsNumeric())
            {
                // Set numeric property template to string if decorated with the [AjaxSelection] attribute as it will be transformed into an auto-complete drop-down
                metadata.TemplateHint = "String";
            }
        }
    }
}
