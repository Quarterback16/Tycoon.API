using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Area.Contract
{
	/// <summary>
	/// Defines the Area Registration for Contract.
	/// </summary>
	public class ContractAreaRegistration : AreaRegistration
	{
		/// <summary>
		/// The name of the area.
		/// </summary>
		public override string AreaName
		{
			get { return "Contract"; }
		}

		/// <summary>
		/// Register the area routes.
		/// </summary>
		public override void RegisterArea(AreaRegistrationContext context)
		{
			IRouteRegistrationService routeRegistration = DependencyResolver.Current.GetService<IRouteRegistrationService>();

			routeRegistration.MapRoute(context, 
				"Contract_default",
				"Contract/{controller}/{action}/{id}",
				new { area = AreaName, controller = "Contract", action = "Index", id = UrlParameter.Optional }
				);
		}
	}
}