using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Extensions;

namespace Employment.Web.Mvc.Infrastructure.ModelValidators
{
    /// <summary>
    /// Represents a custom Model Validator that adds support for the <see cref="RequiredAttribute" /> on <see cref="SelectList" />, <see cref="MultiSelectList" />, <see cref="IEnumerable{SelectListItem}" /> and <see cref="bool" /> objects.
    /// </summary>
    public class InfrastructureRequiredAttributeAdapter : RequiredAttributeAdapter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InfrastructureRequiredAttributeAdapter" /> class.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        /// <param name="context">The controller context.</param>
        /// <param name="attribute">The attribute.</param>
        public InfrastructureRequiredAttributeAdapter(ModelMetadata metadata, ControllerContext context, RequiredAttribute attribute) : base(metadata, context, attribute) { }

        /// <summary>
        /// Validate whether the model is required and has a value. Includes custom support for <see cref="SelectList" />, <see cref="MultiSelectList" />, <see cref="IEnumerable{SelectListItem}" /> and <see cref="bool" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>The model validation result.</returns>
        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            var model = Metadata.Model;

            if (Metadata.ModelType == typeof(SelectList) || Metadata.ModelType == typeof(MultiSelectList) || Metadata.ModelType == typeof(IEnumerable<SelectListItem>))
            {
                // Force required message on a SelectList/MultiSelectList/IEnumerable<SelectListItem> if it has no valid selection(s) by setting model to null
                if (model != null && !((IEnumerable<SelectListItem>)model).Any(m => m.Selected && !string.IsNullOrEmpty(m.Value)))
                {
                    model = null;
                }
            }
            else if (Metadata.ModelType == typeof(bool))
            {
                // Force required message to show on a bool with [Required], by setting value to null if it is not true
                if (model != null && model is bool && !(bool)model && Metadata.GetAttribute<RequiredAttribute>() != null)
                {
                    model = null;
                }
            }
            
            var validationContext = new ValidationContext(container ?? Metadata.Model, null, null);
            validationContext.DisplayName = Metadata.GetDisplayName();
            var validationResult = Attribute.GetValidationResult(model, validationContext);

            if (validationResult != ValidationResult.Success)
            {
                yield return new ModelValidationResult
                {
                    Message = validationResult.ErrorMessage
                };
            }
            yield break;
        }
    }
}
