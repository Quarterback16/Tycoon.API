using Employment.Web.Mvc.Infrastructure.Types;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to copy the value of the dependent property when its value is changed (client-side only).
    /// </summary>
    public class CopyAttribute : Attribute, IMetadataAware
    {
        /// <summary>
        /// Other property or properties in the model that this property is dependent on.
        /// </summary>
        public object DependentProperty { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dependentProperty" /> is null or empty.</exception>
        public CopyAttribute(object dependentProperty)
        {
            if (dependentProperty == null)
            {
                throw new ArgumentNullException("dependentProperty");
            }

            DependentProperty = dependentProperty;
        }

        /// <summary>
        /// On metadata created.
        /// </summary>
        /// <param name="metadata">Metadata.</param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            var htmlAttributes = new Dictionary<string, object>();

            var values = DependentProperty as object[];

            if (values != null)
            {
                htmlAttributes.Add(HtmlDataType.DependentPropertiesCopy, string.Format("[\"{0}\"]", string.Join("\",\"", values)));
            }
            else
            {
                htmlAttributes.Add(HtmlDataType.DependentPropertiesCopy, DependentProperty);
            }

            metadata.AdditionalValues.Add("copy", htmlAttributes);
        }
    }
}
