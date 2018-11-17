using System.Web.Optimization;
using System.Web.Routing;
using ProgramAssuranceTool.Infrastructure.Ioc.Unity;

namespace ProgramAssuranceTool
{
	public class PatApplication : UnityMvcApplication
	{
		//  Replaced with IRegister classes, Application_Start has been overriden 

		protected new void Application_Start()
		{
			RouteConfig.RegisterRoutes( RouteTable.Routes );
			BundleConfig.RegisterBundles( BundleTable.Bundles );
		}
	}
}