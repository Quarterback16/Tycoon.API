using System;
using System.Linq;
using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.DataAnnotations
{
    /// <summary>
    /// Represents an attribute that is used to indicate whether a property is bindable by model binding.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class BindableAttribute : Attribute
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
        public string[] Roles { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employment.Web.Mvc.Infrastructure.DataAnnotations.BindableAttribute" /> class.
        /// </summary>
        /// <param name="roles">If specified, the current user must have one of the roles for the property to be bindable; otherwise, if unspecified it will default to be bindable for all users.</param>
        public BindableAttribute(params string[] roles)
        {
            Roles = roles;
        }

        /// <summary>
        /// Determines whether the property is bindable.
        /// </summary>
        /// <returns>true if the property is bindable; otherwise, false.</returns>
        public bool IsBindable()
        {
            return (Roles == null || Roles.Length == 0 || Roles.Intersect(UserService.Roles).Any());
        }
    }
}
