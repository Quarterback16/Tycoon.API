using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Area.Vacancy
{
	/// <summary>
	/// Defines the Area Registration for Vacancy.
	/// </summary>
	public class VacancyAreaRegistration : AreaRegistration
	{
		/// <summary>
		/// The name of the area.
		/// </summary>
		public override string AreaName
		{
			get { return "Vacancy"; }
		}

		/// <summary>
		/// Register the area routes.
		/// </summary>
		public override void RegisterArea(AreaRegistrationContext context)
		{
			IRouteRegistrationService routeRegistration = DependencyResolver.Current.GetService<IRouteRegistrationService>();

			routeRegistration.MapRoute(context, 
				"Vacancy_default",
				"Vacancy/{controller}/{action}/{id}",
				new { area = AreaName, controller = "Vacancy", action = "Index", id = UrlParameter.Optional }
				);
		}
	}
}