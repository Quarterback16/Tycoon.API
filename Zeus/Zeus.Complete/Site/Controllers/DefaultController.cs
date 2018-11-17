
//using System.Web;
//
//using System;
//using System.Collections.Generic;
//using System.Diagnostics.CodeAnalysis;
//using System.Web.Mvc;
//using Microsoft.IdentityModel.Protocols.WSFederation;
//using Microsoft.IdentityModel.Web;
//using Employment.Web.Mvc.Infrastructure.Controllers;
//using Employment.Web.Mvc.Infrastructure.DataAnnotations;
//using Employment.Web.Mvc.Infrastructure.Interfaces;
//using Employment.Web.Mvc.Infrastructure.ViewModels;
//using Employment.Web.Mvc.Infrastructure.Types;
//using Employment.Web.Mvc.Infrastructure.Models;
//using Employment.Web.Mvc.Infrastructure.Exceptions;
//using Employment.Web.Mvc.Infrastructure.Extensions;
//using Employment.Web.Mvc.Service.Interfaces.Noticeboard;
//using Employment.Web.Mvc.Zeus.ViewModels;


using System.IdentityModel.Services;
using System.Security;
using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Exceptions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Mappers;
using Employment.Web.Mvc.Infrastructure.Models;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using Employment.Web.Mvc.Service.Interfaces.Noticeboard;
using Employment.Web.Mvc.Zeus.Mappers;
using Employment.Web.Mvc.Zeus.ViewModels;
using System.IdentityModel.Protocols.WSFederation;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Mvc;

namespace Employment.Web.Mvc.Zeus.Controllers
{
    /// <summary>
    /// Defines the default controller for the RHEA application.
    /// </summary>
    [Security(AllowAny = true)]
    public class DefaultController : InfrastructureController
    {
        protected readonly IBulletinService BulletinService;
        protected readonly INoticeboardService NoticeboardService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultController" /> class.
        /// </summary>
        /// <param name="userService">User service for retrieving user data.</param>
        /// <param name="adwService">Adw service for retrieving ADW data.</param>
        /// <param name="bulletinService">Bulletin service for retrieving Bulletin data.</param>
        public DefaultController( IUserService userService, IAdwService adwService, IBulletinService bulletinService, INoticeboardService noticeboardService) : base( userService, adwService) 
        {
            if (bulletinService == null)
            {
                throw new ArgumentNullException("bulletinService");
            }

            BulletinService = bulletinService;

            if (noticeboardService == null)
            {
                throw new ArgumentNullException("noticeboardService");
            }

            NoticeboardService = noticeboardService;
        }



    

        /// <summary>
        /// Home which is the main landing page.
        /// </summary>
        //[Menu("Home")]
        // OutputCache not working in environment
        // http://stackoverflow.com/questions/5371773/asp-net-mvc-outputcache-vary-by-and-vary-by-user-cookie
        // http://blogs.msdn.com/b/tmarq/archive/2008/08/27/using-iis-7-0-dynamic-compression-with-asp-net-output-cache.aspx
        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "user", Location = OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            PageTitle = "Home";

            // Only show noticeboard messages if the user has a Diary role
            var viewModel = new IndexViewModel { ShowNoticeboardMessages = false };//UserService.IsInRole(new [] { "DIA", "DIU", "DIV" })

            if (viewModel.ShowNoticeboardMessages)
            {
                try
                {
                    viewModel.NoticeboardMessages = NoticeboardService.GetSpecificMessages(UserService.SiteCode, UserService.DateTime).ToMessageViewModelList(); //   MappingEngine.Map<IEnumerable<MessageViewModel>>(NoticeboardService.GetSpecificMessages(UserService.SiteCode, UserService.DateTime));
                }
                catch (ServiceValidationException se)
                {
                    ModelStateDictionary modelStateDictionary = new ModelStateDictionary();
                    foreach (var item in se.Errors)
	                {
                        var modelState = new ModelState();
                        modelState.Value = new ValueProviderResult(item.Value, item.Value, System.Globalization.CultureInfo.InvariantCulture);
		                modelStateDictionary.Add(new KeyValuePair<string,ModelState>(item.Key, modelState)) ;
	                }
                    ModelState.Merge(modelStateDictionary);
                }
            }

            return View(viewModel);
        }


        /// <summary>
        /// Details of a bulletin.
        /// </summary>
        //[OutputCache(Duration = 60, VaryByParam = "id") ]
        public ActionResult Bulletin(int id)
        {
            var model = BulletinService.Get(id);

            if (UserService.IsInContract(model.BulletinContracts))
            {
                return View((model).ToBulletinViewModel());
            }
            else
            {
                throw new SecurityException("You don't have access to this bulletin.");
            }
        }

        /// <summary>
        /// List of bulletins.
        /// </summary>
        public ActionResult Bulletins()
        {
            var model = new BulletinsViewModel();

            // Set metadata to be for ESS Web bulletins
            var metadata = new BulletinsPageMetadata { Contract = BulletinType.RJCP };

            // Get all ESS Web bulletins
            var bulletins = BulletinService.List(metadata.Contract, 0);

            // Map pageable instance with page metadata
            model.Bulletins = bulletins.ToBulletinViewModelList(new Pageable<BulletinViewModel>(metadata));//     MappingEngine.Map<IEnumerable<BulletinModel>, IPageable<BulletinViewModel>>(bulletins, new Pageable<BulletinViewModel>(metadata));

            return View(model);
        }

        /// <summary>
        /// Get the next page of bulletins using the page metadata.
        /// </summary>
        /// <param name="metadata">The page metadata required for retrieving the next page.</param>
        /// <returns>The next page.</returns>
        [AjaxOnly]
        public ActionResult BulletinsNextPage(BulletinsPageMetadata metadata)
        {
            // Use page metadata to retrieve bulletins
            var bulletins = BulletinService.List(metadata.Contract, 0);

            // Map pageable instance with page metadata
            var data = bulletins.ToBulletinViewModelList(new Pageable<BulletinViewModel>(metadata));//   MappingEngine.Map<IEnumerable<BulletinModel>, IPageable<BulletinViewModel>>(bulletins, new Pageable<BulletinViewModel>(metadata));

            return PagedView(data, "_PagedBulletins");
        }

        /// <summary>
        /// High contrast.
        /// </summary>
        [RememberPreviousAction]
        public ActionResult HighContrast()
        {
            // Invert whether high contrast should be used
            UserService.UseHighContrast = !UserService.UseHighContrast;

            return RedirectToPreviousAction();
        }

        /// <summary>
        /// Privacy statement.
        /// </summary>
        //[Menu("Privacy")]
        public ActionResult Privacy()
        {
            PageTitle = "Privacy";

            var model = new ContentViewModel()
                .AddTitle("Privacy Statement")
                .AddParagraph("The Department collects information about your use of this system. This includes your User ID, records you access and transactions you perform.")
                .AddParagraph("This information is used by the Department to help with system security, fault reporting, performance monitoring, and audit and fraud prevention.")
                .AddParagraph("If authorised or required by law, details about your use of the ES Web system can be provided to third parties. Third parties can include other government agencies and other organisations. This can be done without your consent.");

            model.Hidden = new[] { LayoutType.LeftHandNavigation, LayoutType.RequiredFieldsMessage };

            return View(model);
        }

        /// <summary>
        /// Accessibility.
        /// </summary>
        //[Menu("Accessibility")]
        public ActionResult Accessibility()
        {
            PageTitle = "Accessibility";

            var model = new ContentViewModel()
                .AddTitle("Accessibility")
                .AddParagraph("The Australian Government is committed to improving on-line services to make them more accessible and usable for all people in our society.")
                .AddSubTitle("System Accessibility")
                .BeginParagraph()
                    .AddText("The system is being built to conform to ")
                    .AddExternalLink("http://www.w3.org/tr/wcag20/", "Web Content Accessibility Guidelines 2.0")
                    .AddText(" (WCAG 2) Level AA.")
                .EndParagraph()
                .AddParagraph("ESS Web relies on:")
                .BeginUnorderedList()
                    .AddListItem("CSS3")
                    .AddListItem("HTML5")
                    .AddListItem("jQuery (JavaScript)")
                    .AddListItem("IE 9 Browser")
                .EndUnorderedList()
                .AddParagraph("The ESS Web system does not yet meet WCAG 2 Level AA. In particular there are accessibility issues with functionality that rely on jQuery.")
                .AddParagraph("We remain focused on achieving conformance with WCAG 2 Level AA and are working to resolve these issues.")
                .AddSubTitle("Contact Us")
                .BeginParagraph()
                    .AddText("We welcome your feedback. If you identify any issues or have trouble using this system, please contact us. You can call our Help Desk on 1300 305 520 or send your comments by email to ")
                    .AddEmailLink("eshelpdesk@employment.gov.au")
                    .AddText(".")
                .EndParagraph();

            model.Hidden = new[] { LayoutType.LeftHandNavigation, LayoutType.RequiredFieldsMessage };

            return View(model);
        }

        /// <summary>
        /// ESS Web System Support.
        /// </summary>
        //[Menu("ESS Web System Support")]
        public ActionResult Support()
        {
            PageTitle = "ESS Web System Support";

            var model = new ContentViewModel()
                .AddTitle("ESS Web System Support")
                .AddSubTitle("Frequently Asked Questions and Reference Materials")
                .BeginParagraph()
                    .AddText("ESS Web system users who have completed the relevant training modules through the ")
                    .AddExternalLink("https://ecsn.gov.au/sites/learningcentre/EmploymentServices/Courses/RJCP/Pages/home.aspx", "Learning Centre")
                    .AddText(" may find additional useful information relating to common questions and system issues in the ")
                    .AddExternalLink("https://ecsn.gov.au/vsm/CustomLogin/VSM.aspx", "Employment Assistant Knowledge Base (EAKB)")
                    .AddText(".")
                .EndParagraph()
                .BeginParagraph()
                    .AddText("To be kept up-to-date on current issues, subscribe to EAKB article ")
                    .AddEmphasisText("3944 - ESS Web Current System Issues")
                    .AddText(".")
                .EndParagraph()
                .AddSubTitle("System Access and Password Related Enquiries")
                .AddParagraph("For issues relating to system access, including user creation, user roles and password resets please contact your Organisation or Site Security Contact (OSC or SSC) in the first instance.")
                .AddSubTitle("System Performance, Errors and Technical Enquiries")
                .AddParagraph("After consulting the EA Knowledge Base and/or liaising with their OSC or SSC if relevant, users may contact the Employment Systems Help Desk via one of the following methods for assistance with system issues:")
                .BeginUnorderedList()
                    .AddListItem("Telephone 1300 305 520 between 8.30 am and 5.00 pm Monday to Friday")
                    .BeginListItem()
                        .AddText("Use the ")
                        .AddEmphasisText("'Ask a Question'")
                        .AddText(" form in the Self Help section of the ")
                        .AddExternalLink("https://ecsn.gov.au/vsm/CustomLogin/VSM.aspx", "EA Knowledge Base")
                    .EndListItem()
                    .BeginListItem()
                        .AddText("Email your query to: ")
                        .AddEmailLink("eshelpdesk@employment.gov.au")
                    .EndListItem()
                .EndUnorderedList()
                .BeginParagraph()
                    .AddText("So that emailed queries and ")
                    .AddEmphasisText("'Ask a Question'")
                    .AddText(" forms can be investigated and resolved quickly and efficiently; it is essential that we receive as much relevant information as possible.")
                .EndParagraph()
                .AddParagraph("Please provide your User ID, contact details and a detailed description of the issue, including:")
                .BeginUnorderedList()
                    .AddListItem("All relevant IDs. i.e.: job seeker, vacancy, employer, activity and/or payment IDs.")
                    .AddListItem("What are you trying to do?")
                    .AddListItem("The number of users experiencing the issue (only you? or some/most/all users at your site)")
                    .AddListItem("Where in the system you are experiencing difficulty? (such as which screen)")
                    .AddListItem("The exact wording of the any error message you receive.")
                    .AddListItem("A clear screen shot, showing any error message and where/how it is displayed.")
                    .AddListItem("Details of any EAKB articles, Learning Centre modules, Online Help files or Provider Portal articles relevant to your issue/enquiry.")
                .EndUnorderedList()
                .AddSubTitle("Policy Enquiries")
                .BeginParagraph()
                    .AddText("Policy enquiries should be submitted via the ")
                    .AddEmphasisText("Question Manager")
                    .AddText(" tool within the ECSN ")
                    .AddExternalLink("https://ecsn.gov.au/sites/securesiteportal/Pages/HomePage.aspx", "Provider Portal")
                    .AddText(". Please note: only users assigned the general role SCO - Site Contact Officer are able to access ")
                    .AddEmphasisText("Question Manager")
                    .AddText(". Please contact your OSC if you require this role.")
                .EndParagraph()
                .AddSubTitle("Training Enquiries")
                .BeginParagraph()
                    .AddText("Unfortunately, the ES Help Desk is unable to assist with training in the use of the Department Systems. If you require further training, please refer to the ESS Web training modules in the ")
                    .AddExternalLink("https://ecsn.gov.au/sites/LearningCentre/Pages/home.aspx", "Learning Centre")
                    .AddText(" > Employment Services > Courses.")
                .EndParagraph();

            model.Hidden = new[] { LayoutType.LeftHandNavigation, LayoutType.RequiredFieldsMessage };

            return View(model);
        }

        /// <summary>
        /// STS result
        /// </summary>
        /// <remarks>
        /// Excluded from code coverage as the internals of <see cref="FederatedAuthentication.WSFederationAuthenticationModule" /> are accessing HttpContext.Current, outside of ControllerContext.HttpContext, which is not able to be mocked. 
        /// </remarks>
        [ExcludeFromCodeCoverage]
        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult FederationResult()
        {
            var fam = FederatedAuthentication.WSFederationAuthenticationModule;

            var wrapper = new HttpRequestWrapper(System.Web.HttpContext.Current.Request);
            if (fam.CanReadSignInResponse(wrapper, true))
            {
                string returnUrl = fam.GetSignInResponseMessage(wrapper).Context;

                return new RedirectResult(returnUrl);
            }

            return View("Index");
        }

        /// <summary>
        /// Logout of STS
        /// </summary>
        /// <remarks>
        /// Excluded from code coverage as the internals of <see cref="FederatedAuthentication.WSFederationAuthenticationModule" /> are accessing HttpContext.Current, outside of ControllerContext.HttpContext, which is not able to be mocked. 
        /// </remarks>
        [ExcludeFromCodeCoverage]
        public ActionResult Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("Index");
            }
            
            FederatedAuthentication.WSFederationAuthenticationModule.SignOut(false);
            
            var signOut = new SignOutRequestMessage(new Uri(FederatedAuthentication.WSFederationAuthenticationModule.Issuer), FederatedAuthentication.WSFederationAuthenticationModule.Realm);

            if (Request.Cookies.Get("UserDateTime") != null)
            {
                Response.Cookies.Set(new HttpCookie("UserDateTime") {Expires = DateTime.Now.AddDays(-1)});
            }

            UserService.Session.Abandon();

            return Redirect(signOut.WriteQueryString());
        }

        /// <summary>
        /// Login to STS
        /// </summary>
        /// <remarks>
        /// Excluded from code coverage as the internals of <see cref="FederatedAuthentication.WSFederationAuthenticationModule" /> are accessing HttpContext.Current, outside of ControllerContext.HttpContext, which is not able to be mocked. 
        /// </remarks>
        [ExcludeFromCodeCoverage]
        public ActionResult Logon()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("Index");
            }

            var fam = FederatedAuthentication.WSFederationAuthenticationModule;

            var signIn = new SignInRequestMessage(new Uri(fam.Issuer), fam.Realm)
            {
                Context = fam.Realm
            };

            return Redirect(signIn.WriteQueryString());
        }
    }
}
