using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to determine whether the property should be cleared based on if the value of a dependent property is false.
    /// </summary>
    public class ClearIfFalseAttribute : ClearIfAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClearIfFalseAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        public ClearIfFalseAttribute(string dependentProperty) : base(dependentProperty, ComparisonType.EqualTo, false) { }
    }
}
