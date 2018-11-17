using System;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Properties;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to validate that the property value does not contain special characters (such as accented characters).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SpecialCharactersAttribute : ValidationAttribute
    {
        /// <summary>
        /// Default error message format string.
        /// </summary>
        private string DefaultErrorMessageFormatString
        {
            get { return DataAnnotationsResources.SpecialCharactersAttribute_Invalid; }
        }

        /// <summary>
        /// Default error message with characters format string.
        /// </summary>
        private string DefaultErrorMessageWithCharactersFormatString
        {
            get { return DataAnnotationsResources.SpecialCharactersAttribute_InvalidWithCharacters; }
        }

        /// <summary>
        /// Formats the error message that is displayed when validation fails.
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
        /// Formats the error message that is displayed when validation fails.
        /// </summary>
        /// <param name="name">The name of the field that caused the validation failure.</param>
        /// <param name="characters">The invalid characters found in the field.</param>
        /// <returns>The formatted error message.</returns>
        public string FormatErrorMessage(string name, IEnumerable<string> characters)
        {
            if (string.IsNullOrEmpty(ErrorMessage) && string.IsNullOrEmpty(ErrorMessageResourceName))
            {
                ErrorMessage = DefaultErrorMessageWithCharactersFormatString;
            }

            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, string.Join(", ", characters));
        }

        /// <summary>Validates the specified object.</summary>
		/// <param name="value">The object to validate.</param>
		/// <param name="validationContext">The <see cref="T:System.ComponentModel.DataAnnotations.ValidationContext" /> object that describes the context where the validation checks are performed. This parameter cannot be null.</param>
		/// <exception cref="T:System.ComponentModel.DataAnnotations.ValidationException">Validation failed.</exception>
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var text = value as string;

            return ValidateSpecialCharacters(validationContext.DisplayName, text);
        }

        /// <summary>
        /// Validates that the text does not contain any special characters.
        /// </summary>
        /// <param name="displayName">The display name of the property to include in the error message.</param>
        /// <param name="text">The text to validate for special characters.</param>
        /// <returns>The validation result of success if no special characters were found; otherwise, a validation result with an error message.</returns>
        internal ValidationResult ValidateSpecialCharacters(string displayName, string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var specialCharacters = new HashSet<string>();

                for (int i = 0; i < text.Length; i++)
                {
                    var original = text[i].ToString();
                    var normalized = string.Concat(original.Normalize(NormalizationForm.FormD).Where(c => c < 128 && CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));

                    if (string.Compare(original, normalized) != 0)
                    {
                        specialCharacters.Add(original);
                    }
                }

                if (specialCharacters.Any())
                {
                    return new ValidationResult(FormatErrorMessage(displayName, specialCharacters));
                }
            }

            return ValidationResult.Success;
        }
    }
}
