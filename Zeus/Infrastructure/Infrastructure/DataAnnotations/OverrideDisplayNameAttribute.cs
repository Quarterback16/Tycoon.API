using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate the property to use for retrieving the override value for the property's <see cref="DisplayAttribute.Name" /> value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OverrideDisplayNameAttribute : Attribute, IMetadataAware
    {
        /// <summary>
        /// The name of the property to get the override value from.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OverrideDisplayNameAttribute" /> class.
        /// </summary>
        /// <param name="propertyName">The name of the property to get the value to override <see cref="DisplayAttribute.Name" /> with.</param>
        public OverrideDisplayNameAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// On metadata created.
        /// </summary>
        /// <param name="metadata">Metadata.</param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            var parentModel = metadata.AdditionalValues.ContainsKey("ParentModel") ? metadata.AdditionalValues["ParentModel"] : null;

            if (parentModel == null)
            {
                return;
            }

            var overrideValue = GetPropertyValue(parentModel, PropertyName) as string;

            if (!string.IsNullOrEmpty(overrideValue))
            {
                metadata.DisplayName = overrideValue;
            }
        }

        private object GetPropertyValue(object parentModel, string propertyName)
        {
            if (parentModel != null && !string.IsNullOrEmpty(propertyName))
            {
                var property = parentModel.GetType().GetProperty(propertyName);

                if (property != null)
                {
                    return property.GetValue(parentModel, null);
                }
            }

            return null;
        }
    }
}
