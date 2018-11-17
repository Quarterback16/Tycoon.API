using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Area.Fund
{
	/// <summary>
	/// Defines the Area Registration for Fund.
	/// </summary>
	public class FundAreaRegistration : AreaRegistration
	{
		/// <summary>
		/// The name of the area.
		/// </summary>
		public override string AreaName
		{
			get { return "Fund"; }
		}

		/// <summary>
		/// Register the area routes.
		/// </summary>
		public override void RegisterArea(AreaRegistrationContext context)
		{
			IRouteRegistrationService routeRegistration = DependencyResolver.Current.GetService<IRouteRegistrationService>();

			routeRegistration.MapRoute(context, 
				"Fund_default",
				"Fund/{controller}/{action}/{id}",
				new { area = AreaName, controller = "Fund", action = "Index", id = UrlParameter.Optional }
				);
		}
	}
}