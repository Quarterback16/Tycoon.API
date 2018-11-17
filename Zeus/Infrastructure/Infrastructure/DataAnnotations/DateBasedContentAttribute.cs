using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;


namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate a variable content section that is fetched via AJAX and determined based on a user selected date
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateBasedContentAttribute : Attribute, IMetadataAware
    {
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
        /// On metadata created.
        /// </summary>
        /// <param name="metadata">Metadata.</param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            //metadata.HideSurroundingHtml = true;
            metadata.TemplateHint = "DateBasedContent";
        }

    }
}
