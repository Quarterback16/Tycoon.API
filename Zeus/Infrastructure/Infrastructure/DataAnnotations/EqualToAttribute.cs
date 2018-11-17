using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to validate that the property value is equal to the value of a dependent property.
    /// </summary>
    public class EqualToAttribute : IsAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EqualToAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        public EqualToAttribute(string dependentProperty) : base(ComparisonType.EqualTo, dependentProperty) { }
    }
}
