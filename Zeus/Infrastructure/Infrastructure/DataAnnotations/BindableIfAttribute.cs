using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate whether a property is bindable by model binding based on certain criteria.
    /// </summary>
    public class BindableIfAttribute : ContingentAttribute
    {
        /// <summary>
        /// User service.
        /// </summary>
        public IUserService UserService
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                return (containerProvider != null) ? containerProvider.GetService<IUserService>() : null;
            }
        }

        /// <summary>
        /// Specific roles required to be bindable. If empty, no roles are required.
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableIfAttribute" /> class.
        /// </summary>
        /// <remarks>
        /// This constructor implies the dependent property is the property decorated with this attribute (self-referencing).
        /// </remarks>
        /// <param name="dependentValue">The value to compare against.</param>
        public BindableIfAttribute(object dependentValue) : this(null, dependentValue) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableIfAttribute" /> class.
        /// </summary>
        /// <remarks>
        /// This constructor implies the dependent property is the property decorated with this attribute (self-referencing).
        /// </remarks>
        /// <param name="comparisonType">The comparison type to use.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        public BindableIfAttribute(ComparisonType comparisonType, object dependentValue) : base(ActionForDependencyType.Enabled, null, comparisonType, dependentValue) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableIfAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        public BindableIfAttribute(string dependentProperty, object dependentValue) : this(dependentProperty, ComparisonType.EqualTo, dependentValue) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableIfAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        /// <param name="comparisonType">The comparison type to use.</param>
        /// <param name="dependentValue">The value to compare against.</param>
        public BindableIfAttribute(string dependentProperty, ComparisonType comparisonType, object dependentValue) : base(ActionForDependencyType.Enabled, dependentProperty, comparisonType, dependentValue) { }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="propertyValue">The value of the property decorated with this attribute.</param>
        /// <param name="dependentPropertyValue">The value of the dependent property.</param>
        /// <param name="container">The model this object is contained within.</param>
        /// <returns>true if the specified value is valid; otherwise, false.</returns>
        protected override bool IsConditionMet(object propertyValue, object dependentPropertyValue, object container)
        {
            return base.IsConditionMet(propertyValue, dependentPropertyValue, container) && (Roles == null || Roles.Length == 0 || Roles.Intersect(UserService.Roles).Any());
        }
    }
}
