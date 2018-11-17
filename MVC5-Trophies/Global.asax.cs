using MVC5_Trophies.App_Start;
using MVC5_Trophies.Infrastructure;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;

namespace MVC5_Trophies
{
   public class MvcApplication : System.Web.HttpApplication
   {
      protected void Application_Start()
      {
         AreaRegistration.RegisterAllAreas();

         //  Web API should have put this line in (and class)?
         //WebApiConfig.Register( GlobalConfiguration.Configuration );

         FilterConfig.RegisterGlobalFilters( GlobalFilters.Filters );
         RouteConfig.RegisterRoutes( RouteTable.Routes );
         BundleConfig.RegisterBundles( BundleTable.Bundles );

         //  turn off the response header (X-AspNetMvc-Version:5.0) (not reqd)
         MvcHandler.DisableMvcResponseHeader = true;

         Profiler.Initialize();
      }

      // This removes the Response header - "Server	Microsoft-IIS/8.0" (not needed)
      protected void Application_PreSendRequestHeaders( object sender, EventArgs e )
      {
         HttpApplication app = sender as HttpApplication;
         if ( app != null &&
             app.Context != null )
         {
            app.Context.Response.Headers.Remove( "Server" );
         }
      }

      /// <summary>
      /// The event when the application acquires request state.
      /// </summary>
      /// <param name="sender">
      /// The sender.
      /// </param>
      /// <param name="e">
      /// The event argument..
      /// </param>
      protected void Application_AcquireRequestState( object sender, EventArgs e )
      {
         Profiler.Start( HttpContext.Current );
      }

      /// <summary>
      /// This function is called by ASP .NET at the end of every http request.
      /// </summary>
      /// <param name="sender">
      /// The sender.
      /// </param>
      /// <param name="e">
      /// The event argument.
      /// </param>
      protected void Application_EndRequest( object sender, EventArgs e )
      {
         Profiler.Stop();
      }
   }
}