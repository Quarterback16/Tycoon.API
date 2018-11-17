using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to determine whether the property is editable based on certain criteria.
    /// </summary>
    public class EditableIfAttribute : ContingentAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditableIfAttribute" /> class.
        /// </summary>
        /// <remarks>
        /// This constructor implies the dependent property is the property decorated with this attribute (self-referencing).
        /// </remarks>
        /// <param name="dependentValue">The value to compare against.</param>
        public EditableIfAttribute(object dependentValue) : this(null, dependentValue) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditableIfAttribute" /> class.
        /// </summary>
        /// <remarks>
        /// This constructor implies the dependent property is the property decorated with this attribute (self-referencing).
        /// </remarks>
        /// <param name="comparisonType">The comparison type to use.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        public EditableIfAttribute(ComparisonType comparisonType, object dependentValue) : base(ActionForDependencyType.Enabled, null, comparisonType, dependentValue) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditableIfAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        public EditableIfAttribute(string dependentProperty, object dependentValue) : this(dependentProperty, ComparisonType.EqualTo, dependentValue) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditableIfAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="comparisonType">The comparison type to use.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        public EditableIfAttribute(string dependentProperty, ComparisonType comparisonType, object dependentValue) : base(ActionForDependencyType.Enabled, dependentProperty, comparisonType, dependentValue) { }
    }
}
