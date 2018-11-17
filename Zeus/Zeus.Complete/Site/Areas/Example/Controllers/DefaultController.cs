using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using Employment.Web.Mvc.Area.Example.Mappers;
using Employment.Web.Mvc.Area.Example.Models;
using Employment.Web.Mvc.Area.Example.ViewModels;
using Employment.Web.Mvc.Infrastructure.Controllers;
using Employment.Web.Mvc.Infrastructure.DataAnnotations;
using Employment.Web.Mvc.Infrastructure.Extensions;
using Employment.Web.Mvc.Infrastructure.Interfaces;
using Employment.Web.Mvc.Infrastructure.Types;
using Employment.Web.Mvc.Infrastructure.ViewModels;
using Employment.Web.Mvc.Infrastructure.ViewModels.Dynamic;

namespace Employment.Web.Mvc.Area.Example.Controllers
{
	/// <summary>
	/// Defines the DefaultController controller.
	/// </summary>
	[Security(AllowAny = true)]
    public class DefaultController : InfrastructureController
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="DefaultController" /> class.
        /// </summary>
        /// <param name="mappingEngine">AutoMapper mapping engine for mapping between types.</param>
        /// <param name="userService">User service for retrieving user data.</param>
        /// <param name="adwService">Adw service for retrieving ADW data.</param>
        public DefaultController( IUserService userService, IAdwService adwService) : base( userService, adwService) { }

        /// <summary>
        /// Index action.
        /// </summary>
        public ActionResult Index()
        {
            var model = new ContentViewModel()
                .AddTitle("Examples")
                .AddParagraph("This area contains examples demonstrating how the Infrastructure data annotations can be used for defining how your View Models are validated and rendered in the View.")

                // Area landing page links
                .AddAreaLink(AreaLinkIconType.Default, "Index", "BasicFormElements", "Basic form controls")
                .AddAreaLink(AreaLinkIconType.Default, "Index", "Validation", "Validation of controls")
                .AddAreaLink(AreaLinkIconType.Default, "Index", "ServerSideElements", "Server side form controls")
                .AddAreaLink(AreaLinkIconType.Default, "Index", "Layout", "Laying out pages")
                .AddAreaLink(AreaLinkIconType.Default, "Index", "PowerComponents", "Power components")
                .AddAreaLink(AreaLinkIconType.Default, "Index", "DataAccess", "Data access/mapping")
                .AddAreaLink(AreaLinkIconType.Default, "Index", "Tables", "Grids and tables")
                .AddAreaLink(AreaLinkIconType.Default, "Index", "Report", "Report examples")
                .AddAreaLink(AreaLinkIconType.Default, "Index", "Workflow", "Workflow examples")
                ;

            // Hide left hand navigation on area landing page.
            model.Hidden = new[] {LayoutType.LeftHandNavigation};
            
            return View(model);
        }

        /// <summary>
        /// Demonstrates how to have an action appear in a fake area.  See <see cref="ExampleAreaRegistration" /> route registration for setup of fake area's default route.
        /// </summary>
        /// <remarks>
        /// Notice that the Menu attribute must have the Area property set to the name of the fake area.
        /// </remarks>
        [Menu("My fake area", Area = "Fake")]
        public ActionResult FakeArea()
        {
            var model = new ContentViewModel();

            model.AddTitle("My fake area");

            return View(model);
        }

    }
}
