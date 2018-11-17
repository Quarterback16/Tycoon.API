using System;
using Employment.Web.Mvc.Infrastructure.Types;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate the selection type of a class or enumerable property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class SelectionTypeAttribute : ValidationAttribute, IMetadataAware
    {
        /// <summary>
        /// The selection type of the class or enumerable property.
        /// </summary>
        public SelectionType SelectionType { get; private set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.SelectionTypeAttribute" /> class.
        /// </summary>
        /// <param name="selectionType">The <see cref="SelectionType" /> setting.</param>
        public SelectionTypeAttribute(SelectionType selectionType)
        {
            SelectionType = selectionType;
        }

        /// <summary>Determines whether the specified value of the object is valid.</summary>
		/// <param name="value">The value of the object to validate.</param>
        /// <returns>True if the specified value is valid; otherwise, false.</returns>
		public override bool IsValid(object value)
        {
            return true;
        }

        /// <summary>
        /// On metadata created.
        /// </summary>
        /// <param name="metadata">Metadata.</param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            if (metadata.ModelType == typeof(IEnumerable<SelectListItem>))
            {
                metadata.TemplateHint = SelectionType == SelectionType.Multiple ? "MultiSelectList" : "SelectList";
            }
        }
    }
}
