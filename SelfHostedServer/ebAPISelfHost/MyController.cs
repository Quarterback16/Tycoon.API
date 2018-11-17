using System.Web.Http;

namespace ebAPISelfHost
{
	public class MyController :ApiController
   {
      public string Get()
      {
         return "Hello from my Controller in Self Hosting";
      }
   }
}
