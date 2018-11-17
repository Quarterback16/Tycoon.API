using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to always clear a property when the dependent property value is changed (client-side only).
    /// </summary>
    public class ClearAttribute : ClearIfNotEmptyAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClearIfAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        public ClearAttribute(string dependentProperty) : base(dependentProperty)
        {
            DependencyType = ActionForDependencyType.None;
            PassOnNull = true;
        }
    }
}
