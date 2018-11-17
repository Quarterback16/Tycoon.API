using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Types;
using System.ComponentModel;
#if DEBUG
using StackExchange.Profiling;
#endif

namespace Employment.Web.Mvc.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for <see cref="ModelMetadata" />.
    /// </summary>
    public static class ModelMetadataExtension
    {
        /// <summary>
        /// Get the parent model as object.
        /// </summary>
        /// <typeparam name="TModel">The parent model <see cref="Type" />.</typeparam>
        /// <param name="modelMetadata">The <see cref="ModelMetadata"/> instance.</param>
        /// <returns>The parent model if found; otherwise, <c>null</c>.</returns>
        public static object GetParentModel(this ModelMetadata modelMetadata)
        {
            return modelMetadata.AdditionalValues.ContainsKey("ParentModel") ? modelMetadata.AdditionalValues["ParentModel"] : null;
        }

        /// <summary>
        /// Get the parent model as a specific type.
        /// </summary>
        /// <typeparam name="TModel">The parent model <see cref="Type" />.</typeparam>
        /// <param name="modelMetadata">The <see cref="ModelMetadata"/> instance.</param>
        /// <returns>The parent model if found; otherwise, <c>null</c>.</returns>
        public static TModel GetParentModel<TModel>(this ModelMetadata modelMetadata) where TModel : class
        {
            return modelMetadata.GetParentModel() as TModel;
        }

        /// <summary>
        /// Set the parent model.
        /// </summary>
        /// <typeparam name="TModel">The parent model <see cref="Type" />.</typeparam>
        /// <param name="modelMetadata">The <see cref="ModelMetadata"/> instance.</param>
        /// <param name="model">The parent model object.</param>
        public static void SetParentModel<TModel>(this ModelMetadata modelMetadata, TModel model) where TModel : class
        {
            modelMetadata.AdditionalValues["ParentModel"] = model;
        }

        /// <summary>
        /// Get attributes of a specific type.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute <see cref="Type" />.</typeparam>
        /// <param name="modelMetadata">The <see cref="ModelMetadata"/> instance.</param>
        /// <returns>An <see cref="IEnumerable{Attribute}"/> collection.</returns>
        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this ModelMetadata modelMetadata) where TAttribute : Attribute
        {
            //     #if DEBUG
            //            var step = MiniProfiler.Current.Step("ModelMetadata.GetAttributes<TAttribute>");

            //            try
            //            {
            //#endif
            IEnumerable<Attribute> attributes = modelMetadata.AdditionalValues["Attributes"] as IEnumerable<Attribute>;

            return attributes != null ? attributes.OfType<TAttribute>() : Enumerable.Empty<TAttribute>();
            //#if DEBUG
            //            }
            //            finally
            //            {
            //                if (step != null)
            //                {
            //                    step.Dispose();
            //                }
            //            }
            //#endif
        }

        /// <summary>
        /// Get attributes of a specific type.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute <see cref="Type" />.</typeparam>
        /// <param name="modelMetadata">The <see cref="ModelMetadata"/> instance.</param>
        /// <param name="predicate">Filters attributes based on the predicate.</param>
        /// <returns>An <see cref="IEnumerable{Attribute}"/> collection.</returns>
        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this ModelMetadata modelMetadata, Func<TAttribute, bool> predicate) where TAttribute : Attribute
        {
            return modelMetadata.GetAttributes<TAttribute>().Where(predicate);
        }

        /// <summary>
        /// Get attribute of a specific type.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute <see cref="Type" />.</typeparam>
        /// <param name="modelMetadata">The <see cref="ModelMetadata"/> instance.</param>
        /// <returns>The attribute.</returns>
        public static TAttribute GetAttribute<TAttribute>(this ModelMetadata modelMetadata) where TAttribute : Attribute
        {
            return modelMetadata.GetAttributes<TAttribute>().FirstOrDefault();
        }

        /// <summary>
        /// Get attribute of a specific type.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute <see cref="Type" />.</typeparam>
        /// <param name="modelMetadata">The <see cref="ModelMetadata"/> instance.</param>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <returns>The attribute.</returns>
        public static TAttribute GetAttribute<TAttribute>(this ModelMetadata modelMetadata, Func<TAttribute, bool> predicate) where TAttribute : Attribute
        {
            return modelMetadata.GetAttributes<TAttribute>().FirstOrDefault(predicate);
        }

        /// <summary>
        /// Determine if the property is a ViewModel or enumerable ViewModel.
        /// </summary>
        /// <param name="modelMetadata">The <see cref="ModelMetadata"/> instance.</param>
        /// <returns><c>true</c> if the property is a ViewModel or enumerable ViewModel; otherwise, <c>false</c>.</returns>
        public static bool IsViewModel(this ModelMetadata modelMetadata)
        {
#if DEBUG
            var step = MiniProfiler.Current.Step("ModelMetadata.IsViewModel");

            try
            {
#endif
                var isViewModel = (modelMetadata.ModelType != null && modelMetadata.ModelType.FullName != null && modelMetadata.ModelType.FullName.EndsWith("ViewModel", StringComparison.Ordinal)) || modelMetadata.GetAttribute<ViewModelAttribute>() != null;

                if (!isViewModel)
                {
                    var underlyingType = modelMetadata.ModelType.GetUnderlyingType();

                    if (underlyingType != null)
                    {
                        isViewModel = (underlyingType.FullName != null && underlyingType.FullName.EndsWith("ViewModel", StringComparison.Ordinal)) || TypeDescriptor.GetAttributes(underlyingType).OfType<ViewModelAttribute>().Any();
                    }
                }

                return isViewModel;
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
        /// Determine if the property is a read only hyperlink.
        /// </summary>
        /// <param name="modelMetadata">The <see cref="ModelMetadata"/> instance.</param>
        /// <returns><c>true</c> if the property is a read only hyperlink; otherwise, <c>false</c>.</returns>
        public static bool IsReadOnlyHyperlink(this ModelMetadata modelMetadata)
        {
#if DEBUG
            var step = MiniProfiler.Current.Step("ModelMetadata.IsReadOnlyHyperlink");

            try
            {
#endif
                // Check property is readonly and has a link or external link
                if (modelMetadata.IsReadOnly && !modelMetadata.IsViewModel() && (modelMetadata.HasLink() || modelMetadata.HasExternalLink()))
                {
                    EditableAttribute editable = modelMetadata.GetAttribute<EditableAttribute>();
                    ReadOnlyAttribute readOnly = modelMetadata.GetAttribute<ReadOnlyAttribute>();

                    // Property must ALWAYS be read only to render as a hyperlink
                    if ((editable != null && !editable.AllowEdit) || (readOnly != null && readOnly.IsReadOnly))
                    {
                        var notDefault = ComparisonType.NotEqualTo.Compare(modelMetadata.Model, modelMetadata.ModelType.GetDefaultValue());
                        var selectList = modelMetadata.Model as IEnumerable<SelectListItem>;

                        if (selectList != null)
                        {
                            // Handle "not default value" for IEnumerable<SelectListItem> properties based on whether there are selections
                            notDefault = selectList.Any(p => p.Selected);
                        }

                        // Only a hyperlink if the model has a value
                        if (modelMetadata.ModelType == typeof(string) ? !string.IsNullOrEmpty(modelMetadata.Model as string) : notDefault)
                        {
                            return true;
                        }
                    }
                }

                return false;
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
        /// Determine if the property is has <see cref="LinkAttribute" /> or <see cref="ExternalLinkAttribute" /> attributes.
        /// </summary>
        /// <param name="modelMetadata">The <see cref="ModelMetadata"/> instance.</param>
        /// <returns><c>true</c> if the property has <see cref="LinkAttribute" /> or <see cref="ExternalLinkAttribute" /> attributes; otherwise, <c>false</c>.</returns>
        public static bool HasLink(this ModelMetadata modelMetadata)
        {
            return modelMetadata.GetAttributes<LinkAttribute>().Any();
        }

        /// <summary>
        /// Determine if the property is has <see cref="ExternalLinkAttribute" /> attributes.
        /// </summary>
        /// <param name="modelMetadata">The <see cref="ModelMetadata"/> instance.</param>
        /// <returns><c>true</c> if the property has <see cref="ExternalLinkAttribute" /> attributes; otherwise, <c>false</c>.</returns>
        public static bool HasExternalLink(this ModelMetadata modelMetadata)
        {
            return modelMetadata.GetAttributes<ExternalLinkAttribute>().Any();
        }

        /// <summary>
        /// Determine if the property is visible based on <see cref="VisibleIfAttribute" /> based attributes.
        /// </summary>
        /// <param name="modelMetadata">The <see cref="ModelMetadata"/> instance.</param>
        /// <returns><c>true</c> if the property is visible; otherwise, <c>false</c>.</returns>
        public static bool IsVisible(this ModelMetadata modelMetadata)
        {
            var visibleIfAttributes = modelMetadata.GetAttributes<ContingentAttribute>().Where(a => a.GetType().Name.StartsWith("VisibleIf", StringComparison.Ordinal));

            var visible = true;

            foreach (var visibleIfAttribute in visibleIfAttributes)
            {
                if (!visibleIfAttribute.IsConditionMet(modelMetadata.PropertyName, modelMetadata.Model, modelMetadata.AdditionalValues["ParentModel"]))
                {
                    visible = false;
                }
            }

            return visible;
        }

        /// <summary>
        /// Determine if the property is has <see cref="RequiredIfAttribute" /> based attributes.
        /// </summary>
        /// <param name="modelMetadata">The <see cref="ModelMetadata"/> instance.</param>
        /// <returns><c>true</c> if the property has <see cref="RequiredIfAttribute" /> based attributes; otherwise, <c>false</c>.</returns>
        public static bool HasRequiredIf(this ModelMetadata modelMetadata)
        {
            return modelMetadata.GetAttributes<RequiredIfAttribute>().Any();
        }

        /// <summary>
        /// Determine if the property is required based on <see cref="RequiredIfAttribute" /> based attributes.
        /// </summary>
        /// <param name="modelMetadata">The <see cref="ModelMetadata"/> instance.</param>
        /// <returns><c>true</c> if the property is required; otherwise, <c>false</c>.</returns>
        public static bool IsRequired(this ModelMetadata modelMetadata)
        {
#if DEBUG
            var step = MiniProfiler.Current.Step("ModelMetadata.IsRequired");

            try
            {
#endif
                var requiredIfAttributes = modelMetadata.GetAttributes<RequiredIfAttribute>();

                foreach (RequiredIfAttribute requiredIfAttribute in requiredIfAttributes)
                {
                    if (requiredIfAttribute.DependentProperty != null)
                    {
                        object container = modelMetadata.AdditionalValues["ParentModel"];
                        PropertyInfo property = container.GetType().GetProperty(requiredIfAttribute.DependentProperty);

                        if (property != null)
                        {
                            var dependentPropertyValue = property.GetValue(container, null);

                            if (requiredIfAttribute.IsRequired(modelMetadata.Model, dependentPropertyValue))
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;

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
        /// Find the Model Metadata for a property, including nested properties (delimited by dot).
        /// </summary>
        /// <param name="modelMetadata">The <see cref="ModelMetadata"/> instance.</param>
        /// <param name="propertyName">The property name to find.</param>
        /// <returns>If found, the model metadata of the property name; otherwise, null.</returns>
        public static ModelMetadata Find(this ModelMetadata modelMetadata, string propertyName)
        {
            if (modelMetadata == null || string.IsNullOrEmpty(propertyName))
            {
                return null;
            }

            // Check if property is nested
            if (propertyName.Contains('.'))
            {
                // Split nested property name into its nested segments
                var propertyNameSegments = propertyName.Split('.');

                var currentModelMetadata = modelMetadata;

                // Drill down into the Model Metadata to find the property
                for (int i = 0; i < propertyNameSegments.Length; i++)
                {
                    var propertyNameSegment = propertyNameSegments[i];
                    if (currentModelMetadata != null)
                    {
                        // Get matching property for current segment
                        currentModelMetadata = currentModelMetadata.Properties.FirstOrDefault(p => p.PropertyName.Equals(propertyNameSegment, StringComparison.OrdinalIgnoreCase));
                    }
                }

                // Will be populated if final segment (actual property) was found, otherwise null
                return currentModelMetadata;
            }

            // Find matching property name in current Model Metadata, will be populated if found, otherwise null
            return modelMetadata.Properties.FirstOrDefault(p => p.PropertyName.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Whether the client-side validation should always be skipped.
        /// </summary>
        /// <param name="modelMetadata">The <see cref="ModelMetadata"/> instance.</param>
        /// <returns><c>true</c> if the client-side validation should always be skipped; otherwise, <c>false</c>.</returns>
        public static bool SkipClientSideValidation(this ModelMetadata modelMetadata)
        {
            var attribute = modelMetadata.GetAttribute<SkipClientSideAttribute>();

            return attribute != null && attribute.Validation;
        }

        /// <summary>
        /// Whether the client-side unsaved changes prompt should always be skipped.
        /// </summary>
        /// <param name="modelMetadata">The <see cref="ModelMetadata"/> instance.</param>
        /// <returns><c>true</c> if the client-side unsaved changes prompt should always be skipped; otherwise, <c>false</c>.</returns>
        public static bool SkipClientSideUnsavedChanges(this ModelMetadata modelMetadata)
        {
            var attribute = modelMetadata.GetAttribute<SkipClientSideAttribute>();

            return attribute != null && attribute.UnsavedChanges;
        }
        
        public static GroupType? GetGroupType(this HtmlHelper html, string propertyGroupKey)
        {
            return html.ViewData.ModelMetadata.GetAttributes<GroupAttribute>().Where(g => string.Equals(g.Name, propertyGroupKey, StringComparison.Ordinal)).Select(g => g.GroupType).FirstOrDefault();

        }

        public static Dictionary<string, List<ModelMetadata>> GetGroupedAndOrderedMetadataAsList(this ViewDataDictionary viewData)
        {
            var propertyGroups = viewData.GetGroupedAndOrderedMetadata();
            var dictionary = new Dictionary<string, List<ModelMetadata>>();

            foreach (var propertyGroup in propertyGroups)
            {
                var list = new List<ModelMetadata>();

                foreach (var property in propertyGroup)
                {
                    list.Add(property);
                }

                if (string.IsNullOrEmpty(propertyGroup.Key))
                {
                    if (dictionary.ContainsKey(string.Empty))
                    {
                        dictionary[string.Empty].AddRange(list);
                    }
                    else
                    {
                        dictionary.Add(string.Empty, list);
                    }
                }
                else
                {
                    dictionary.Add(propertyGroup.Key, list);
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Group and order Model Metadata based on the Display attribute Order and GroupName settings.
        /// </summary>
        /// <param name="viewData">The ViewData instance.</param>
        /// <returns>Grouped and ordered Model Metadata.</returns>
        public static IOrderedEnumerable<IGrouping<string, ModelMetadata>> GetGroupedAndOrderedMetadata(this ViewDataDictionary viewData)
        {
            var groups = viewData.ModelMetadata.GetAttributes<GroupAttribute>();

            var propertyGroups = viewData.ModelMetadata.Properties
                .Where(p => p.ShowForEdit && !viewData.TemplateInfo.Visited(p))
                // Order properties by Display.GetOrder()
                .OrderBy(p => p.GetAttribute<DisplayAttribute>() != null ? p.GetAttribute<DisplayAttribute>().GetOrder() : int.MaxValue)
                // Group properties by Display.GetGroupName()
                .GroupBy(p => p.GetAttribute<DisplayAttribute>() != null ? p.GetAttribute<DisplayAttribute>().GetGroupName() : string.Empty)
                // Order groups by Group.Order where Group.Name matches group name
                .OrderBy(g =>
                {
                    if (groups != null)
                    {
                        var group = groups.SingleOrDefault(a => String.Equals(a.Name, g.Key, StringComparison.Ordinal));

                        return group != null ? group.Order : int.MaxValue;
                    }

                    return int.MaxValue;
                });

            return propertyGroups;
        }

        /// <summary>
        /// Gets the default value based on Model Metadata and <see cref="DefaultValueAttribute"/>.
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <param name="modelMetadata"><see cref="ModelMetadata"/> instance.</param>
        /// <param name="value">The default value.</param>
        /// <returns><c>true</c> if there is a default value; otherwise, <c>false</c>.</returns>
        public static bool TryGetDefaultValue<T>(this ModelMetadata modelMetadata, out T value)
        {
            value = default(T);
            var defaultValue = modelMetadata.GetAttribute<DefaultValueAttribute>();
            
            if (defaultValue == null)
            {
                return false;
            }

            try
            {
                value = (T)defaultValue.Value;
            }
            catch (InvalidCastException ex)
            {
                return false;
            }

            return true;
        }
    }
}
