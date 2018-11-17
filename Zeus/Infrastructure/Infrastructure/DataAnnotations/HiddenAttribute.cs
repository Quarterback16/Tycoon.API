using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Extensions;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate that the property should be hidden.
    /// </summary>
    /// <remarks>
    /// Use [Hidden] instead of [HiddenInput(DisplayValue = false)].
    /// Use [Hidden(LabelOnly = true)] instead of [HiddenInput(DisplayValue = true)].
    /// Use [Hidden(ExcludeFromView = true)] for capturing values like the 'submitType' in your View Model on postback.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class HiddenAttribute : Attribute, IMetadataAware
    {
        /// <summary>
        /// Whether only the label should be hidden.
        /// </summary>
        public bool LabelOnly { get; set; }

        /// <summary>
        /// Whether the property is not just hidden from display but excluded from the view entirely (not in HTML mark-up).
        /// </summary>
        public bool ExcludeFromView { get; set; }

        /// <summary>
        /// On metadata created.
        /// </summary>
        /// <param name="metadata">Metadata.</param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            if (!LabelOnly)
            {
                metadata.HideSurroundingHtml = true;
                metadata.TemplateHint = "HiddenInput";
            }
        }
    }
}
