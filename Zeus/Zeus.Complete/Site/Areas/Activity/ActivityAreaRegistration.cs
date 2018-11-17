using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Area.Activity
{
	/// <summary>
	/// Defines the Area Registration for Activity.
	/// </summary>
	public class ActivityAreaRegistration : AreaRegistration
	{
		/// <summary>
		/// The name of the area.
		/// </summary>
		public override string AreaName
		{
			get { return "Activity"; }
		}

		/// <summary>
		/// Register the area routes.
		/// </summary>
		public override void RegisterArea(AreaRegistrationContext context)
		{
			IRouteRegistrationService routeRegistration = DependencyResolver.Current.GetService<IRouteRegistrationService>();

			routeRegistration.MapRoute(context, 
				"Activity_default",
				"Activity/{controller}/{action}/{id}",
				new { area = AreaName, controller = "Activity", action = "Index", id = UrlParameter.Optional }
				);
		}
	}
}