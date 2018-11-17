using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is aware of the model the property is contained within as a condition is dependent on the value of a dependent property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ContingentAttribute : Attribute, IMetadataAware
    {
        /// <summary>
        /// Type of action to perform based on <see cref="DependentProperty" /> meeting the comparison condition.
        /// </summary>
        public ActionForDependencyType? DependencyType { get; protected set; }

        /// <summary>
        /// Other property in the model that this property is dependent on.
        /// </summary>
        public string DependentProperty { get; protected set; }

        /// <summary>
        /// The comparison type to use.
        /// </summary>
        public ComparisonType ComparisonType { get; protected set; }

        /// <summary>
        /// The value to compare against.
        /// </summary>
        public object DependentValue { get; private set; }

        /// <summary>
        /// Whether to always pass validation on a null dependent value.
        /// </summary>
        public bool PassOnNull { get; set; }

        /// <summary>
        /// Whether to always fail validation on a null dependent value.
        /// </summary>
        public bool FailOnNull { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContingentAttribute" /> class.
        /// </summary>
        protected ContingentAttribute() : this(ActionForDependencyType.None, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContingentAttribute" /> class.
        /// </summary>
        /// <param name="dependencyType">The type of action to take if the dependency condition is met.</param>
        protected ContingentAttribute(ActionForDependencyType dependencyType) : this(dependencyType, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContingentAttribute" /> class.
        /// </summary>
        /// <param name="dependencyType">The type of action to take if the dependency condition is met.</param>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        protected ContingentAttribute(ActionForDependencyType dependencyType, string dependentProperty) : this(dependencyType, dependentProperty, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContingentAttribute" /> class.
        /// </summary>
        /// <param name="dependencyType">The type of action to take if the dependency condition is met.</param>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        protected ContingentAttribute(ActionForDependencyType dependencyType, string dependentProperty, object dependentValue) : this(dependencyType, dependentProperty, ComparisonType.EqualTo, dependentValue) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContingentAttribute" /> class.
        /// </summary>
        /// <param name="dependencyType">The type of action to take if the dependency condition is met.</param>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="comparisonType">The comparison type to use.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        protected ContingentAttribute(ActionForDependencyType dependencyType, string dependentProperty, ComparisonType comparisonType, object dependentValue)
        {
            DependencyType = dependencyType;
            DependentProperty = dependentProperty;
            ComparisonType = comparisonType;
            DependentValue = dependentValue;
        }

        /// <summary>
        /// Get the value of the dependent property from the container.
        /// </summary>
        /// <param name="container">The object containing the dependent property.</param>
        /// <returns>The value of the dependent property.</returns>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="DependentProperty" /> doesn't exist in container.</exception>
        protected object GetDependentPropertyValue(object container)
        {
            if (container == null)
            {
                throw new InvalidOperationException("Container of DependentProperty is null.");
            }

            var property = container.GetType().GetProperty(DependentProperty);

            if (property != null)
            {
                return property.GetValue(container, null);
            }

            throw new InvalidOperationException("DependentProperty does not exist within the object.");
        }

        /// <summary>
        /// Determines whether the specified value of the object meets the condition.
        /// </summary>
        /// <remarks>
        /// Used only by attributes that are expected to have <see cref="DependentProperty" /> set, such as those at the class level.
        /// </remarks>
        /// <param name="container">The model this object is contained within.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
        public bool IsConditionMet(object container)
        {
            return IsConditionMet(string.Empty, null, container);
        }

        /// <summary>
        /// Determines whether the specified value of the object meets the condition.
        /// </summary>
        /// <remarks>
        /// Used only by attributes at the property level.
        /// </remarks>
        /// <param name="property">The name of the property decorated with this attribute.</param>
        /// <param name="propertyValue">The value of the property decorated with this attribute.</param>
        /// <param name="container">The model this object is contained within.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
        public bool IsConditionMet(string property, object propertyValue, object container)
        {
            // Assume valid if there is no action for dependency type or no property or dependent property to compare against
            if (DependencyType == ActionForDependencyType.None || string.IsNullOrEmpty(DependentProperty) && string.IsNullOrEmpty(property))
            {
                return true;
            }

            // Use property if it is supplied and no dependent property is set
            if (string.IsNullOrEmpty(DependentProperty) && !string.IsNullOrEmpty(property))
            {
                DependentProperty = property;
            }

            // Assume invalid if container is null
            if (container == null)
            {
                return false;
            }

            return IsConditionMet(propertyValue, GetDependentPropertyValue(container), container);
        }

        /// <summary>
        /// Determines whether the specified value of the object meets the condition.
        /// </summary>
        /// <param name="propertyValue">The value of the property decorated with this attribute.</param>
        /// <param name="dependentPropertyValue">The value of the dependent property.</param>
        /// <param name="container">The model this object is contained within.</param>
        /// <returns><c>true</c> if the specified value is valid; otherwise, <c>false</c>.</returns>
        protected virtual bool IsConditionMet(object propertyValue, object dependentPropertyValue, object container)
        {
            dependentPropertyValue = HandleEnumerableSelectListItem(dependentPropertyValue);

            if (PassOnNull && dependentPropertyValue == null)
            {
                return true;
            }

            if (FailOnNull && dependentPropertyValue == null)
            {
                return false;
            }

            var valuesToTestAgainst = DependentValue as object[] ?? new[] {DependentValue};
            var actualValues = dependentPropertyValue as object[] ?? new[] {dependentPropertyValue};

            var result = new List<bool>();


            foreach (var dependentValue in valuesToTestAgainst)
            {
                foreach (var actualValue in actualValues)
                {
                    result.Add(ComparisonType.Compare(actualValue, dependentValue));
                }
            }
            if (ComparisonType == ComparisonType.NotEqualTo)
            {
                // Negative AND validation (all must be true)
                return (!result.Contains(false));
            }
            else
            {
                // Positive OR validation (at least one must be true)
                return (result.Contains(true));
            }


            
        }

        /// <summary>
        /// Handles the case where <paramref name="dependentPropertyValue" /> is a <see cref="IEnumerable{SelectListItem}" /> by converting it return the value of the selected item(s) as the value or array of values.
        /// </summary>
        /// <param name="dependentPropertyValue">The value of the dependent property.</param>
        /// <returns>The value of the dependent property.</returns>
        protected object HandleEnumerableSelectListItem(object dependentPropertyValue)
        {
            // Handle IEnumerable<SelectListItem>
            var selectList = dependentPropertyValue as IEnumerable<SelectListItem>;

            if (selectList != null)
            {
                // Set dependentPropertyValue to selected item value
                var selections = selectList.Where(p => p.Selected).Select(p => p.Value).ToList();

                if (selections.Count > 1)
                {
                    dependentPropertyValue = selections.ToArray();
                }
                else
                {
                    dependentPropertyValue = selections.FirstOrDefault();
                }
            }

            return dependentPropertyValue;
        }

        /// <summary>
        /// On metadata created.
        /// </summary>
        /// <param name="metadata">Metadata.</param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            var attributeName = GetType().Name;

            // Ignore these contingent types
            if (attributeName.StartsWith("Link", StringComparison.Ordinal) || attributeName.StartsWith("ExternalLink", StringComparison.Ordinal) || attributeName.StartsWith("Button", StringComparison.Ordinal) || attributeName.StartsWith("Group", StringComparison.Ordinal))
            {
                return;
            }

            var propertyName = metadata.PropertyName;
            var parentModel = metadata.AdditionalValues.ContainsKey("ParentModel") ? metadata.AdditionalValues["ParentModel"] : null;

            if (parentModel == null)
            {
                return;
            }

            var propertyValue = GetPropertyValue(parentModel, propertyName);
            var conditionMet = IsConditionMet(propertyName, propertyValue, parentModel);

            // If no dependent property is specified, use own property name (self-referencing)
            var dependentProperty = !string.IsNullOrEmpty(DependentProperty) ? DependentProperty : propertyName;

            // VisibleIf attributes
            if (attributeName.StartsWith("VisibleIf", StringComparison.Ordinal))
            {
                var htmlAttributes = new Dictionary<string, object>();

                htmlAttributes.Add(HtmlDataType.DependentPropertyVisibleIf, dependentProperty);
                htmlAttributes.Add(HtmlDataType.ComparisonTypeVisibleIf, ComparisonType);
                htmlAttributes.Add(HtmlDataType.PassOnNullVisibleIf, PassOnNull.ToString().ToLowerInvariant());
                htmlAttributes.Add(HtmlDataType.FailOnNullVisibleIf, FailOnNull.ToString().ToLowerInvariant());

                var values = DependentValue as object[];

                if (values != null)
                {
                    htmlAttributes.Add(HtmlDataType.DependentValueVisibleIf, string.Format("[\"{0}\"]", string.Join("\",\"", values)));
                }
                else
                {
                    htmlAttributes.Add(HtmlDataType.DependentValueVisibleIf, DependentValue);
                }

                metadata.AdditionalValues.Add("visibleif", htmlAttributes);
            }
            // ReadOnlyIf attributes
            else if (attributeName.StartsWith("ReadOnlyIf", StringComparison.Ordinal))
            {
                if (conditionMet)
                {
                    metadata.IsReadOnly = true;
                }

                var htmlAttributes = new Dictionary<string, object>();

                htmlAttributes.Add(HtmlDataType.DependentPropertyReadOnlyIf, dependentProperty);
                htmlAttributes.Add(HtmlDataType.ComparisonTypeReadOnlyIf, ComparisonType);
                htmlAttributes.Add(HtmlDataType.PassOnNullReadOnlyIf, PassOnNull.ToString().ToLowerInvariant());
                htmlAttributes.Add(HtmlDataType.FailOnNullReadOnlyIf, FailOnNull.ToString().ToLowerInvariant());

                var values = DependentValue as object[];

                if (values != null)
                {
                    htmlAttributes.Add(HtmlDataType.DependentValueReadOnlyIf, string.Format("[\"{0}\"]", string.Join("\",\"", values)));
                }
                else
                {
                    htmlAttributes.Add(HtmlDataType.DependentValueReadOnlyIf, DependentValue);
                }

                metadata.AdditionalValues.Add("readonlyif", htmlAttributes);
            }
            // EditableIf attributes
            else if (attributeName.StartsWith("EditableIf", StringComparison.Ordinal))
            {
                if (!conditionMet)
                {
                    metadata.IsReadOnly = true;
                }

                var htmlAttributes = new Dictionary<string, object>();

                htmlAttributes.Add(HtmlDataType.DependentPropertyEditableIf, dependentProperty);
                htmlAttributes.Add(HtmlDataType.ComparisonTypeEditableIf, ComparisonType);
                htmlAttributes.Add(HtmlDataType.PassOnNullEditableIf, PassOnNull.ToString().ToLowerInvariant());
                htmlAttributes.Add(HtmlDataType.FailOnNullEditableIf, FailOnNull.ToString().ToLowerInvariant());

                var values = DependentValue as object[];

                if (values != null)
                {
                    htmlAttributes.Add(HtmlDataType.DependentValueEditableIf, string.Format("[\"{0}\"]", string.Join("\",\"", values)));
                }
                else
                {
                    htmlAttributes.Add(HtmlDataType.DependentValueEditableIf, DependentValue);
                }

                metadata.AdditionalValues.Add("editableif", htmlAttributes);
            }
            // Clear attributes
            else if (attributeName.StartsWith("Clear", StringComparison.Ordinal))
            {
                var htmlAttributes = new Dictionary<string, object>();

                htmlAttributes.Add(HtmlDataType.DependentPropertyClearIf, dependentProperty);
                htmlAttributes.Add(HtmlDataType.ComparisonTypeClearIf, ComparisonType);
                htmlAttributes.Add(HtmlDataType.PassOnNullClearIf, PassOnNull.ToString().ToLowerInvariant());
                htmlAttributes.Add(HtmlDataType.FailOnNullClearIf, FailOnNull.ToString().ToLowerInvariant());
                htmlAttributes.Add(HtmlDataType.AlwaysClearIf, GetType() == typeof(ClearAttribute));

                var values = DependentValue as object[];

                if (values != null)
                {
                    htmlAttributes.Add(HtmlDataType.DependentValueClearIf, string.Format("[\"{0}\"]", string.Join("\",\"", values)));
                }
                else
                {
                    htmlAttributes.Add(HtmlDataType.DependentValueClearIf, DependentValue);
                }

                metadata.AdditionalValues.Add("clearif", htmlAttributes);
            }
            // Gst attribute
            else if (attributeName.StartsWith("Gst", StringComparison.Ordinal))
            {
                var gst = this as GstAttribute;
                var htmlAttributes = new Dictionary<string, object>();

                htmlAttributes.Add(HtmlDataType.DependentPropertyGst, dependentProperty);
                htmlAttributes.Add(HtmlDataType.ExclusiveGst, gst != null ? gst.IsExclusive.ToString().ToLower() : "false");

                metadata.AdditionalValues.Add("gst", htmlAttributes);
            }
            // Age attribute
            else if (attributeName.StartsWith("Age"))
            {
                var htmlAttributes = new Dictionary<string, object>();

                htmlAttributes.Add(HtmlDataType.DependentPropertyAge, dependentProperty);

                metadata.AdditionalValues.Add("age", htmlAttributes);
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
