using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Properties;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to validate that the number of characters in the property value is within the specified minimum and maximum length.
    /// </summary>
    /// <remarks>
    /// Only the length of the whole number is validated. Formatting and decimal values are ignored.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NumericLengthAttribute : StringLengthAttribute
    {
        /// <summary>
        /// Default error message format string.
        /// </summary>
        private string DefaultErrorMessageFormatString
        {
            get { return DataAnnotationsResources.NumericLengthAttribute_Invalid; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericLengthAttribute" /> class.
        /// </summary>
        /// <param name="maximumLength">The maximum length of the numeric string.</param>
        public NumericLengthAttribute(int maximumLength) : base(maximumLength) { }

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

                if (MinimumLength != 0)
                {
                    ErrorMessage = DataAnnotationsResources.NumericLengthAttribute_InvalidWithMinimumLength;
                }
            }

            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, MaximumLength, MinimumLength);
        }

        /// <summary>
        /// Include client side validation.
        /// </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            return new []
			{
				new ModelClientValidationStringLengthRule(ErrorMessage, MinimumLength, MaximumLength)
			};
        }

        /// <summary>Determines whether a specified object is valid.</summary>
        /// <param name="value">The object to validate.</param>
        /// <returns><c>true</c> if the specified object is valid; otherwise, <c>false</c>.</returns>
        public override bool IsValid(object value)
        {
            string stringValue = null;

            if (value != null)
            {
                // Ignore negative symbol and decimal values for purposes of character length validation
                stringValue = value.ToString().Replace("-", string.Empty);

                if (stringValue.Contains('.'))
                {
                    stringValue = stringValue.Split('.').First();
                }
            }

            return base.IsValid(stringValue);
        }
    }
}
