using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to determine whether the property is visible based on if the value of a dependent property is false.
    /// </summary>
    public class VisibleIfFalseAttribute : VisibleIfAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisibleIfFalseAttribute" /> class.
        /// </summary>
        /// <remarks>
        /// This constructor implies the dependent property is the property decorated with this attribute (self-referencing).
        /// </remarks>
        public VisibleIfFalseAttribute() : base(null, ComparisonType.EqualTo, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisibleIfFalseAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        public VisibleIfFalseAttribute(string dependentProperty) : base(dependentProperty, ComparisonType.EqualTo, false) { }
    }
}
