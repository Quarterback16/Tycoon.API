using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NServiceBus;

namespace BusStop.API
{
   public class WebApiApplication : System.Web.HttpApplication
   {
      public static IBus Bus { get; set; }

      protected void Application_Start()
      {
         AreaRegistration.RegisterAllAreas();
         GlobalConfiguration.Configure( WebApiConfig.Register );
         FilterConfig.RegisterGlobalFilters( GlobalFilters.Filters );
         RouteConfig.RegisterRoutes( RouteTable.Routes );
         BundleConfig.RegisterBundles( BundleTable.Bundles );

         //Bus = Configure.With()
         //   .UseTransport<MsmqTransport>() as IBus;

         Bus = Bus.Create(new BusConfiguration());

         //  This old code is now obsolete
         //Bus = NServiceBus.Configure.With()
         //    .Log4Net()
         //    .DefaultBuilder()
         //    .XmlSerializer()
         //    .MsmqTransport()
         //        .IsTransactional(false)
         //        .PurgeOnStartup(false)
         //    .UnicastBus()
         //        .ImpersonateSender(false)
         //    .CreateBus()
         //    .Start();

      }
   }
}
