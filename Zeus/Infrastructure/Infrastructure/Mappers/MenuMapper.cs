using System.Collections.Generic;
using Employment.Esc.Adw.Contracts.DataContracts;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;

namespace Employment.Web.Mvc.Infrastructure.Mappers
{
    /// <summary>
    /// Represents a mapper that is used to map between the menu related attributes to equivelant models.
    /// </summary>
    public static class MenuMapper 
    {
        /// <summary>
        /// Map between menu related attributes to equivelant models.
        /// </summary>
        public static MenuModel  ToMenuModel(this MenuAttribute src)
        {
            var dest = new MenuModel();
            dest.Action = src.Action;
            dest.ActionSecurity = ToSecurityModel(src.ActionSecurity);
            dest.Area = src.Area;
            dest.Controller = src.Controller;
            dest.ControllerSecurity = ToSecurityModel(src.ControllerSecurity);
            dest.Name = src.Name;
            dest.Order = src.Order;
            dest.ParentAction = src.ParentAction;
            dest.ParentArea = src.ParentArea;
            dest.ParentController = src.ParentController;
            dest.Parameters = src.Parameters;
            return dest;
        }

        /// <summary>
        /// Convert to the security model.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static SecurityModel ToSecurityModel(SecurityAttribute source)
        {
            SecurityModel dest = new SecurityModel();
            if (source == null)
            {
                return null;
            }
            dest.AllowAny = source.AllowAny;
            dest.AllowInProduction = source.AllowInProduction;
            dest.AllowWindowsAuthentication = source.AllowWindowsAuthentication;
            dest.Contracts = source.Contracts;
            dest.OrganisationCodes = source.OrganisationCodes;
            dest.Roles = source.Roles;
            dest.Users = source.Users;

            return dest;
        }


        /// <summary>
        /// Convert to the list user model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static List<UserModel> ToListUserModel(this User[] src)
        {
            List<UserModel> result = new List<UserModel>();
            if (src != null)
            {
                foreach (var usr in src)
                {
                    result.Add(usr.ToListUserModel());
                }
            }
            return result;
        }
        /// <summary>
        /// Convert to the list user model.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static UserModel ToListUserModel(this User src)
        {
            var dest = new UserModel();
            dest.FirstName = src.FirstName;
            dest.LastName = src.LastName;
            dest.OrganisationCode = src.Organisation;
            dest.UserID = src.UserId;
            return dest;
        }
    }
}
