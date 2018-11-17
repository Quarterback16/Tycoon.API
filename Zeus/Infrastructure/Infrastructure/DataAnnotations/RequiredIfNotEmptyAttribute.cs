using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Helpers;
using Employment.Web.Mvc.Infrastructure.Properties;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to validate that this property is required if the value of a dependent property is not empty.
    /// </summary>
    public class RequiredIfNotEmptyAttribute : RequiredIfAttribute
    {
        /// <summary>
        /// Default error message format string.
        /// </summary>
        protected override string DefaultErrorMessageFormatString
        {
            get { return DataAnnotationsResources.RequiredIfNotEmptyAttribute_Invalid; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredIfNotEmptyAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        public RequiredIfNotEmptyAttribute(string dependentProperty) : base(dependentProperty, ComparisonType.NotEqualTo, string.Empty) { }

        /// <summary>
        /// Checks whether the required condition has been met.
        /// </summary>
        /// <param name="propertyValue">The value of the property decorated with this attribute.</param>
        /// <param name="dependentPropertyValue">The value of the dependent property.</param>
        /// <param name="valueToTestAgainst">The value to test the dependent property value against.</param>
        /// <returns><c>true</c> if the required condition has been met; otherwise, <c>false</c>.</returns>
        protected override bool IsConditionMet(object propertyValue, object dependentPropertyValue, object valueToTestAgainst)
        {
            dependentPropertyValue = HandleEnumerableSelectListItem(dependentPropertyValue);

            if (PassOnNull && dependentPropertyValue == null)
            {
                return true;
            }

            if (FailOnNull && dependentPropertyValue == null)
            {
                return false;
            }

            if (dependentPropertyValue == null)
            {
                return false;
            }

            if (dependentPropertyValue is string)
            {
                return !string.IsNullOrEmpty(dependentPropertyValue.ToString().Trim());
            }

            return ComparisonType.NotEqualTo.Compare(dependentPropertyValue, DelegateHelper.CreateConstructorDelegate(dependentPropertyValue.GetType())());
        }
    }
}
