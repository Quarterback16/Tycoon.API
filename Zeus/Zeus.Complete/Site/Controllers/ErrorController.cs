using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure;
using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using Employment.Web.Mvc.Zeus.ViewModels;


namespace Employment.Web.Mvc.Zeus.Controllers
{
    /// <summary>
    /// Defines the error controller for the RHEA application.
    /// </summary>
    [Security(AllowAny = true)]
    public class ErrorController : InfrastructureController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorController" /> class.
        /// </summary>
        /// <param name="userService">User service for retrieving user data.</param>
        /// <param name="adwService">Adw service for retrieving ADW data.</param>
        public ErrorController( IUserService userService, IAdwService adwService) : base( userService, adwService) { }

        /// <summary>
        /// Error page for HTTP Status Code 401 (unauthorized).
        /// </summary>
        //[Menu("Access Denied")]
        public ActionResult Http401()
        {
            PageTitle = "Access Denied";

            // Set response headers incase the user goes directly to this action
            Response.StatusCode = 401;
            Response.TrySkipIisCustomErrors = true;

            var statusDescription = "You don't have permission to access this page.";

            // Check for custom error status code description
            if (ControllerContext.HttpContext.Items.Contains("ErrorStatusCodeDescription"))
            {
                var customStatusDescription = ControllerContext.HttpContext.Items["ErrorStatusCodeDescription"] as string;

                if (!string.IsNullOrEmpty(customStatusDescription))
                {
                    statusDescription = customStatusDescription;
                }
            }

            var model = new ContentViewModel()
                .AddTitle("Access Denied")
                .AddParagraph(statusDescription)
                .BeginParagraph()
                    .AddText("For assistance, please contact the ES Help Desk on 1300 305 520 or ")
                    .AddEmailLink("eshelpdesk@employment.gov.au")
                    .AddText(".")
                .EndParagraph();

            model.Hidden = new[] { LayoutType.LeftHandNavigation, LayoutType.RequiredFieldsMessage };

            return View(model);
        }

        /// <summary>
        /// Error page for HTTP Status Code 403 (forbidden).
        /// </summary>
        //[Menu("Access Denied")]
        public ActionResult Http403()
        {
            PageTitle = "Access Denied";

            // Set response headers incase the user goes directly to this action
            Response.StatusCode = 403;
            Response.TrySkipIisCustomErrors = true;

            var statusDescription = "You don't have permission to access this page.";

            // Check for custom error status code description
            if (ControllerContext.HttpContext.Items.Contains("ErrorStatusCodeDescription"))
            {
                var customStatusDescription = ControllerContext.HttpContext.Items["ErrorStatusCodeDescription"] as string;

                if (!string.IsNullOrEmpty(customStatusDescription))
                {
                    statusDescription = customStatusDescription;
                }
            }

            var model = new ContentViewModel()
                .AddTitle("Access Denied")
                .AddParagraph(statusDescription)
                .BeginParagraph()
                    .AddText("For assistance, please contact the ES Help Desk on 1300 305 520 or ")
                    .AddEmailLink("eshelpdesk@employment.gov.au")
                    .AddText(".")
                .EndParagraph();

            model.Hidden = new[] { LayoutType.LeftHandNavigation, LayoutType.RequiredFieldsMessage };

            return View(model);
        }

        /// <summary>
        /// Error page for HTTP Status Code 404 (not found).
        /// </summary>
        //[Menu("Not Found")]
        public ActionResult Http404()
        {
            PageTitle = "Not Found";

            // Set response headers incase the user goes directly to this action
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;

            var statusDescription = "Your requested page does not exist. Please check the URL and try again.";

            // Check for custom error status code description
            if (ControllerContext.HttpContext.Items.Contains("ErrorStatusCodeDescription"))
            {
                var customStatusDescription = ControllerContext.HttpContext.Items["ErrorStatusCodeDescription"] as string;

                if (!string.IsNullOrEmpty(customStatusDescription))
                {
                    statusDescription = customStatusDescription;
                }
            }

            var model = new ContentViewModel()
                .AddTitle("Not Found")
                .AddParagraph(statusDescription)
                .BeginParagraph()
                    .AddText("For assistance, please contact the ES Help Desk on 1300 305 520 or ")
                    .AddEmailLink("eshelpdesk@employment.gov.au")
                    .AddText(".")
                .EndParagraph();

            model.Hidden = new[] { LayoutType.LeftHandNavigation, LayoutType.RequiredFieldsMessage };

            return View(model);
        }

        /// <summary>
        /// Error page for HTTP Status Code 500 (internal server error).
        /// </summary>
        //[Menu("Application Error")]
        public ActionResult Http500()
        {
            PageTitle = "Application Error";

            // Set response headers incase the user goes directly to this action
            Response.StatusCode = 500;
            Response.TrySkipIisCustomErrors = true;

            List<string> errorIDs;

            // Get logged error ID's for current request
            if (!UserService.Session.TryGet(MvcApplication.LoggedErrorIDKey, out errorIDs))
            {
                errorIDs = new List<string>();
            }

            // Clear stored logged error ID's for current request so next request starts with no logged error ID's
            UserService.Session.Remove(MvcApplication.LoggedErrorIDKey);

            var model = new ContentViewModel()
                .AddTitle("Application Error")
                .AddParagraph("The server encountered an internal error and was unable to process your request. Please try again later.");

            if (errorIDs.Any())
            {
                if (errorIDs.Count == 1)
                {
                    model.BeginParagraph()
                            .AddText("Your error ID is: ")
                            .AddStrongText(errorIDs.FirstOrDefault())
                        .EndParagraph();
                }
                else
                {
                    model.AddParagraph("Your error ID's are:")
                        .BeginUnorderedList();

                    foreach (var errorID in errorIDs)
                    {
                        model.BeginListItem()
                                .AddStrongText(errorID)
                            .EndListItem();
                    }

                    model.EndUnorderedList();
                }
            }

            model.BeginParagraph()
                    .AddText("For assistance, please contact the ES Help Desk on 1300 305 520 or ")
                    .AddEmailLink("eshelpdesk@employment.gov.au")
                    .AddText(".")
                .EndParagraph();

            model.Hidden = new[] { LayoutType.LeftHandNavigation, LayoutType.RequiredFieldsMessage };

            return View(model);
        }


        /// <summary>
        /// Error page to be navigated to upon encountering error / invalid response while processing HTML Report.
        /// </summary>
        /// <remarks>
        /// This action should only be called when new tab is opened (as part of HTML Report) and error is obtained and you do not wish to display the report.
        /// </remarks>
        /// <returns>Custom Error View.</returns>
        public ActionResult CustomError()
        {
            PageTitle = "Error";

            //var errorToDisplay =  string.IsNullOrEmpty(errorToDisplay) ? "Error has occurred." : errorToDisplay;

            CustomReportErrorViewModel model = new CustomReportErrorViewModel();

            return View("CustomError", model);

        }
    }
}