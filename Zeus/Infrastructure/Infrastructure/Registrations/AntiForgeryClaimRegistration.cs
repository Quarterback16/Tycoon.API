using System.Diagnostics.CodeAnalysis;
using System.Web.Helpers;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;

namespace Employment.Web.Mvc.Infrastructure.Registrations
{
    /// <summary>
    /// Represents a registration that is used to register the unique user claim used for anti-forgery.
    /// 
    /// Excluded From Code Coverage as not unit "testable"
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AntiForgeryClaimRegistration : IRegistration
    {
        /// <summary>
        /// Register anti-forgery claim.
        /// </summary>
        public void Register()
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimType.Name;
        }
    }
}
