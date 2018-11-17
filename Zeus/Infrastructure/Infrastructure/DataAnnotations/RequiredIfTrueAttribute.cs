using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to validate that this property is required if the value of a dependent property is true.
    /// </summary>
    public class RequiredIfTrueAttribute : RequiredIfAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredIfTrueAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        public RequiredIfTrueAttribute(string dependentProperty) : base(dependentProperty, ComparisonType.EqualTo, true) { }
    }
}
