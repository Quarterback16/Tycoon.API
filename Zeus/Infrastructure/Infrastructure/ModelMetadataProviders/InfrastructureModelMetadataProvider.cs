using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.ViewModels.Geospatial;
using ReadOnlyAttribute = System.ComponentModel.ReadOnlyAttribute;
using Employment.Web.Mvc.Infrastructure.ViewModels.JobSeeker;
#if DEBUG
using StackExchange.Profiling;
#endif

namespace Employment.Web.Mvc.Infrastructure.ModelMetadataProviders
{
    /// <summary>
    /// Represents a custom Model Metadata Provider. 
    /// </summary>
    public class InfrastructureModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        /// <summary>Gets the metadata for the specified property.</summary>
        /// <returns>The metadata for the property.</returns>
        /// <param name="attributes">The attributes.</param>
        /// <param name="containerType">The type of the container.</param>
        /// <param name="modelAccessor">The model accessor.</param>
        /// <param name="modelType">The type of the model.</param>
        /// <param name="propertyName">The name of the property.</param>
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
      #if DEBUG
            var step = MiniProfiler.Current.Step("InfrastructureModelMetadataProvider.CreateMetadata");

            try
            {
#endif
           var attrList = attributes.ToList();
            var metadata = base.CreateMetadata(attrList, containerType, modelAccessor, modelType, propertyName);

            // Add all attributes into additional values
            metadata.AdditionalValues.Add("Attributes", attributes);

            if (!string.IsNullOrEmpty(propertyName) && (containerType == null || (Nullable.GetUnderlyingType(containerType) == null)))
            {
                // Get parent model to be able to retrieve dependent property values
                var parentModel = GetParentModel(modelAccessor);

                if (parentModel != null)
                {
                    metadata.AdditionalValues.Add("ParentModel", parentModel);
                    metadata.AdditionalValues.Add("PropertyNameInParentModel", propertyName);

                    var propertyValue = GetPropertyValue(parentModel, propertyName);

                    if (metadata.IsComplexType && modelType.IsGenericType && modelType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    {
                        var enumerable = propertyValue as IEnumerable<dynamic>;

                        if (enumerable != null && enumerable.Any())
                        {
                            if (attrList.OfType<DataTypeAttribute>().Any(a => a != null && a.DataType == DataType.Custom && a.CustomDataType == CustomDataType.Grid))
                            {
                                metadata.TemplateHint = "Grid";
                            }
                        }
                    }

                    if (propertyValue == null && metadata.IsComplexType && ((modelType == typeof(ContentViewModel)) || (modelType.IsGenericType && modelType.GetGenericTypeDefinition() == typeof(IEnumerable<>) && !(attributes.OfType<SelectionAttribute>().Any() || attributes.OfType<AdwSelectionAttribute>().Any() || attributes.OfType<AjaxSelectionAttribute>().Any()) && modelType != typeof(IEnumerable<SelectListItem>) && modelType != typeof(SelectList) && modelType != typeof(MultiSelectList))))
                    {
                        metadata.HideSurroundingHtml = true;   
                    }

                    if (parentModel is AddressViewModel && propertyName == AddressViewModel.AjaxProperty)
                    {
                        metadata.AdditionalValues[AddressViewModel.AjaxPropertyModelMetadataKey] = true;

                        if( ((AddressViewModel)parentModel).ReturnLatLongDetails)
                        {
                            metadata.AdditionalValues[AddressViewModel.ReturnLatLongData] = true;
                        }
                    }

                    if(parentModel is JobseekerSearchViewModel && propertyName == JobseekerSearchViewModel.AjaxProperty )
                    {
                        metadata.AdditionalValues[JobseekerSearchViewModel.AjaxPropertyModelMetadataKey] = true;
                    }
                }
            }
            
            var elementType = modelType.IsGenericType && modelType.HasElementType ? modelType.GetElementType() : modelType.GetGenericArguments().FirstOrDefault();

            // Override Template for enums
            if (modelType.GetNonNullableType().IsEnum || elementType != null && elementType.IsEnum)
            {
                metadata.TemplateHint = "Enum";
            }

            if (modelType.IsNumeric())
            {
                metadata.TemplateHint = "Number";
            }
                
            // Override Template for file upload properties
            if (modelType == typeof(HttpPostedFileBase))
            {
                metadata.TemplateHint = "File";
            }

            return metadata;
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

        private static object GetPropertyValue(object parentModel, string propertyName)
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
        
        private static object GetParentModel(Func<object> modelAccessor)
        {
            if (modelAccessor != null && modelAccessor.Target != null)
            {
                var container = modelAccessor.Target.GetType().GetFields().FirstOrDefault(f => "container".Equals(f.Name, StringComparison.Ordinal));

                if (container != null)
                {
                    return container.GetValue(modelAccessor.Target);
                }

                container = modelAccessor.Target.GetType().GetFields().FirstOrDefault(f => "vdi".Equals( f.Name,StringComparison.Ordinal ));

                if (container != null)
                {
                    var viewDataInfo = container.GetValue(modelAccessor.Target) as ViewDataInfo;

                    if (viewDataInfo != null)
                    {
                        return viewDataInfo.Container;
                    }
                }
            }

            return null;
        }
    }
}
