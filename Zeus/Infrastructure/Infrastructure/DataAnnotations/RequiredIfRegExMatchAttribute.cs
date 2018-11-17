using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to validate that this property is required if the value of a dependent property matches the regex.
    /// </summary>
    public class RequiredIfRegExMatchAttribute : RequiredIfAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredIfRegExMatchAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="pattern">The regex pattern to match against.</param>
        public RequiredIfRegExMatchAttribute(string dependentProperty, string pattern) : base(dependentProperty, ComparisonType.RegExMatch, pattern) { }
    }
}
