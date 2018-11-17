using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to recalculate an age based on the system date and the dependent property value when its value is changed (client-side only).
    /// </summary>
    public class AgeAttribute : ContingentAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgeAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        public AgeAttribute(string dependentProperty) : base(ActionForDependencyType.None, dependentProperty) { }
    }
}
