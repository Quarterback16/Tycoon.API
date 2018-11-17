using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to determine whether the property is read only based on if the value of a dependent property is true.
    /// </summary>
    public class ReadOnlyIfTrueAttribute : ReadOnlyIfAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyIfTrueAttribute" /> class.
        /// </summary>
        /// <remarks>
        /// This constructor implies the dependent property is the property decorated with this attribute (self-referencing).
        /// </remarks>
        public ReadOnlyIfTrueAttribute() : base(null, ComparisonType.EqualTo, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyIfTrueAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        public ReadOnlyIfTrueAttribute(string dependentProperty) : base(dependentProperty, ComparisonType.EqualTo, true) { }
    }
}
