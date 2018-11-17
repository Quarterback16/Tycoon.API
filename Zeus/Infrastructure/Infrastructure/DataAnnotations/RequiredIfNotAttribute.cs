using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to validate that this property is required if the value of a dependent property is not equal to.
    /// </summary>
    public class RequiredIfNotAttribute : RequiredIfAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredIfNotAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="dependentValue">The value the dependent property should not be.</param>
        public RequiredIfNotAttribute(string dependentProperty, object dependentValue) : base(dependentProperty, ComparisonType.NotEqualTo, dependentValue) { }
    }
}
