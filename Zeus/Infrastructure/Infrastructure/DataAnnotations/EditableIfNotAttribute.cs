using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to determine whether the property is editable based on if the value of a dependent property is not equal to.
    /// </summary>
    public class EditableIfNotAttribute : EditableIfAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditableIfNotAttribute" /> class.
        /// </summary>
        /// <remarks>
        /// This constructor implies the dependent property is the property decorated with this attribute (self-referencing).
        /// </remarks>
        /// <param name="value">The value to compare against.</param>
        public EditableIfNotAttribute(object value) : base(null, ComparisonType.NotEqualTo, value) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditableIfNotAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="value">The value to compare against.</param>
        public EditableIfNotAttribute(string dependentProperty, object value) : base(dependentProperty, ComparisonType.NotEqualTo, value) { }
    }
}
