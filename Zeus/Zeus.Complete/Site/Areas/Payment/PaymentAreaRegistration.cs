using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Area.Payment
{
	/// <summary>
	/// Defines the Area Registration for Payment.
	/// </summary>
	public class PaymentAreaRegistration : AreaRegistration
	{
		/// <summary>
		/// The name of the area.
		/// </summary>
		public override string AreaName
		{
			get { return "Payment"; }
		}

		/// <summary>
		/// Register the area routes.
		/// </summary>
		public override void RegisterArea(AreaRegistrationContext context)
		{
			IRouteRegistrationService routeRegistration = DependencyResolver.Current.GetService<IRouteRegistrationService>();

			routeRegistration.MapRoute(context, 
				"Payment_default",
				"Payment/{controller}/{action}/{id}",
				new { area = AreaName, controller = "Payment", action = "Index", id = UrlParameter.Optional }
				);
		}
	}
}