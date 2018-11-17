using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Types;
#if DEBUG
using StackExchange.Profiling;
#endif
namespace Employment.Web.Mvc.Infrastructure.Helpers
{
    /// <summary>
    /// Represents a helper that is used to assist with Grids.
    /// </summary>
    public class GridHelper
    {
        /// <summary>
        /// get key meta data from model metadata properties
        /// </summary>
        /// <param name="modelMetadataProperties"></param>
        /// <returns></returns>
        public static ModelMetadata GetKeyMetadata(IEnumerable<ModelMetadata> modelMetadataProperties)
        {
#if DEBUG
            var step = MiniProfiler.Current.Step("GridHelper.GetKeyMetadata");

            try
            {
#endif
            var metadataProperties = modelMetadataProperties as List<ModelMetadata> ?? modelMetadataProperties.ToList();

            if (metadataProperties.Count(m => m.GetAttribute<KeyAttribute>() != null) != 1)
            {
                throw new SelectorException("There must be only 1 property in the view model decorated with the [Key] attribute.");
            }

            return metadataProperties.SingleOrDefault(m => m.GetAttribute<KeyAttribute>() != null);

#if DEBUG
            }
            finally
            {
                if (step != null)
                {
                    step.Dispose();
                }
            }
#endif
        }

        /// <summary>
        /// Get selection type
        /// </summary>
        /// <param name="modelMetadata"></param>
        /// <param name="parentModelPropertyMetadata"></param>
        /// <returns></returns>
        public static SelectionType GetSelectionType(ModelMetadata modelMetadata, ModelMetadata parentModelPropertyMetadata)
        {
 #if DEBUG
            var step = MiniProfiler.Current.Step("GridHelper.GetSelectionType");

            try
            {
#endif
                // Check for SelectionType on property in parent model
            var selectionTypeAttribute = parentModelPropertyMetadata.GetAttribute<SelectionTypeAttribute>();

            if (selectionTypeAttribute != null)
            {
                return selectionTypeAttribute.SelectionType;
            }

            // If not found, check for SelectionType on model
            selectionTypeAttribute = modelMetadata.GetAttribute<SelectionTypeAttribute>();

            return (selectionTypeAttribute != null) ? selectionTypeAttribute.SelectionType : SelectionType.Default;
#if DEBUG
            }
            finally
            {
                if (step != null)
                {
                    step.Dispose();
                }
            }
#endif
        }

        /// <summary>
        /// Get selector metadata
        /// </summary>
        /// <param name="modelMetadata"></param>
        /// <param name="modelPropertiesMetadata"></param>
        /// <param name="parentModelPropertyMetadata"></param>
        /// <param name="parentModelPropertiesMetadata"></param>
        /// <returns></returns>
        public static ModelMetadata GetSelectorMetadata(ModelMetadata modelMetadata, IEnumerable<ModelMetadata> modelPropertiesMetadata, ModelMetadata parentModelPropertyMetadata, IEnumerable<ModelMetadata> parentModelPropertiesMetadata)
        {
 #if DEBUG
            var step = MiniProfiler.Current.Step("GridHelper.GetSelectorMetadata");

            try
            {
#endif
           var keyMetadata = GetKeyMetadata(modelPropertiesMetadata);

            if (keyMetadata == null)
            {
                throw new SelectorException("A property in the view model must be marked as the 'Key' when the 'SelectionType' is 'Single' or 'Multiple'.");
            }

            var type = GetSelectionType(modelMetadata, parentModelPropertyMetadata);

            if (type == SelectionType.Single)
            {
                // 'Selector' for single selection should be in parent model

                // Get the 'Selector' properties in the parent model
                var selectorPropertiesMetadata = parentModelPropertiesMetadata.Where(m => m.GetAttribute<SelectorAttribute>() != null).ToList();

                if (selectorPropertiesMetadata.Count==0)
                {
                    throw new SelectorException("A property in the parent view model must be marked as the 'Selector'.");
                }
                
                if (selectorPropertiesMetadata.Count == 1)
                {
                    // 'Selector' from parent model
                    return selectorPropertiesMetadata.SingleOrDefault();
                }

                // If more than 1 Selector, then they all must have TargetProperty defined to indicate which 'Selector' is for which target.
                if (selectorPropertiesMetadata.Count(m => string.IsNullOrEmpty(m.GetAttribute<SelectorAttribute>().TargetProperty)) > 0)
                {
                    throw new SelectorException("More than 1 'Selector' requires the 'TargetProperty' is defined in each 'Selector'.");
                }

                // The 'TargetProperty' must also be unique
                if (selectorPropertiesMetadata.GroupBy(m => m.GetAttribute<SelectorAttribute>().TargetProperty, (key, group) => group.First()).ToArray().Length != selectorPropertiesMetadata.Count())
                {
                    throw new SelectorException("Cannot have more than 1 'Selector' with the same 'TargetProperty'.");
                }

                return selectorPropertiesMetadata.SingleOrDefault(m => string.Equals(m.GetAttribute<SelectorAttribute>().TargetProperty, parentModelPropertyMetadata.PropertyName,StringComparison.Ordinal));
            }

            if (type == SelectionType.Multiple)
            {
                var modelPropertiesMetadataList = modelPropertiesMetadata as List<ModelMetadata> ?? modelPropertiesMetadata.ToList();

                // Selector for multiple seleciton should be in model
                var count = modelPropertiesMetadataList.Count(m => m.GetAttribute<SelectorAttribute>() != null);

                if (count != 1)
                {
                    throw new SelectorException("There must be only 1 property in the view model decorated with the [Selector] attribute.");
                }

                // 'Selector' from model
                return modelPropertiesMetadataList.SingleOrDefault(m => m.GetAttribute<SelectorAttribute>() != null);
            }

            return null;

#if DEBUG
            }
            finally
            {
                if (step != null)
                {
                    step.Dispose();
                }
            }
#endif
        }
    }
}
