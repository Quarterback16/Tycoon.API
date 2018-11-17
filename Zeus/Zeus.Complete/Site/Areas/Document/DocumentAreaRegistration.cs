using System.Web.Mvc;
using Employment.Web.Mvc.Infrastructure.Interfaces;

namespace Employment.Web.Mvc.Area.Document
{
	/// <summary>
	/// Defines the Area Registration for Document.
	/// </summary>
	public class DocumentAreaRegistration : AreaRegistration
	{
		/// <summary>
		/// The name of the area.
		/// </summary>
		public override string AreaName
		{
			get { return "Document"; }
		}

		/// <summary>
		/// Register the area routes.
		/// </summary>
		public override void RegisterArea(AreaRegistrationContext context)
		{
			IRouteRegistrationService routeRegistration = DependencyResolver.Current.GetService<IRouteRegistrationService>();

			routeRegistration.MapRoute(context, 
				"Document_default",
				"Document/{controller}/{action}/{id}",
				new { area = AreaName, controller = "Document", action = "Index", id = UrlParameter.Optional }
				);
		}
	}
}