using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to determine whether the property should be cleared based on certain criteria.
    /// </summary>
    public class ClearIfAttribute : ContingentAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClearIfAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        public ClearIfAttribute(string dependentProperty, object dependentValue) : this(dependentProperty, ComparisonType.EqualTo, dependentValue) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClearIfAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="comparisonType">The comparison type to use.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        public ClearIfAttribute(string dependentProperty, ComparisonType comparisonType, object dependentValue) : base(ActionForDependencyType.Disabled, dependentProperty, comparisonType, dependentValue) { }
    }
}
