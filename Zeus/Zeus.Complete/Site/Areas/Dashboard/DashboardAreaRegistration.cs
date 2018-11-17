using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Area.Dashboard
{
	/// <summary>
	/// Defines the Area Registration for Dashboard.
	/// </summary>
	public class DashboardAreaRegistration : AreaRegistration
	{
		/// <summary>
		/// The name of the area.
		/// </summary>
		public override string AreaName
		{
			get { return "Dashboard"; }
		}

		/// <summary>
		/// Register the area routes.
		/// </summary>
		public override void RegisterArea(AreaRegistrationContext context)
		{
			IRouteRegistrationService routeRegistration = DependencyResolver.Current.GetService<IRouteRegistrationService>();

			routeRegistration.MapRoute(context, 
				"Dashboard_default",
				"Dashboard/{controller}/{action}/{id}",
				new { area = AreaName, controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
				);
		}
	}
}