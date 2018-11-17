using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to validate that the property value is not equal to the value of a dependent property.
    /// </summary>
    public class NotEqualToAttribute : IsAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotEqualToAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        public NotEqualToAttribute(string dependentProperty) : base(ComparisonType.NotEqualTo, dependentProperty) { }
    }
}
