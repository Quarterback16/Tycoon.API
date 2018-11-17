using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to validate that this property is required if the value of a dependent property is false.
    /// </summary>
    public class RequiredIfFalseAttribute : RequiredIfAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredIfFalseAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        public RequiredIfFalseAttribute(string dependentProperty) : base(dependentProperty, ComparisonType.EqualTo, false) { }
    }
}
