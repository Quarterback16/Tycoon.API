using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Properties;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to validate that the property value is a valid Australian Business Number (ABN).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AbnAttribute : DataTypeAttribute, IClientValidatable
    {
        /// <summary>
        /// Default error message format string.
        /// </summary>
        private string DefaultErrorMessageFormatString
        {
            get { return DataAnnotationsResources.AbnAttribute_Invalid; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbnAttribute" /> class.
        /// </summary>
        public AbnAttribute() : base("abn") { }

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
                ValidationType = "abn"
            };

            return new[] { clientValidationRule };
        }

        /// <summary>
        /// Determines whether the <paramref name="value"/> is a valid Australian Business Number (ABN). 
        /// </summary>
        /// <param name="value">The data field value to validate.</param>
        /// <returns><c>true</c> if the specified value is a valid ABN; otherwise, <c>false</c>.</returns>
        /// <exception cref="T:System.ComponentModel.DataAnnotations.ValidationException">The data field value is not a valid ABN.</exception>
        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return true;
            }

            string abn = value.ToString();
            
            if (abn.Length != 11)
            {
                return false;
            }

            int[] weight = { 10, 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 };
            int sum = 0;
            int digit;

            // Sum the multiplication of all the digits and weights
            for (int i = 0; i < weight.Length; i++)
            {
                // If character is numeric, put it into an array
                if (char.IsDigit(abn[i]) && int.TryParse(abn[i].ToString(), out digit))
                {
                    // Subtract 1 from the first digit before multiplying against the weight
                    sum += (i == 0) ? (digit - 1) * weight[i] : digit * weight[i];
                }
                else
                {
                    // Invalid ABN, don't check further
                    return false;
                }
            }

            // Divide the sum by 89, if there is no remainder the ABN is valid
            return (sum % 89) == 0;
        }
    }
}
