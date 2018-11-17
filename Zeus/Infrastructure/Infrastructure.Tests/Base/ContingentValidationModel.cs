using System.ComponentModel.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.Tests.Base
{
    public abstract class ContingentValidationModel<T> where T : ValidationAttribute
    {
        public T GetAttribute(string property)
        {
            return (T)GetType().GetProperty(property).GetCustomAttributes(typeof(T), false)[0];
        }

        public bool IsValid(string property)
        {
            var attribute = GetAttribute(property);

            var validationContext = new ValidationContext(this, null, null);
            validationContext.DisplayName = property;

            var result = attribute.GetValidationResult(GetType().GetProperty(property).GetValue(this, null), validationContext);

            return result == ValidationResult.Success;
        }
    }
}
