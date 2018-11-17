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
    /// Represents an attribute that is used to validate that the property value is equal to or greater than the specified maximum value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MaxAttribute : DataTypeAttribute, IClientValidatable
    {
        /// <summary>
        /// Default error message format string.
        /// </summary>
        private string DefaultErrorMessageFormatString
        {
            get { return DataAnnotationsResources.MaxAttribute_Invalid; }
        }

        /// <summary>
        /// Gets the type of the data field whose value must be validated.
        /// </summary>
        /// <returns>The type of the data field whose value must be validated.</returns>
        public Type OperandType { get; private set; }

        /// <summary>
        /// Gets the maximum allowed field value.
        /// </summary>
        /// <returns>The maximum value that is allowed for the data field.</returns>
        public object Maximum { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxAttribute" /> class.
        /// </summary>
        /// <param name="maximum">Specifies the maximum value allowed for the data field value.</param>
        public MaxAttribute(int maximum)
            : base("max")
        {
            Maximum = maximum;
            OperandType = typeof(int);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxAttribute" /> class.
        /// </summary>
        /// <param name="maximum">Specifies the maximum value allowed for the data field value.</param>
        public MaxAttribute(double maximum)
            : base("max")
        {
            Maximum = maximum;
            OperandType = typeof(double);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxAttribute" /> class.
        /// </summary>
        /// <param name="type">Specifies the type of the object to test.</param>
        /// <param name="maximum">Specifies the maximum value allowed for the data field value.</param>
        public MaxAttribute(Type type, string maximum)
            : base("max")
        {
            Maximum = maximum;
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

            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Maximum);
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

            clientValidationRule.ValidationParameters.Add("max", Maximum);

            return new[] { clientValidationRule };
        }

        /// <summary>
        /// Checks that the value is equal to or less than the maximum value.
        /// </summary>
        /// <param name="value">The data field value to validate.</param>
        /// <returns><c>true</c> if the specified value is equal to or less than the maximum value; otherwise, <c>false</c>.</returns>
        /// <exception cref="T:System.ComponentModel.DataAnnotations.ValidationException">The data field value is less than the maximum value.</exception>
        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return true;
            }

            return ComparisonType.LessThanOrEqualTo.Compare(value, Convert.ChangeType(Maximum, OperandType));
        }
    }
}
