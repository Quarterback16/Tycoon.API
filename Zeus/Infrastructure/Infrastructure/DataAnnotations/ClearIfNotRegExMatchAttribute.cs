using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to determine whether the property should be cleared based on if the value of a dependent property does not match the regex.
    /// </summary>
    public class ClearIfNotRegExMatchAttribute : ClearIfAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClearIfNotRegExMatchAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="pattern">The regex pattern to match against.</param>
        public ClearIfNotRegExMatchAttribute(string dependentProperty, string pattern) : base(dependentProperty, ComparisonType.NotRegExMatch, pattern) { }
    }
}
