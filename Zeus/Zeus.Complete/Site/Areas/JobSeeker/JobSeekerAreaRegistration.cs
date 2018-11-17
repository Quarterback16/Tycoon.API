using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Area.JobSeeker
{
	/// <summary>
	/// Defines the Area Registration for JobSeeker.
	/// </summary>
	public class JobSeekerAreaRegistration : AreaRegistration
	{
		/// <summary>
		/// The name of the area.
		/// </summary>
		public override string AreaName
		{
			get { return "JobSeeker"; }
		}

		/// <summary>
		/// Register the area routes.
		/// </summary>
		public override void RegisterArea(AreaRegistrationContext context)
		{
			IRouteRegistrationService routeRegistration = DependencyResolver.Current.GetService<IRouteRegistrationService>();

			routeRegistration.MapRoute(context, 
				"JobSeeker_default",
				"JobSeeker/{controller}/{action}/{id}",
				new { area = AreaName, controller = "JobSeeker", action = "Index", id = UrlParameter.Optional }
				);
		}
	}
}