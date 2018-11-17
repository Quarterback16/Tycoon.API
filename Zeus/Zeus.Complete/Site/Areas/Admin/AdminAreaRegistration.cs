using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Area.Admin
{
	/// <summary>
	/// Defines the Area Registration for Admin.
	/// </summary>
	public class AdminAreaRegistration : AreaRegistration
	{
		/// <summary>
		/// The name of the area.
		/// </summary>
		public override string AreaName
		{
			get { return "Admin"; }
		}

	    /// <summary>
	    /// Register the area routes.
	    /// </summary>
	    public override void RegisterArea(AreaRegistrationContext context)
	    {
            IRouteRegistrationService routeRegistration = DependencyResolver.Current.GetService<IRouteRegistrationService>();




            //context.MapRoute("Adm_AdwLookup_default", "Adm/AdwLookup", new { area = "Admin", controller = "AdwLookup", action = "ListCodeType"});

            //context.MapRoute("Admin_AdwLookup_default", "Admin/AdwLookup/", new { area = "Admin", controller = "AdwLookup", action = "ListCodeType" });


            routeRegistration.MapRoute(context, "Admin_Elmah", "Admin/Elmah/{resource}", new { area = "Admin", controller = "Elmah", action = "Index", resource = UrlParameter.Optional });

            routeRegistration.MapRoute(context, "Admin_Elmah_Index", "Admin/Elmah/Index/{resource}", new { area = "Admin", controller = "Elmah", action = "Index", resource = UrlParameter.Optional });



            //context.MapRoute("Adm_default", "Adm/{controller}/{action}/{id}", new { area = "Admin", controller = "Default", action = "Index", id = UrlParameter.Optional });
            routeRegistration.MapRoute(context, "Admin_default", "Admin/{controller}/{action}/{id}", new { area = "Admin", controller = "Default", action = "Index", id = UrlParameter.Optional });


	    }
	}
}