using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Area.Employer
{
	/// <summary>
	/// Defines the Area Registration for Employer.
	/// </summary>
	public class EmployerAreaRegistration : AreaRegistration
	{
		/// <summary>
		/// The name of the area.
		/// </summary>
		public override string AreaName
		{
			get { return "Employer"; }
		}

		/// <summary>
		/// Register the area routes.
		/// </summary>
		public override void RegisterArea(AreaRegistrationContext context)
		{
			IRouteRegistrationService routeRegistration = DependencyResolver.Current.GetService<IRouteRegistrationService>();

			routeRegistration.MapRoute(context, 
				"Employer_default",
				"Employer/{controller}/{action}/{id}",
				new { area = AreaName, controller = "Employer", action = "Index", id = UrlParameter.Optional }
				);
		}
	}
}