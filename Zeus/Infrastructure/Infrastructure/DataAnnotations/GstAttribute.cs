using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to recalculate the GST of the dependent property value when its value is changed (client-side only).
    /// </summary>
    /// <remarks>
    /// By default, the dependent property value is assumed to be GST inclusive. To change this, set <see cref="IsExclusive" /> to <c>true</c>.
    /// </remarks>
    public class GstAttribute : ContingentAttribute
    {
        /// <summary>
        /// Whether the dependent property value does not contain GST (assumes value is inclusive by default).
        /// </summary>
        public bool IsExclusive { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GstAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        public GstAttribute(string dependentProperty) : base(ActionForDependencyType.None, dependentProperty) { }
    }
}
