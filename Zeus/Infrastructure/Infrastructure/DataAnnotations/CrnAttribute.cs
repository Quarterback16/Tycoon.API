using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Properties;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to validate that the property value is a valid Centrelink Customer Reference Number (CRN).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CrnAttribute : DataTypeAttribute, IClientValidatable
    {
        /// <summary>
        /// Default error message format string.
        /// </summary>
        private string DefaultErrorMessageFormatString
        {
            get { return DataAnnotationsResources.CrnAttribute_Invalid; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrnAttribute" /> class.
        /// </summary>
        public CrnAttribute() : base("crn") { }

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

            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
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
                ValidationType = "crn"
            };

            return new[] { clientValidationRule };
        }

        /// <summary>
        /// Determines whether the <paramref name="value"/> is a valid Centrelink Customer Reference Number (CRN). 
        /// 
        /// The crn is a 10 character string, consisting of nine numeric values
        /// and a "check sum" character. This function validates a crn and returns
        /// true if the CRN contains valid numbers and a valid checksum.
        ///
        /// Check digit is calculated as follows - 
        /// 
        /// 1. digit1 * 512 + digit2 * 256 + digit3 * 128 + digit4 * 64 +
        /// digit5 * 32 + digit6 * 16 + digit7 * 8 + digit8 * 4 + digit9 * 2 
        /// 
        /// 2. Divide the above value by 11 
        /// 
        /// 3. Use the remainder to convert to the appropriate letter as follows - 
        /// 
        /// Remainder       Check digit
        /// 0               X
        /// 1               V
        /// 2               T
        /// 3               S
        /// 4               L
        /// 5               K
        /// 6               J
        /// 7               H
        /// 8               C
        /// 9               B
        /// 10              A 
        /// 
        /// Example CRN: 123456789T
        /// </summary>
        /// <param name="value">The data field value to validate.</param>
        /// <returns><c>true</c> if the specified value is a valid CRN; otherwise, <c>false</c>.</returns>
        /// <exception cref="T:System.ComponentModel.DataAnnotations.ValidationException">The data field value is not a valid CRN.</exception>
        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return true;
            }

            string crn = value.ToString();

            if (crn.Length != 10)
            {
                return false;
            }

            int multiplier = 512;
            long digitSum = 0;
            char[] checkSumDigits = new[] { 'X', 'V', 'T', 'S', 'L', 'K', 'J', 'H', 'C', 'B', 'A' };

            // Scan digits
            for (int i = 0; i <= 8; i++)
            {
                // If character is numeric, put it into an array
                if (char.IsDigit(crn[i]))
                {
                    // ASCII values for 0-9 are 48-57
                    digitSum += (crn[i] - 48) * multiplier;
                    multiplier = multiplier / 2;
                }
                else
                {
                    // Invalid CRN, don't check further
                    return false;
                }
            }

            // Validate checksum value
            return checkSumDigits[digitSum % 11] == crn[9];
        }
    }
}
