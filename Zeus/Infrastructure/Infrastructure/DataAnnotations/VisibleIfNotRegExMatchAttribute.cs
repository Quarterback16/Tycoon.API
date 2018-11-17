using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to determine whether the property is visible based on if the value of a dependent property does not match the regex.
    /// </summary>
    public class VisibleIfNotRegExMatchAttribute : VisibleIfAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisibleIfNotRegExMatchAttribute" /> class.
        /// </summary>
        /// <remarks>
        /// This constructor implies the dependent property is the property decorated with this attribute (self-referencing).
        /// </remarks>
        /// <param name="pattern">The regex pattern to match against.</param>
        public VisibleIfNotRegExMatchAttribute(string pattern) : base(null, ComparisonType.NotRegExMatch, pattern) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisibleIfNotRegExMatchAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="pattern">The regex pattern to match against.</param>
        public VisibleIfNotRegExMatchAttribute(string dependentProperty, string pattern) : base(dependentProperty, ComparisonType.NotRegExMatch, pattern) { }
    }
}
