using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate whether a property is bindable by model binding based on certain criteria.
    /// </summary>
    public class BindableIfNotRegExMatchAttribute : BindableIfAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindableIfNotRegExMatchAttribute" /> class.
        /// </summary>
        /// <remarks>
        /// This constructor implies the dependent property is the property decorated with this attribute (self-referencing).
        /// </remarks>
        /// <param name="pattern">The regex pattern to match against.</param>
        public BindableIfNotRegExMatchAttribute(string pattern) : base(null, ComparisonType.NotRegExMatch, pattern) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableIfNotRegExMatchAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="pattern">The regex pattern to match against.</param>
        public BindableIfNotRegExMatchAttribute(string dependentProperty, string pattern) : base(dependentProperty, ComparisonType.NotRegExMatch, pattern) { }
    }
}
