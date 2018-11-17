using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Helpers;
using Employment.Web.Mvc.Infrastructure.Extensions;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate whether a property is bindable by model binding based on certain criteria.
    /// </summary>
    public class BindableIfEmptyAttribute : ContingentAttribute
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
        /// Initializes a new instance of the <see cref="BindableIfEmptyAttribute" /> class.
        /// </summary>
        /// <remarks>
        /// This constructor implies the dependent property is the property decorated with this attribute (self-referencing).
        /// </remarks>
        public BindableIfEmptyAttribute() : base(ActionForDependencyType.Bindable) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindableIfEmptyAttribute" /> class.
        /// </summary>
        /// <param name="dependentProperty">The dependent property in the model that this object is dependent on.</param>
        public BindableIfEmptyAttribute(string dependentProperty) : base(ActionForDependencyType.Bindable, dependentProperty) { }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="propertyValue">The value of the property decorated with this attribute.</param>
        /// <param name="dependentPropertyValue">The value of the dependent property.</param>
        /// <param name="container">The model this object is contained within.</param>
        /// <returns>true if the specified value is valid; otherwise, false.</returns>
        protected override bool IsConditionMet(object propertyValue, object dependentPropertyValue, object container)
        {
            // Fail if role check does not pass
            if (!(Roles == null || Roles.Length == 0 || Roles.Intersect(UserService.Roles).Any()))
            {
                return false;
            }

            dependentPropertyValue = HandleEnumerableSelectListItem(dependentPropertyValue);

            if (PassOnNull && dependentPropertyValue == null)
            {
                return true;
            }

            if (FailOnNull && dependentPropertyValue == null)
            {
                return false;
            }

            if (dependentPropertyValue == null)
            {
                return true;
            }

            if (dependentPropertyValue is string)
            {
                return string.IsNullOrEmpty(dependentPropertyValue.ToString().Trim());
            }

            return ComparisonType.EqualTo.Compare(dependentPropertyValue, DelegateHelper.CreateConstructorDelegate(dependentPropertyValue.GetType())());
        }
    }
}
