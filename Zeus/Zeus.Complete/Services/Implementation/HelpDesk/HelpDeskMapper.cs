
using Employment.Esc.HelpDeskNotification.Contracts.DataContracts;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Service.Interfaces.HelpDesk;

namespace Employment.Web.Mvc.Service.Implementation.HelpDesk
{
    /// <summary>
    /// Help desk mapper class - map between the wcf request <see cref="InsHelpDeskNotificationRequest"/> InsHelpDeskNotificationRequest and the help desk model class <see cref="HelpDeskModel"/>
    /// </summary>
    public static class HelpDeskMapper 
    {
        /// <summary>
        /// Convert to the ins help desk notification request.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public static InsHelpDeskNotificationRequest ToInsHelpDeskNotificationRequest(this HelpDeskModel src)
        {
            var dest = new InsHelpDeskNotificationRequest();
            dest.application = src.Application;
            dest.callTakenBy = src.UserId;
            dest.dateLogged = src.Date;
            dest.email = src.Email;
            dest.identifier = src.Identifier;
            dest.name = src.Name;
            dest.org = src.Organisation;
            dest.phone = src.Phone;
            dest.site = src.Site;
            dest.subjectArea = src.Subject;
            dest.userID = src.UserId;
            dest.description = src.Description;
            return dest;
        }

    }
}