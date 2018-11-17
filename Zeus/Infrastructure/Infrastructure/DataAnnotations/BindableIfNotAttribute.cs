using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate whether a property is bindable by model binding based on certain criteria.
    /// </summary>
    public class BindableIfNotAttribute : BindableIfAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindableIfNotAttribute" /> class.
        /// </summary>
        /// <remarks>
        /// This constructor implies the dependent property is the property decorated with this attribute (self-referencing).
        /// </remarks>
        /// <param name="value">The value to compare against.</param>
        public BindableIfNotAttribute(object value) : base(null, ComparisonType.NotEqualTo, value) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableIfNotAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="value">The value to compare against.</param>
        public BindableIfNotAttribute(string dependentProperty, object value) : base(dependentProperty, ComparisonType.NotEqualTo, value) { }
    }
}
