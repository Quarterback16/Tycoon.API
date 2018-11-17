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
using Employment.Web.Mvc.Infrastructure.ViewModels.JobSeeker;

#if DEBUG
using StackExchange.Profiling;
#endif

namespace Employment.Web.Mvc.Infrastructure.ModelMetadataProviders
{
    /// <summary>
    /// Represents a custom Model Metadata Provider. 
    /// </summary>
    public class InfrastructureCachedModelMetadataProvider : CachedDataAnnotationsModelMetadataProvider
    {

        /// <summary>
        /// Creates the metadata from prototype.
        /// </summary>
        /// <param name="prototype">The prototype.</param>
        /// <param name="modelAccessor">The model accessor.</param>
        /// <returns>metadata</returns>
        protected override CachedDataAnnotationsModelMetadata CreateMetadataFromPrototype(CachedDataAnnotationsModelMetadata prototype, Func<object> modelAccessor)
        {
            var result = base.CreateMetadataFromPrototype(prototype, modelAccessor);

            //modify the base result with your custom logic, typically adding items from prototype.AdditionalValues
            result.AdditionalValues.Add("Attributes", prototype.AdditionalValues["Attributes"]);
            var attrList = prototype.AdditionalValues["Attributes"] as IEnumerable<Attribute>;
            var attributes = attrList;

            var propertyName = prototype.PropertyName;
            var containerType = prototype.ContainerType;
            var modelType = prototype.ModelType;
            if (!string.IsNullOrEmpty(prototype.PropertyName) && (containerType == null || (Nullable.GetUnderlyingType(containerType) == null)))
            {
                // Get parent model to be able to retrieve dependent property values
                var parentModel = GetParentModel(modelAccessor);

                if (parentModel != null)
                {
                    result.AdditionalValues.Add("ParentModel", parentModel);
                    result.AdditionalValues.Add("PropertyNameInParentModel", propertyName);

                    var propertyValue = GetPropertyValue(parentModel, propertyName);

                    if (prototype.IsComplexType && modelType.IsGenericType && modelType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    {
                        var enumerable = propertyValue as IEnumerable<dynamic>;

                        if (enumerable != null && enumerable.Any())
                        {
                            if (attrList.OfType<DataTypeAttribute>().Any(a => a != null && a.DataType == DataType.Custom && a.CustomDataType == CustomDataType.Grid))
                            {
                                result.TemplateHint = "Grid";
                            }
                        }
                    }

                    if (propertyValue == null && prototype.IsComplexType && ((modelType == typeof(ContentViewModel)) || (modelType.IsGenericType && modelType.GetGenericTypeDefinition() == typeof(IEnumerable<>) && !(attributes.OfType<SelectionAttribute>().Any() || attributes.OfType<AdwSelectionAttribute>().Any() || attributes.OfType<AjaxSelectionAttribute>().Any()) && modelType != typeof(IEnumerable<SelectListItem>) && modelType != typeof(SelectList) && modelType != typeof(MultiSelectList))))
                    {
                        result.HideSurroundingHtml = true;
                    }

                    if (parentModel is AddressViewModel && propertyName == AddressViewModel.AjaxProperty)
                    {
                        result.AdditionalValues[AddressViewModel.AjaxPropertyModelMetadataKey] = true;

                        if( (parentModel as AddressViewModel).ReturnLatLongDetails )
                        {
                            result.AdditionalValues[AddressViewModel.ReturnLatLongData] = true;
                        }
                    }

                    if(parentModel is JobseekerSearchViewModel && propertyName == JobseekerSearchViewModel.AjaxProperty)
                    {
                        result.AdditionalValues[JobseekerSearchViewModel.AjaxPropertyModelMetadataKey] = true;
                    }
                }
            }


            result.TemplateHint = prototype.TemplateHint;

            return result;
        }

        /// <summary>
        /// Creates the metadata prototype.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <param name="containerType">Type of the container.</param>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>metadata prototype</returns>
        protected override CachedDataAnnotationsModelMetadata CreateMetadataPrototype(IEnumerable<Attribute> attributes, Type containerType, Type modelType, string propertyName)
        {
            var attrList = attributes.ToList();
            CachedDataAnnotationsModelMetadata prototype = base.CreateMetadataPrototype(attrList, containerType, modelType, propertyName);

            //Add custom prototype data, e.g.
            prototype.AdditionalValues.Add("Attributes", attributes);

            //TODO: can any of this griddy stuff be done in the prototype
            //if (!string.IsNullOrEmpty(propertyName) && (containerType == null || (Nullable.GetUnderlyingType(containerType) == null)))
            //{
            //    // Get parent model to be able to retrieve dependent property values
            //    var parentModel = GetParentModel(modelAccessor);

            //    if (parentModel != null)
            //    {
            //        prototype.AdditionalValues.Add("ParentModel", parentModel);
            //        prototype.AdditionalValues.Add("PropertyNameInParentModel", propertyName);

            //        var propertyValue = GetPropertyValue(parentModel, propertyName);

            //        if (prototype.IsComplexType && modelType.IsGenericType && modelType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            //        {
            //            var enumerable = propertyValue as IEnumerable<dynamic>;

            //            if (enumerable != null && enumerable.Any())
            //            {
            //                if (attrList.OfType<DataTypeAttribute>().Any(a => a != null && a.DataType == DataType.Custom && a.CustomDataType == CustomDataType.Grid))
            //                {
            //                    prototype.TemplateHint = "Grid";
            //                }
            //            }
            //        }

            //        if (propertyValue == null && prototype.IsComplexType && ((modelType == typeof(ContentViewModel)) || (modelType.IsGenericType && modelType.GetGenericTypeDefinition() == typeof(IEnumerable<>) && !(attributes.OfType<SelectionAttribute>().Any() || attributes.OfType<AdwSelectionAttribute>().Any() || attributes.OfType<AjaxSelectionAttribute>().Any()) && modelType != typeof(IEnumerable<SelectListItem>) && modelType != typeof(SelectList) && modelType != typeof(MultiSelectList))))
            //        {
            //            prototype.HideSurroundingHtml = true;
            //        }

            //        if (parentModel is AddressViewModel && propertyName == AddressViewModel.AjaxProperty)
            //        {
            //            prototype.AdditionalValues[AddressViewModel.AjaxPropertyModelMetadataKey] = true;
            //        }
            //    }
            //}

            var elementType = modelType.IsGenericType && modelType.HasElementType ? modelType.GetElementType() : modelType.GetGenericArguments().FirstOrDefault();

            // Override Template for enums
            if (modelType.GetNonNullableType().IsEnum || elementType != null && elementType.IsEnum)
            {
                prototype.TemplateHint = "Enum";
            }

            if (modelType.IsNumeric())
            {
                prototype.TemplateHint = "Number";
            }

            // Override Template for file upload properties
            if (modelType == typeof(HttpPostedFileBase))
            {
                prototype.TemplateHint = "File";
            }

            return prototype;
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
