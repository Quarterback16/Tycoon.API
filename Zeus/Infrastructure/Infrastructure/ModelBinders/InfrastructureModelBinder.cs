using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Helpers;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using Employment.Web.Mvc.Infrastructure.ViewModels.Dynamic;
using Employment.Web.Mvc.Infrastructure.ViewModels.Geospatial;
using BindableAttribute = Employment.Web.Mvc.Infrastructure.DataAnnotations.BindableAttribute;
using Employment.Web.Mvc.Infrastructure.ViewModels.JobSeeker;
#if DEBUG
using StackExchange.Profiling;
#endif

namespace Employment.Web.Mvc.Infrastructure.ModelBinders
{
    /// <summary>
    /// Represents a custom Model Binder that maps a request to a data object. 
    /// </summary>
    public class InfrastructureModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// Binds the specified property by using the specified controller context and binding context and the specified property descriptor.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <param name="propertyDescriptor">Describes a property to be bound. The descriptor provides information such as the component type, property type, and property value. It also provides methods to get or set the property value.</param>
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
     #if DEBUG
            var step = MiniProfiler.Current.Step("InfrastructureModelBinder.BindProperty");

            try
            {
#endif
           var bindable = false;

            // Get the Bindable attribute
            var bindableAttribute = propertyDescriptor.Attributes.OfType<BindableAttribute>().FirstOrDefault();

            if (bindableAttribute != null && bindableAttribute.IsBindable())
            {
                bindable = true;
            }
            else
            {
                var bindableIfAttributes = propertyDescriptor.Attributes.OfType<ContingentAttribute>().Where(a => a.GetType().Name.StartsWith("BindableIf",StringComparison.Ordinal));

                var contingentAttributes = bindableIfAttributes as IList<ContingentAttribute> ?? bindableIfAttributes.ToList();
                if (contingentAttributes.Any())
                {
                    string propertyName = CreateSubPropertyName(bindingContext.ModelName, propertyDescriptor.Name);

                    if (!bindingContext.ValueProvider.ContainsPrefix(propertyName))
                    {
                        return;
                    }

                    object model = CreateModel(controllerContext, bindingContext, bindingContext.ModelType);

                    IModelBinder propertyBinder = Binders.GetBinder(propertyDescriptor.PropertyType);
                    ModelMetadata propertyModelMetadata = bindingContext.PropertyMetadata[propertyDescriptor.Name];

                    var propertyBindingContext = new ModelBindingContext
                    {
                        ModelMetadata = propertyModelMetadata,
                        ModelName = propertyName,
                        ModelState = bindingContext.ModelState,
                        ValueProvider = bindingContext.ValueProvider
                    };

                    object propertyValue = GetPropertyValue(controllerContext, propertyBindingContext, propertyDescriptor, propertyBinder);
                    propertyDescriptor.SetValue(model, propertyValue);
                    
                    foreach (var bindableIfAttribute in contingentAttributes)
                    {
                        var dependentPropertyDescriptor = TypeDescriptor.GetProperties(bindingContext.Model).Cast<PropertyDescriptor>().FirstOrDefault(p => p.Name == bindableIfAttribute.DependentProperty);
                        string dependentPropertyName = CreateSubPropertyName(bindingContext.ModelName, bindableIfAttribute.DependentProperty);

                        if (dependentPropertyDescriptor == null || !bindingContext.ValueProvider.ContainsPrefix(dependentPropertyName))
                        {
                            continue;
                        }

                        IModelBinder dependentPropertyBinder = Binders.GetBinder(dependentPropertyDescriptor.PropertyType);
                        var dependentPropertyModelMetadata = bindingContext.PropertyMetadata[bindableIfAttribute.DependentProperty];

                        var dependentPropertyBindingContext = new ModelBindingContext
                        {
                            ModelMetadata = dependentPropertyModelMetadata,
                            ModelName = dependentPropertyName,
                            ModelState = bindingContext.ModelState,
                            ValueProvider = bindingContext.ValueProvider
                        };

                        object dependentPropertyValue = GetPropertyValue(controllerContext, dependentPropertyBindingContext, propertyDescriptor, dependentPropertyBinder);
                        dependentPropertyDescriptor.SetValue(model, dependentPropertyValue);

                        if (bindableIfAttribute.IsConditionMet(propertyDescriptor.Name, propertyValue, model))
                        {
                            bindable = true;
                            break;
                        }
                    }
                }
            }

            // Bind property only if it is bindable
            if (bindable)
            {
                base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
            }
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
        
        /// <summary>Binds the model by using the specified controller context and binding context.</summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>The bound object.</returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="bindingContext" />parameter is null.</exception>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
      #if DEBUG
            var step = MiniProfiler.Current.Step("InfrastructureModelBinder.BindModel");

            try
            {
#endif
           if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }

            var serialized = bindingContext.ModelMetadata.GetAttribute<SerializedAttribute>();

            if (serialized != null)
            {
                byte[] model = new ByteArrayModelBinder().BindModel(controllerContext, bindingContext) as byte[];

                if (model == null)
                {
                    return null;
                }

                return model.Deserialize();
            }

            // Allow binding for currency values
            if (bindingContext.ModelMetadata.DataTypeName == DataType.Currency.ToString())
            {
                string attemptedValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).AttemptedValue;

                if (bindingContext.ModelType == typeof(double) || bindingContext.ModelType == typeof(double?))
                {
                    double d;
                    double.TryParse(attemptedValue, NumberStyles.Any, CultureInfo.CurrentCulture, out d);
                    return d;
                }
                
                if (bindingContext.ModelType == typeof(decimal) || bindingContext.ModelType == typeof(decimal?))
                {
                    decimal d;
                    decimal.TryParse(attemptedValue, NumberStyles.Any, CultureInfo.CurrentCulture, out d);
                    return d;
                }

                if (bindingContext.ModelType == typeof(float) || bindingContext.ModelType == typeof(float?))
                {
                    float f;
                    float.TryParse(attemptedValue, NumberStyles.Any, CultureInfo.CurrentCulture, out f);
                    return f;
                }
            }

            if (bindingContext.ModelType == typeof(SelectList))
            {
                return BindSelectList(controllerContext, bindingContext);
            }

            if (bindingContext.ModelType == typeof(MultiSelectList))
            {
                return BindMultiSelectList(controllerContext, bindingContext);
            }

            if (bindingContext.ModelType == typeof(IEnumerable<SelectListItem>))
            {
                return BindEnumerableSelectListItem(controllerContext, bindingContext);
            }

            if (bindingContext.ModelType == typeof(IEnumerable<string>))
            {
                return BindEnumerableString(controllerContext, bindingContext);
            }

            if (bindingContext.ModelType.IsGenericType && new[] { typeof(IPageable<>), typeof(Pageable<>) }.Contains(bindingContext.ModelType.GetGenericTypeDefinition()))
            {
                return BindPageable(controllerContext, bindingContext);
            }

            if (bindingContext.ModelMetadata.DataTypeName == CustomDataType.Grid || bindingContext.ModelMetadata.DataTypeName == CustomDataType.GridEditable)
            {
                return BindGrid(controllerContext, bindingContext);
            }

            if (bindingContext.ModelType == typeof(DynamicViewModel))
            {
                return BindDynamicViewModel(controllerContext, bindingContext);
            }

            if (bindingContext.ModelType == typeof(AddressViewModel))
            {
                return BindAddressViewModel(controllerContext, bindingContext);
            }

            if(bindingContext.ModelType == typeof(JobseekerSearchViewModel))
            {
                return BindJobSeekerSearchViewModel(controllerContext, bindingContext);
            }

            if (bindingContext.ModelType == typeof(DateTime) || bindingContext.ModelType == typeof(DateTime?))
            {
                return BindDateTime(controllerContext, bindingContext);
            }

            return base.BindModel(controllerContext, bindingContext);
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
        /// Bind date time value ensuring it meets the allowed format (values from URL do not need to match the format but are corrected).
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>A date time.</returns>
        public object BindDateTime(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = base.BindModel(controllerContext, bindingContext);

            string dateString = null;
            var validationResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (validationResult != null)
            {
                dateString = validationResult.AttemptedValue;
            }

            // Get data type, defaulting to DateTime
            var dataType = DataType.DateTime;
            var dataTypeAttribute = bindingContext.ModelMetadata.GetAttribute<DataTypeAttribute>();

            if (dataTypeAttribute != null && (dataTypeAttribute.DataType == DataType.Date || dataTypeAttribute.DataType == DataType.Time))
            {
                dataType = dataTypeAttribute.DataType;
            }

            // Get format string
            string format;

            switch (dataType)
            {
                case DataType.Date: format = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern; break;
                case DataType.Time: format = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern; break;
                default: format = string.Format("{0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern); break;
            }

            // Skip format check if the value is being binded from the URL or it is empty or is from an Ajax request
            if (string.IsNullOrEmpty(dateString) || 
                // Binded from Query String
                (controllerContext.HttpContext.Request.QueryString[bindingContext.ModelName] != null) ||
                // Binded from Route
                (controllerContext.RouteData.Values[bindingContext.ModelName] != null) ||
                // Is an Ajax request
                controllerContext.HttpContext.Request.IsAjaxRequest())
            {
                if (bindingContext.ModelState[bindingContext.ModelName] != null)
                {
                    // Ignore errors for URL
                    if (!controllerContext.HttpContext.Request.IsAjaxRequest())
                    {
                        bindingContext.ModelState[bindingContext.ModelName].Errors.Clear();
                    }

                    // Correct ModelState to use allowed date format
                    var attemptedValue = value != null ? ((DateTime)value).ToString(format) : string.Empty;
                    bindingContext.ModelState[bindingContext.ModelName].Value = new ValueProviderResult(value, attemptedValue, CultureInfo.CurrentCulture);
                }

                return value;
            }

            // Check if supplied value is in the correct format and add an error if it is not
            DateTime dateTime;
            if (!DateTime.TryParseExact(dateString, format, CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTime))
            {
                // Remove default format exception error if there is one (to prevent duplicate errors about format)
                var error = bindingContext.ModelState[bindingContext.ModelName].Errors.FirstOrDefault(e => e.Exception != null && e.Exception.InnerException != null && e.Exception.InnerException.GetType() == typeof(FormatException));

                if (error != null)
                {
                    bindingContext.ModelState[bindingContext.ModelName].Errors.Remove(error);
                }

                // Include custom error for invalid format
                var dataTypeDescription = dataType == DataType.DateTime ? "date and time" : dataType.ToString().ToLower();

                bindingContext.ModelState.AddModelError(bindingContext.ModelName, string.Format("The field {0} must be a valid {1}.", bindingContext.ModelMetadata.GetDisplayName(), dataTypeDescription));
            }
            
            return value;
        }

        /// <summary>
        /// Bind select list.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>A <see cref="SelectList" /> binded with its corresponding values from the binding context value provider.</returns>
        public object BindSelectList(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var items = BindEnumerableSelectListItem(controllerContext, bindingContext) as IEnumerable<SelectListItem>;

            return items != null ? new SelectList(items, "Value", "Text", GetSelectorValue(bindingContext).FirstOrDefault()) : null;
        }

        /// <summary>
        /// Bind multi select list.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>A <see cref="MultiSelectList" /> binded with its corresponding values from the binding context value provider.</returns>
        public object BindMultiSelectList(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var items = BindEnumerableSelectListItem(controllerContext, bindingContext) as IEnumerable<SelectListItem>;

            return items != null ? new MultiSelectList(items, "Value", "Text", GetSelectorValue(bindingContext)) : null;
        }

        /// <summary>
        /// Bind enumerable strings, for use when constructing the model out of a multi select list.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>A <see cref="IEnumerable{string}" /> binded with its corresponding values from the binding context value provider.</returns>
        public object BindEnumerableString(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = new List<string>();
            var selectorValue = GetSelectorValue(bindingContext);

            int count = 0;
            string indexName = CreateSubIndexName(bindingContext.ModelName, count);

            if (bindingContext.ValueProvider.ContainsPrefix(indexName))
            {
                // Continue while value provider has the corresponding index name
                while (bindingContext.ValueProvider.ContainsPrefix(indexName))
                {
                    ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(string.Format("{0}.Value", indexName));
                    if (valueResult != null)
                    {
                        string item = valueResult.AttemptedValue;
                        if (selectorValue.Contains(item))
                        {
                            model.Add(item);
                        }
                    }

                    // Get next index name
                    indexName = CreateSubIndexName(bindingContext.ModelName, ++count);
                }
            }
            else
            {
                // Handle [Selection(SelectionType.Multiple)] and [Check Box List for [AdwSelection]
                foreach (var sV in selectorValue)
                {
                    var selected = sV as string;

                    if (!string.IsNullOrWhiteSpace(selected))
                    {
                        model.Add(selected);
                    }
                }
            }

            return model.Count > 0 ? model : null;
        }

        /// <summary>
        /// Bind enumerable select list item.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>A <see cref="IEnumerable{SelectListItem}" /> binded with its corresponding values from the binding context value provider.</returns>
        public object BindEnumerableSelectListItem(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = new List<SelectListItem>();
            var selectorValue = GetSelectorValue(bindingContext);

            int count = 0;
            string indexName = CreateSubIndexName(bindingContext.ModelName, count);

            // Continue while value provider has the corresponding index name
            while (bindingContext.ValueProvider.ContainsPrefix(indexName))
            {
                var item = new SelectListItem();

                ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(string.Format("{0}.Value", indexName));

                if (valueResult != null)
                {
                    item.Value = valueResult.AttemptedValue;
                    item.Selected = selectorValue.Contains(item.Value);
                }

                ValueProviderResult textResult = bindingContext.ValueProvider.GetValue(string.Format("{0}.Text", indexName));

                if (textResult != null)
                {
                    item.Text = textResult.AttemptedValue;
                }

                model.Add(item);

                // Get next index name
                indexName = CreateSubIndexName(bindingContext.ModelName, ++count);
            }

            return model.Count > 0 ? model : null;
        }

        private static IEnumerable<object> GetSelectorValue(ModelBindingContext bindingContext)
        {
            List<object> value = new List<object>();

            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueResult != null && valueResult.RawValue != null)
            {
                if (valueResult.RawValue.GetType().IsArray)
                {
                    object[] objArray = valueResult.RawValue as object[];

                    if (objArray != null)
                    {
                        // Correct for comma separated lists
                        if (objArray.Length == 1 && objArray[0] is string)
                        {
                            objArray = ((string)objArray[0]).Split(',');
                        }

                        foreach (var obj in objArray)
                        {
                            value.Add(obj);
                        }
                    }
                }
                else
                {
                    value.Add(valueResult.RawValue);
                }
            }

            return value.AsEnumerable();
        }

        /// <summary>
        /// Bind Pageable collection.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>A <see cref="IPageable{T}" /> binded with its corresponding values from the binding context value provider.</returns>
        public object BindPageable(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
        #if DEBUG
            var step = MiniProfiler.Current.Step("InfrastructureModelBinder.BindPageable");

            try
            {
#endif
          var elementType = bindingContext.ModelType.HasElementType ? bindingContext.ModelType.GetElementType() : bindingContext.ModelType.GetGenericArguments().FirstOrDefault();

            if (elementType == null)
            {
                return null;
            }

            PageMetadata metadata = null;

            var valueResult = bindingContext.ValueProvider.GetValue(string.Format("{0}.Metadata", bindingContext.ModelName));

            if (valueResult != null)
            {
                metadata = valueResult.AttemptedValue.Deserialize<PageMetadata>();
            }

            var pageableAccessor = (IPageable)DelegateHelper.CreateConstructorDelegate(typeof(Pageable<>).MakeGenericType(new[] { elementType }))();
            var pageable = pageableAccessor;
            var elementInstance = DelegateHelper.CreateConstructorDelegate(elementType)();
            var elementMetadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForType(() => elementInstance, elementType);

            if (elementMetadata.GetAttribute<SerializableAttribute>() != null)
            {
                BindSerializedGrid(controllerContext, bindingContext, pageable);
            }
            else
            {
                var pageableBindingContext = new ModelBindingContext
                {
                    ModelMetadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForType(() => pageableAccessor, pageableAccessor.GetType()),
                    ModelName = bindingContext.ModelName,
                    ModelState = bindingContext.ModelState,
                    PropertyFilter = bindingContext.PropertyFilter,
                    ValueProvider = bindingContext.ValueProvider
                };

                pageable = (IPageable)base.BindModel(controllerContext, pageableBindingContext);
            }

            if (pageable != null)
            {
                pageable.Metadata = metadata;
            }

            return pageable;
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
        /// Bind grid collection.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>A <see cref="IEnumerable{T}" /> binded with its corresponding values from the binding context value provider.</returns>
        public object BindGrid(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
       #if DEBUG
            var step = MiniProfiler.Current.Step("InfrastructureModelBinder.BindGrid");

            try
            {
#endif
           var elementType = bindingContext.ModelType.HasElementType ? bindingContext.ModelType.GetElementType() : bindingContext.ModelType.GetGenericArguments().FirstOrDefault();

            if (elementType == null)
            {
                return null;
            }

            var elementInstance = DelegateHelper.CreateConstructorDelegate(elementType)();
            var elementMetadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForType(() => elementInstance, elementType);

            if (elementMetadata.GetAttribute<SerializableAttribute>() != null)
            {
                var listAccessor = (IList)DelegateHelper.CreateConstructorDelegate(typeof(List<>).MakeGenericType(new[] { elementType }))();

                return BindSerializedGrid(controllerContext, bindingContext, listAccessor);
            }

            return base.BindModel(controllerContext, bindingContext);
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

        private IList BindSerializedGrid(ControllerContext controllerContext, ModelBindingContext bindingContext, IList list)
        {
        #if DEBUG
            var step = MiniProfiler.Current.Step("InfrastructureModelBinder.BindSerializedGrid");

            try
            {
#endif           
            int count = 0;
            string indexName = CreateSubIndexName(bindingContext.ModelName, count);

            var selector = string.Empty;
            var key = string.Empty;

            // Get element type in enumerable
            var elementType = bindingContext.ModelType.HasElementType ? bindingContext.ModelType.GetElementType() : bindingContext.ModelType.GetGenericArguments().FirstOrDefault();
            
            if (elementType != null)
            {
                var properties = TypeDescriptor.GetProperties(elementType).Cast<PropertyDescriptor>();

                // Check for a [Selector] property
                var selectorProperty = properties.FirstOrDefault(p => p.Attributes.OfType<SelectorAttribute>().Any());

                if (selectorProperty != null)
                {
                    // [Selector] found so this grid allows multiple selection
                    selector = selectorProperty.Name;
                }

                var keyProperty = properties.FirstOrDefault(p => p.Attributes.OfType<KeyAttribute>().Any());

                if (keyProperty != null)
                {
                    // [Key] found
                    key = keyProperty.Name;
                }
            }

            // Loop through each grid row
            while (bindingContext.ValueProvider.ContainsPrefix(indexName))
            {
                // Get serialized row value
                var valueResult = bindingContext.ValueProvider.GetValue(indexName);

                if (valueResult != null && valueResult.RawValue != null)
                {
                    string serializedValue = null;

                    if (valueResult.RawValue.GetType().IsArray)
                    {
                        object[] objArray = valueResult.RawValue as object[];

                        if (objArray != null)
                        {
                            serializedValue = objArray.FirstOrDefault() as string;
                        }
                    }
                    else
                    {
                        serializedValue = valueResult.RawValue.ToString();
                    }

                    if (!string.IsNullOrEmpty(serializedValue))
                    {
                        // Deserialize row value to its row view model
                        var deserializedValue = serializedValue.Deserialize();

                        if (deserializedValue != null)
                        {
                            var properties = TypeDescriptor.GetProperties(deserializedValue).Cast<PropertyDescriptor>();
                            var deserializedValues = new Dictionary<string, object>();
                            var bindedDeserializedValue = DelegateHelper.CreateConstructorDelegate(deserializedValue.GetType())();
                            
                            // Loop through each property in the current row
                            foreach (var property in properties)
                            {
                                string propertyName =  indexName+ "." + property.Name;

                                object propertyValue = null;

                                // Don't get deserialized value for [Selector] property
                                if (!string.IsNullOrEmpty(selector) && string.Equals(selector, property.Name, StringComparison.Ordinal))
                                {
                                    // Instead read its value from the value provider
                                    var selectorValue = bindingContext.ValueProvider.GetValue(propertyName);

                                    if (selectorValue != null)
                                    {
                                        propertyValue = selectorValue.RawValue;
                                    }
                                }
                                else if (!string.IsNullOrEmpty(key) && string.Equals(key , property.Name,StringComparison.Ordinal))
                                {
                                    // Get deserialized value
                                    propertyValue = property.GetValue(deserializedValue);
                                }
                                else
                                {
                                    // Ignore all other properties
                                    continue;
                                }

                                // Add its deserialized value to the collection of deserialized values (if not already added)
                                if (!deserializedValues.ContainsKey(propertyName))
                                {
                                    deserializedValues.Add(propertyName, propertyValue);
                                }

                                // Prepare new binding context for current property using the deserialized values
                                var propertyBindingContext = new ModelBindingContext
                                {
                                    ModelMetadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperty(() => deserializedValue, deserializedValue.GetType(), property.Name),
                                    ModelName = propertyName,
                                    ModelState = bindingContext.ModelState,
                                    PropertyFilter = bindingContext.PropertyFilter,
                                    ValueProvider = new DictionaryValueProvider<object>(deserializedValues, CultureInfo.InvariantCulture)
                                };

                                // Get binded value
                                var bindedPropertyValue = property.PropertyType.GetNonNullableType() == typeof(DateTime) ? base.BindModel(controllerContext, propertyBindingContext) : BindModel(controllerContext, propertyBindingContext);
                                var bindable = propertyBindingContext.ModelMetadata.GetAttribute<BindableAttribute>();
                                
                                // Only bind if bindable
                                if (bindable != null && bindable.IsBindable())
                                {
                                    property.SetValue(bindedDeserializedValue, bindedPropertyValue);
                                }
                            }
                            
                            // Add binding for row
                            list.Add(bindedDeserializedValue);
                        }
                    }
                }

                // Get next index name
                indexName = CreateSubIndexName(bindingContext.ModelName, ++count);
            }

            return list;

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
        /// Bind a <see cref="DynamicViewModel" />.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>A <see cref="DynamicViewModel" /> binded with its corresponding values from the binding context value provider.</returns>
        public object BindDynamicViewModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
        #if DEBUG
            var step = MiniProfiler.Current.Step("InfrastructureModelBinder.BindDynamicViewModel");

            try
            {
#endif
  

           byte[] serializedModel = new ByteArrayModelBinder().BindModel(controllerContext, bindingContext) as byte[];

            if (serializedModel == null)
            {
                return null;
            }

            // Deserialize to retrieve dynamic view model without user supplied values
            var model = serializedModel.Deserialize() as DynamicViewModel;
            
            int count = 0;
            string indexName = CreateSubIndexName(bindingContext.ModelName, count);

            if (model != null)
            {
                var updateIndex = new Dictionary<int, object>();

                int valueIndex = 0;
                foreach (var value in model.Values)
                {
                    if (value is LabelViewModel)
                    {
                        valueIndex++;
                        continue;
                    }

                    var properties = TypeDescriptor.GetProperties(value).Cast<PropertyDescriptor>();

                    // Loop through each property in the current row
                    foreach (var property in properties)
                    {
                        if (property.Name != "Value")
                        {
                            continue;
                        }

                        var propertyName = string.Format("{0}.Value", indexName);

                        // Prepare new binding context for current property
                        var propertyBindingContext = new ModelBindingContext
                        {
                            ModelMetadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperty(() => value, value.GetType(), property.Name),
                            ModelName = propertyName,
                            ModelState = bindingContext.ModelState,
                            PropertyFilter = bindingContext.PropertyFilter,
                            ValueProvider = bindingContext.ValueProvider
                        };

                        // Get binded value
                        var bindedPropertyValue = BindModel(controllerContext, propertyBindingContext);
                        var bindable = propertyBindingContext.ModelMetadata.GetAttribute<BindableAttribute>();

                        // Only bind if bindable
                        if (bindable != null && bindable.IsBindable())
                        {
                            property.SetValue(value, bindedPropertyValue);

                            updateIndex.Add(valueIndex, value);
                        }

                        // Get next index name
                        indexName = CreateSubIndexName(bindingContext.ModelName, ++count);
                    }
                    
                    valueIndex++;
                }

                foreach (var update in updateIndex)
                {
                    model.Values[update.Key] = update.Value;
                }
            }

            return model;


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
        /// Bind a <see cref="AddressViewModel" />.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>A <see cref="AddressViewModel" /> binded with its corresponding values from the binding context value provider.</returns>
        public object BindAddressViewModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // Start with normal binding
            var model = base.BindModel(controllerContext, bindingContext) as AddressViewModel;
            
            // Clear all errors except the SingleLineAddress error if address is required and SingleLineAddress is the only visible property
            if (model != null)
            {
                if (model.Required && !model.CustomAddress)
                {
                    foreach (var property in bindingContext.PropertyMetadata)
                    {
                        if (property.Key == "SingleLineAddress")
                        {
                            continue;
                        }

                        var subPropertyName = CreateSubPropertyName(bindingContext.ModelName, property.Key);

                        if (bindingContext.ModelState.ContainsKey(subPropertyName))
                        {
                            bindingContext.ModelState[subPropertyName].Errors.Clear();
                        }
                    }
                }

                var customAddress = CreateSubPropertyName(bindingContext.ModelName, "CustomAddress");

                // Hide custom address group if the address is valid, since it will be shown in the SingleLineAddress
                if (bindingContext.ModelState.IsValid && bindingContext.ModelState.ContainsKey(customAddress))
                {
                    bindingContext.ModelState[customAddress].Value = new ValueProviderResult(false, false.ToString(), CultureInfo.CurrentCulture);
                    model.CustomAddress = false;
                } 
            }

            return model;
        }

        /// <summary>
        /// Bind a <see cref="JobseekerSearchViewModel"/>.
        /// </summary>
        /// <param name="controllerContext">The context within which a controller operates. the context information includes the controller, Http Content, request context and route data.</param>
        /// <param name="bindingContext">The context within which a model is bound. This information includes model object, model name, model type, property filter and value provider.</param>
        /// <returns><see cref="JobseekerSearchViewModel"/> bound with its corresponding values from the binding context value provider.</returns>
        public object BindJobSeekerSearchViewModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // Let's start with normal model binding.
            var model = base.BindModel(controllerContext, bindingContext) as JobseekerSearchViewModel;

            // Clear all errors except the single line search error if jsk is required and SingleLineSearch is the only visible property.
            if(model != null)
            {
                if(model.Required)
                {
                    foreach(var property in bindingContext.PropertyMetadata)
                    {
                        if(property.Key == JobseekerSearchViewModel.AjaxProperty)
                        {
                            continue;
                        }
                        var subPropertyName = CreateSubPropertyName(bindingContext.ModelName, property.Key);
                        if (bindingContext.ModelState.ContainsKey(subPropertyName))
                        {
                            bindingContext.ModelState[subPropertyName].Errors.Clear();
                        }

                    }
                }

                // Do not bind to custom JSk Model.

            }

            return model;
        }
    }
}
