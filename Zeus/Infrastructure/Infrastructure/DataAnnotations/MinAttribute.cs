using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Properties;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to validate that the property value is equal to or greater than the specified minimum value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MinAttribute : DataTypeAttribute, IClientValidatable
    {
        /// <summary>
        /// Default error message format string.
        /// </summary>
        private string DefaultErrorMessageFormatString
        {
            get { return DataAnnotationsResources.MinAttribute_Invalid; }
        }

        /// <summary>
        /// Gets the type of the data field whose value must be validated.
        /// </summary>
        /// <returns>The type of the data field whose value must be validated.</returns>
        public Type OperandType { get; private set; }

        /// <summary>
        /// Gets the minimum allowed field value.
        /// </summary>
        /// <returns>The minimum value that is allowed for the data field.</returns>
        public object Minimum { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinAttribute" /> class.
        /// </summary>
        /// <param name="minimum">Specifies the minimum value allowed for the data field value.</param>
        public MinAttribute(int minimum) : base("min")
        {
            Minimum = minimum;
            OperandType = typeof(int);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinAttribute" /> class.
        /// </summary>
        /// <param name="minimum">Specifies the minimum value allowed for the data field value.</param>
        public MinAttribute(double minimum) : base("min")
        {
            Minimum = minimum;
            OperandType = typeof(double);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinAttribute" /> class.
        /// </summary>
        /// <param name="type">Specifies the type of the object to test.</param>
        /// <param name="minimum">Specifies the minimum value allowed for the data field value.</param>
        public MinAttribute(Type type, string minimum) : base("min")
        {
            Minimum = minimum;
            OperandType = type;
        }

        /// <summary>
        /// Formats the error message that is displayed when maximum validation fails.
        /// </summary>
        /// <param name="name">The name of the field that caused the validation failure.</param>
        /// <returns>The formatted error message.</returns>
        public override string FormatErrorMessage(string name)
        {
            if (string.IsNullOrEmpty(ErrorMessage) && string.IsNullOrEmpty(ErrorMessageResourceName))
            {
                ErrorMessage = DefaultErrorMessageFormatString;
            }

            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Minimum);
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
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "range"
            };

            clientValidationRule.ValidationParameters.Add("min", Minimum);

            return new[] { clientValidationRule };
        }

        /// <summary>
        /// Checks that the value is equal to or greater than the minimum value.
        /// </summary>
        /// <param name="value">The data field value to validate.</param>
        /// <returns><c>true</c> if the specified value is equal to or greater than the minimum value; otherwise, <c>false</c>.</returns>
        /// <exception cref="T:System.ComponentModel.DataAnnotations.ValidationException">The data field value is less than the minimum value.</exception>
        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return true;
            }

            return ComparisonType.GreaterThanOrEqualTo.Compare(value, Convert.ChangeType(Minimum, OperandType));
        }
    }
}
