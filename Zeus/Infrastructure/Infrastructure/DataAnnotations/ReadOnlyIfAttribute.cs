using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to determine whether the property is read only based on certain criteria.
    /// </summary>
    public class ReadOnlyIfAttribute : ContingentAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyIfAttribute" /> class.
        /// </summary>
        /// <remarks>
        /// This constructor implies the dependent property is the property decorated with this attribute (self-referencing).
        /// </remarks>
        /// <param name="dependentValue">The value to compare against.</param>
        public ReadOnlyIfAttribute(object dependentValue) : this(null, ComparisonType.EqualTo, dependentValue) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyIfAttribute" /> class.
        /// </summary>
        /// <remarks>
        /// This constructor implies the dependent property is the property decorated with this attribute (self-referencing).
        /// </remarks>
        /// <param name="comparisonType">The comparison type to use.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        public ReadOnlyIfAttribute(ComparisonType comparisonType, object dependentValue) : base(ActionForDependencyType.Disabled, null, comparisonType, dependentValue) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyIfAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        public ReadOnlyIfAttribute(string dependentProperty, object dependentValue) : this(dependentProperty, ComparisonType.EqualTo, dependentValue) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyIfAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="comparisonType">The comparison type to use.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        public ReadOnlyIfAttribute(string dependentProperty, ComparisonType comparisonType, object dependentValue) : base(ActionForDependencyType.Disabled, dependentProperty, comparisonType, dependentValue) { }
    }
}
