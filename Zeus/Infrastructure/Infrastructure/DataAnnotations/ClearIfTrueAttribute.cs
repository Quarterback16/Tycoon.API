using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to determine whether the property should be cleared based on if the value of a dependent property is true.
    /// </summary>
    public class ClearIfTrueAttribute : ClearIfAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClearIfTrueAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        public ClearIfTrueAttribute(string dependentProperty) : base(dependentProperty, ComparisonType.EqualTo, true) { }
    }
}
