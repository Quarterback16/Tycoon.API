using System.Collections.Generic;
using System.Web.Mvc;

using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Infrastructure.ValueResolvers
{
    /// <summary>
    /// Defines a value resolver that returns a bool indicating whether the user is in the role.
    /// </summary>
    public class IsInRoleValueResolver : IsInRoleValueResolver<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsInRoleValueResolver{T}" /> class using object.
        /// </summary>
        /// <param name="roles">The roles to compare against.</param>
        public IsInRoleValueResolver(IEnumerable<string> roles) : base(roles) { }
    }

    /// <summary>
    /// Defines a value resolver that returns a bool indicating whether the user is in the role.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    public class IsInRoleValueResolver<TModel> 
    {
        /// <summary>
        /// User service.
        /// </summary>
        public IUserService UserService
        {
            get
            {
                var containerProvider = DependencyResolver.Current as IContainerProvider;

                if (containerProvider != null)
                {
                    return containerProvider.GetService<IUserService>();
                }

                return null;
            }
        }

        private readonly IEnumerable<string> roles;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsInRoleValueResolver{T}" /> class.
        /// </summary>
        /// <param name="roles">The roles to compare against.</param>
        public IsInRoleValueResolver(IEnumerable<string> roles)
        {
            this.roles = roles;
        }

        /// <summary>
        /// Resolves whether the user is in the role.
        /// </summary>
        /// <param name="model">The model to resolve.</param>
        /// <returns><c>true</c> if the user is in the role; otherwise, <c>false</c>.</returns>
        public bool Resolve(TModel model)
        {
            return UserService.IsInRole(roles);
        }
    } 
}


