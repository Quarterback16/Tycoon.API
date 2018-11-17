using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;
using System.Web.Http;
using ebAPISelfHost;

namespace WebAPISelfHost
{
   class Program
   {
      static void Main(string[] args)
      {
         var config = new HttpSelfHostConfiguration("http:/localhost:8999");
         config.Routes.MapHttpRoute("default",
            "api/{controller}/{id}", 
            new { id = RouteParameter.Optional });

         var server = new HttpSelfHostServer(config);
            //, new MySimpleHttpMessageHandler());
         var task = server.OpenAsync();
         task.Wait();
         Console.WriteLine("Server is up and running");
         Console.ReadLine();
      }
   }
}
