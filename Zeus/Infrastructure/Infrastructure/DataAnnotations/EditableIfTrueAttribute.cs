using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to determine whether the property is editable based on if the value of a dependent property is true.
    /// </summary>
    public class EditableIfTrueAttribute : EditableIfAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditableIfTrueAttribute" /> class.
        /// </summary>
        /// <remarks>
        /// This constructor implies the dependent property is the property decorated with this attribute (self-referencing).
        /// </remarks>
        public EditableIfTrueAttribute() : base(null, ComparisonType.EqualTo, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditableIfTrueAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        public EditableIfTrueAttribute(string dependentProperty) : base(dependentProperty, ComparisonType.EqualTo, true) { }
    }
}
