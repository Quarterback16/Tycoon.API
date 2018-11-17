using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to determine whether the property is visible based on certain criteria.
    /// </summary>
    public class VisibleIfAttribute : ContingentAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisibleIfAttribute" /> class.
        /// </summary>
        /// <remarks>
        /// This constructor implies the dependent property is the property decorated with this attribute (self-referencing).
        /// </remarks>
        /// <param name="dependentValue">The value to compare against.</param>
        public VisibleIfAttribute(object dependentValue) : this(null, ComparisonType.EqualTo, dependentValue) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisibleIfAttribute" /> class.
        /// </summary>
        /// <remarks>
        /// This constructor implies the dependent property is the property decorated with this attribute (self-referencing).
        /// </remarks>
        /// <param name="comparisonType">The comparison type to use.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        public VisibleIfAttribute(ComparisonType comparisonType, object dependentValue) : base(ActionForDependencyType.Visible, null, comparisonType, dependentValue) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisibleIfAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        public VisibleIfAttribute(string dependentProperty, object dependentValue) : this(dependentProperty, ComparisonType.EqualTo, dependentValue) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisibleIfAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="comparisonType">The comparison type to use.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        public VisibleIfAttribute(string dependentProperty, ComparisonType comparisonType, object dependentValue) : base(ActionForDependencyType.Visible, dependentProperty, comparisonType, dependentValue) { }
    }
}
