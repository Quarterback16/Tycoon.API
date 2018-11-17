using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate whether a property is bindable by model binding based on certain criteria.
    /// </summary>
    public class BindableIfFalseAttribute : BindableIfAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindableIfFalseAttribute" /> class.
        /// </summary>
        /// <remarks>
        /// This constructor implies the dependent property is the property decorated with this attribute (self-referencing).
        /// </remarks>
        public BindableIfFalseAttribute() : base(null, ComparisonType.EqualTo, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableIfFalseAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        public BindableIfFalseAttribute(string dependentProperty) : base(dependentProperty, ComparisonType.EqualTo, false) { }
    }
}
