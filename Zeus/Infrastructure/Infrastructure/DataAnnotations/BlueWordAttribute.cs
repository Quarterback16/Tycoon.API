using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Properties;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to validate that the property value does not contain words that are deemed inappropriate (blue words).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class BlueWordAttribute : ValidationAttribute
    {
        /// <summary>
        /// Adw service.
        /// </summary>
        private IAdwService AdwService
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IAdwService>() : null;
            }
        }

        /// <summary>
        /// Default error message format string.
        /// </summary>
        private string DefaultErrorMessageFormatString
        {
            get { return DataAnnotationsResources.BlueWordAttribute_Invalid; }
        }

        /// <summary>
        /// Default error message with blue words format string.
        /// </summary>
        private string DefaultErrorMessageWithBlueWordsFormatString
        {
            get { return DataAnnotationsResources.BlueWordAttribute_InvalidWithBlueWords; }
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
        /// <param name="blueWords">The blue words found in the field.</param>
        /// <returns>The formatted error message.</returns>
        public string FormatErrorMessage(string name, IEnumerable<string> blueWords)
        {
            if (string.IsNullOrEmpty(ErrorMessage) && string.IsNullOrEmpty(ErrorMessageResourceName))
            {
                ErrorMessage = DefaultErrorMessageWithBlueWordsFormatString;
            }

            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, string.Join(", ", blueWords));
        }

        /// <summary>Validates the specified object.</summary>
		/// <param name="value">The object to validate.</param>
		/// <param name="validationContext">The <see cref="T:System.ComponentModel.DataAnnotations.ValidationContext" /> object that describes the context where the validation checks are performed. This parameter cannot be null.</param>
		/// <exception cref="T:System.ComponentModel.DataAnnotations.ValidationException">Validation failed.</exception>
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var text = value as string;

            return ValidateBlueWord(validationContext.DisplayName, text);
        }

        /// <summary>
        /// Validates that the text does not contain any blue words.
        /// </summary>
        /// <param name="displayName">The display name of the property to include in the error message.</param>
        /// <param name="text">The text to validate for blue words.</param>
        /// <returns>The validation result of success if no blue words were found; otherwise, a validation result with an error message.</returns>
        internal ValidationResult ValidateBlueWord(string displayName, string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var blueWords = new List<string>();

                // Get unacceptable words
                var unacceptableWords = AdwService.GetRelatedCodes("OFTW", "UNA").ToCodeModelList().Select(a => Regex.Escape(a.Description)).ToList();

                // Get identified words that contain unacceptable words but are identified as being allowed
                var identifiedWords = AdwService.GetRelatedCodes("OFTW", "IDE").ToCodeModelList().Select(a => Regex.Escape(a.Description)).ToList();

                // Build word boundary pattern to match against identified words (the full word must match)
                var identifiedWordsPattern = string.Format(@"\b({0})\b", string.Join("|", identifiedWords));

                // Remove identified words from text so we can ignore them as they are allowed
                text = Regex.Replace(text, identifiedWordsPattern, " ", (RegexOptions.IgnoreCase | RegexOptions.Singleline));

                // Build word boundary pattern to match against unacceptable words (any words containing an unacceptable word will be matched)
                var unacceptableWordsPattern = string.Format("({0})", string.Join("|", unacceptableWords));

                // Get unacceptable word matches
                var matches = Regex.Matches(text, unacceptableWordsPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                foreach (Match match in matches)
                {
                    if (!blueWords.Contains(match.Value))
                    {
                        blueWords.Add(match.Value);
                    }
                }

                if (blueWords.Any())
                {
                    return new ValidationResult(FormatErrorMessage(displayName, blueWords));
                }
            }

            return ValidationResult.Success;
        }
    }
}
