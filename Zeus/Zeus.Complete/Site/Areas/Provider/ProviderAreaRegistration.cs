using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Area.Provider
{
	/// <summary>
	/// Defines the Area Registration for Provider.
	/// </summary>
	public class ProviderAreaRegistration : AreaRegistration
	{
		/// <summary>
		/// The name of the area.
		/// </summary>
		public override string AreaName
		{
			get { return "Provider"; }
		}

		/// <summary>
		/// Register the area routes.
		/// </summary>
		public override void RegisterArea(AreaRegistrationContext context)
		{
			IRouteRegistrationService routeRegistration = DependencyResolver.Current.GetService<IRouteRegistrationService>();

			routeRegistration.MapRoute(context, 
				"Provider_default",
				"Provider/{controller}/{action}/{id}",
				new { area = AreaName, controller = "Provider", action = "Index", id = UrlParameter.Optional }
				);
		}
	}
}