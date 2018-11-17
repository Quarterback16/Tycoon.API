using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Area.Transition
{
	/// <summary>
	/// Defines the Area Registration for Transition.
	/// </summary>
	public class TransitionAreaRegistration : AreaRegistration
	{
		/// <summary>
		/// The name of the area.
		/// </summary>
		public override string AreaName
		{
			get { return "Transition"; }
		}

		/// <summary>
		/// Register the area routes.
		/// </summary>
		public override void RegisterArea(AreaRegistrationContext context)
		{
            IRouteRegistrationService routeRegistration = DependencyResolver.Current.GetService<IRouteRegistrationService>();

            routeRegistration.MapRoute(context,
				"Transition_default",
				"Transition/{action}/{id}",
				new { area = AreaName, controller = "Transition", action = "Index", id = UrlParameter.Optional }
				);
		}
	}
}