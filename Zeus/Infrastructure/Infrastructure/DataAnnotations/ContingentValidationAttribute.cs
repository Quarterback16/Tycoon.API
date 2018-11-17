using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Properties;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is aware of the model the property is contained within as validation is dependent on the value of andependent property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ContingentValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Default error message format string.
        /// </summary>
        protected virtual string DefaultErrorMessageFormatString
        {
            get { return DataAnnotationsResources.ContingentValidationAttribute_Invalid; }
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
        /// The value to compare against.
        /// </summary>
        public object DependentValue { get; protected set; }

        /// <summary>
        /// Whether to always pass validation on a null dependent value.
        /// </summary>
        public bool PassOnNull { get; set; }

        /// <summary>
        /// Whether to always fail validation on a null dependent value.
        /// </summary>
        public bool FailOnNull { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContingentValidationAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        protected ContingentValidationAttribute(string dependentProperty) : this(dependentProperty, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContingentValidationAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        protected ContingentValidationAttribute(string dependentProperty, object dependentValue) : this(dependentProperty, ComparisonType.EqualTo, dependentValue) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContingentValidationAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="comparisonType">The comparison type to use.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        protected ContingentValidationAttribute(string dependentProperty, ComparisonType comparisonType, object dependentValue)
        {
            DependentProperty = dependentProperty;
            ComparisonType = comparisonType;
            DependentValue = dependentValue;
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
        protected string FormatErrorMessage(string displayName, string otherDisplayName)
        {
            if (string.IsNullOrEmpty(ErrorMessage) && string.IsNullOrEmpty(ErrorMessageResourceName))
            {
                ErrorMessage = DefaultErrorMessageFormatString;
            }

            return string.Format(ErrorMessageString, displayName, otherDisplayName, ComparisonType.GetDescription(), DependentValue);
        }

        /// <summary>
        /// Returns the display name of the dependent property based on the model metadata of the container.
        /// </summary>
        /// <param name="container">The object containing the dependent property.</param>
        /// <returns>The display name of the dependent property.</returns>
        protected string GetOtherDisplayName(object container)
        {
            if (container != null)
            {
                var dependentPropertyMetadata = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperty(() => container, container.GetType(), DependentProperty);

                return dependentPropertyMetadata.GetDisplayName();
            }

            return DependentProperty;
        }

        /// <summary>
        /// Get the value of the dependent property from the container.
        /// </summary>
        /// <param name="container">The object containing the dependent property.</param>
        /// <returns>The value of the dependent property.</returns>
        protected object GetDependentPropertyValue(object container)
        {
            if (DependentProperty != null)
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

            return null;
        }

        /// <summary>
        /// Determines whether the specified value of the object meets the condition.
        /// </summary>
        /// <param name="propertyValue">The value of the property decorated with this attribute.</param>
        /// <param name="dependentPropertyValue">The value of the dependent property.</param>
        /// <returns><c>true</c> if the required condition has been met; otherwise, <c>false</c>.</returns>
        public abstract bool IsConditionMet(object propertyValue, object dependentPropertyValue);

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
    }
}
