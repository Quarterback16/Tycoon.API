using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to determine whether the property is visible based on if the value of a dependent property matches the regex.
    /// </summary>
    public class VisibleIfRegExMatchAttribute : VisibleIfAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisibleIfRegExMatchAttribute" /> class.
        /// </summary>
        /// <remarks>
        /// This constructor implies the dependent property is the property decorated with this attribute (self-referencing).
        /// </remarks>
        /// <param name="pattern">The regex pattern to match against.</param>
        public VisibleIfRegExMatchAttribute(string pattern) : base(null, ComparisonType.RegExMatch, pattern) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisibleIfRegExMatchAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="pattern">The regex pattern to match against.</param>
        public VisibleIfRegExMatchAttribute(string dependentProperty, string pattern) : base(dependentProperty, ComparisonType.RegExMatch, pattern) { }
    }
}
