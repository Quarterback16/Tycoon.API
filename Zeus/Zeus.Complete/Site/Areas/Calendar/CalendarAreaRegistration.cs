using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Area.Calendar
{
	/// <summary>
	/// Defines the Area Registration for Calendar.
	/// </summary>
	public class CalendarAreaRegistration : AreaRegistration
	{
		/// <summary>
		/// The name of the area.
		/// </summary>
		public override string AreaName
		{
			get { return "Calendar"; }
		}

		/// <summary>
		/// Register the area routes.
		/// </summary>
		public override void RegisterArea(AreaRegistrationContext context)
		{
			IRouteRegistrationService routeRegistration = DependencyResolver.Current.GetService<IRouteRegistrationService>();

			routeRegistration.MapRoute(context, 
				"Calendar_default",
				"Calendar/{controller}/{action}/{id}",
				new { area = AreaName, controller = "Calendar", action = "Index", id = UrlParameter.Optional }
				);
		}
	}
}