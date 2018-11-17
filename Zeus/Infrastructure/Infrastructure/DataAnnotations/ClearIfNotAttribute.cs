using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to determine whether the property should be cleared based on if the value of a dependent property is not equal to.
    /// </summary>
    public class ClearIfNotAttribute : ClearIfAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClearIfNotAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="value">The value to compare against.</param>
        public ClearIfNotAttribute(string dependentProperty, object value) : base(dependentProperty, ComparisonType.NotEqualTo, value) { }
    }
}
