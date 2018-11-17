using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using TrophyWebApi.App_Start;

namespace TrophyWebApi
{
   public static class WebApiConfig
   {
      public static void Register( HttpConfiguration config )
      {
         // Web API configuration and services

         // Web API routes
         config.MapHttpAttributeRoutes();

         config.Routes.MapHttpRoute(
             name: "Competition",
             routeTemplate: "api/trophy/Competitions/{compid}",
             defaults: new { controller = "Competitions", compid = RouteParameter.Optional },
             constraints: new {  id = "/d+" }
         );

         config.Routes.MapHttpRoute(
             name: "Winners",
             routeTemplate: "api/trophy/Competitions/{compid}/Winners/{id}",
             defaults: new { controller = "Winners", id = RouteParameter.Optional }
         );

         var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();
         jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
      }
   }
}