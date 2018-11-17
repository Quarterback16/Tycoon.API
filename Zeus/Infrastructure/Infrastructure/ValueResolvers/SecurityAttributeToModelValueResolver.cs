
//using System.Web.UI;
//using Employment.Web.Mvc.Infrastructure.DataAnnotations;
//using Employment.Web.Mvc.Infrastructure.Models;
//using System.Web.Mvc;
//using Employment.Web.Mvc.Infrastructure.Interfaces;
//using Employment.Web.Mvc.Infrastructure.TypeConverters;

//namespace Employment.Web.Mvc.Infrastructure.ValueResolvers
//{
//    /// <summary>
//    /// Defines a <see cref="ValueResolver{TSource,TDestination}" /> that returns a <see cref="SecurityModel" />.
//    /// </summary>
//    public class SecurityAttributeToModelValueResolver : ValueResolver<SecurityAttribute, SecurityModel>
//    {
//        //private IMappingEngine MappingEngine
//        //{
//        //    get
//        //    {
//        //        var containerProvider = DependencyResolver.Current as IContainerProvider;
//        //        return containerProvider != null ? containerProvider.GetService<IMappingEngine>() : null;
//        //    }
//        //}

//        /// <summary>
//        /// Resolves whether the flag is "Y" or "N".
//        /// </summary>
//        /// <returns>"Y" if the bool is <c>true</c>; otherwise, "N".</returns>
//        protected override SecurityModel ResolveCore(SecurityAttribute source)
//        {
//            SecurityModel dest = new SecurityModel();
//            dest.AllowAny = source.AllowAny;
//            dest.AllowInProduction = source.AllowInProduction;
//            dest.AllowWindowsAuthentication = source.AllowWindowsAuthentication;
//            //dest.ConfigurationManager = source.ConfigurationManager;
//            dest.Contracts = source.Contracts;
//            dest.OrganisationCodes = source.OrganisationCodes;
//            dest.Roles = source.Roles;
//           // dest.UserService = source.UserService;
//            dest.Users = source.Users;

//            return dest;


//            //return MappingEngine.Map<SecurityModel>(source);
//        }
//    } 
//}


