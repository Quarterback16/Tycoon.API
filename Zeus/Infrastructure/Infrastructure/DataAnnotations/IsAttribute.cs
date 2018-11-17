using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Properties;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to validate that the property value meets a certain criteria against the value of a dependent property.
    /// </summary>
    public class IsAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// Default error message format string.
        /// </summary>
        protected virtual string DefaultErrorMessageFormatString
        {
            get { return DataAnnotationsResources.IsAttribute_Invalid; }
        }

        /// <summary>
        /// Other property in the model that this property is dependent on.
        /// </summary>
        public string DependentProperty { get; protected set; }

        /// <summary>
        /// The comparison type to use.
        /// </summary>
        public ComparisonType ComparisonType { get; private set; }

        /// <summary>
        /// Whether to always pass validation on a null value.
        /// </summary>
        public bool PassOnNull { get; set; }

        /// <summary>
        /// Whether to always fail validation on a null value.
        /// </summary>
        public bool FailOnNull { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IsAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="comparisonType">The comparison type to use.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dependentProperty"/> is <c>null</c>.</exception>
        public IsAttribute(ComparisonType comparisonType, string dependentProperty)
        {
            if (string.IsNullOrEmpty(dependentProperty))
            {
                throw new ArgumentNullException("dependentProperty");
            }

            ComparisonType = comparisonType;
            DependentProperty = dependentProperty;
            PassOnNull = false;
        }

        /// <summary>
        /// Returns the display name of the dependent property based on the model metadata of the container.
        /// </summary>
        /// <param name="container">The object containing the dependent property.</param>
        /// <returns>The display name of the dependent property.</returns>
        private string GetOtherDisplayName(object container)
        {
            if (container != null)
            {
                var dependentPropertyMetadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperty(() => container, container.GetType(), DependentProperty);

                return dependentPropertyMetadata.GetDisplayName();
            }

            return DependentProperty;
        }

        /// <summary>
        /// Format of the error message.
        /// </summary>
        /// <param name="name">Property display name.</param>
        /// <returns>The formatted error message.</returns>
        public override string FormatErrorMessage(string name)
        {
            return FormatErrorMessage(name, DependentProperty);
        }

        /// <summary>
        /// Format of the error message.
        /// </summary>
        /// <param name="displayName">Property display name.</param>
        /// <param name="otherDisplayName">Other property display name.</param>
        /// <returns>The formatted error message.</returns>
        public string FormatErrorMessage(string displayName, string otherDisplayName)
        {
            if (string.IsNullOrEmpty(ErrorMessageResourceName) && string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = DefaultErrorMessageFormatString;
            }

            return string.Format(ErrorMessageString, displayName, ComparisonType.GetDescription(), otherDisplayName);
        }

        /// <summary>
        /// Returns the client validation rules.
        /// </summary>
        /// <param name="metadata">The model metadata of the object being validated.</param>
        /// <param name="context">The controller context.</param>
        /// <returns>The client validation rules.</returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var clientValidationRule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName(), GetOtherDisplayName(metadata.AdditionalValues["ParentModel"])),
                ValidationType = string.Format("is{0}",ComparisonType.ToString().ToLower())
            };

            clientValidationRule.ValidationParameters.Add("dependentproperty", DependentProperty);
            clientValidationRule.ValidationParameters.Add("comparisontype", ComparisonType);
            clientValidationRule.ValidationParameters.Add("passonnull", PassOnNull.ToString().ToLowerInvariant());
            clientValidationRule.ValidationParameters.Add("failonnull", FailOnNull.ToString().ToLowerInvariant());

            return new[] { clientValidationRule };
        }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value of the property decorated with this attribute.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult" /> class.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var properties = TypeDescriptor.GetProperties(validationContext.ObjectType).Cast<PropertyDescriptor>();

            var dependentProperty = properties.FirstOrDefault(p => p.Name == DependentProperty);
            var dependentPropertyValue = dependentProperty != null ? dependentProperty.GetValue(validationContext.ObjectInstance) : null;

            // Get current property by matching validationContext.DisplayName to the property [Display] name value
            var currentProperty = properties.Where(p => p.Attributes.OfType<DisplayAttribute>().Any(a => !string.IsNullOrEmpty(a.Name) && a.Name == validationContext.DisplayName)).FirstOrDefault();

            // If there was no match against the display name, then match directly against the property name as the display name will actually be the property name in this case
            if (currentProperty == null)
            {
                currentProperty = properties.FirstOrDefault(p => p.Name == validationContext.DisplayName);
            }

            // Handle comparing DateTime properties
            if (value != null && dependentPropertyValue != null && currentProperty != null 
                && (currentProperty.PropertyType.GetNonNullableType() == typeof(DateTime) && dependentProperty.PropertyType.GetNonNullableType() == typeof(DateTime)))
            {
                // Get DataType's
                var currentDataType = currentProperty.Attributes.OfType<DataTypeAttribute>().Any() ? currentProperty.Attributes.OfType<DataTypeAttribute>().First().DataType : DataType.DateTime;
                var dependentDataType = dependentProperty.Attributes.OfType<DataTypeAttribute>().Any() ? dependentProperty.Attributes.OfType<DataTypeAttribute>().First().DataType : DataType.DateTime;
                
                // If comparing Date against DateTime (or vice-versa), reset Time component to effectively ignore it
                if ((currentDataType == DataType.Date && dependentDataType == DataType.DateTime)
                    || (currentDataType == DataType.DateTime && dependentDataType == DataType.Date))
                {
                    var currentDateTime = (DateTime)value;
                    value = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 0, 0, 0, 0, currentDateTime.Kind);

                    var dependentDateTime = (DateTime)dependentPropertyValue;
                    dependentPropertyValue = new DateTime(dependentDateTime.Year, dependentDateTime.Month, dependentDateTime.Day, 0, 0, 0, 0, dependentDateTime.Kind);
                }
                // If comparing Time against DateTime (or vice-versa), reset Date component to effectively ignore it
                else if ((currentDataType == DataType.Time && dependentDataType == DataType.DateTime)
                    || (currentDataType == DataType.DateTime && dependentDataType == DataType.Time))
                {
                    var currentDateTime = (DateTime)value;
                    value = new DateTime(1, 1, 1, currentDateTime.Hour, currentDateTime.Minute, currentDateTime.Second, currentDateTime.Millisecond, currentDateTime.Kind);

                    var dependentDateTime = (DateTime)dependentPropertyValue;
                    dependentPropertyValue = new DateTime(1, 1, 1, dependentDateTime.Hour, dependentDateTime.Minute, dependentDateTime.Second, dependentDateTime.Millisecond, dependentDateTime.Kind);
                }
            }

            if (!Is(value, dependentPropertyValue))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName, GetOtherDisplayName(validationContext.ObjectInstance)));
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Checks whether the required condition has been met.
        /// </summary>
        /// <param name="propertyValue">The value of the property decorated with this attribute.</param>
        /// <param name="dependentPropertyValue">The value of the dependent property.</param>
        /// <returns><c>true</c> if the required condition has been met; otherwise, <c>false</c>.</returns>
        private bool Is(object propertyValue, object dependentPropertyValue)
        {
            if (PassOnNull && (propertyValue == null || dependentPropertyValue == null))
            {
                return true;
            }

            if (FailOnNull && (propertyValue == null || dependentPropertyValue == null))
            {
                return false;
            }

            return ComparisonType.Compare(propertyValue, dependentPropertyValue);
        }
    }
}
