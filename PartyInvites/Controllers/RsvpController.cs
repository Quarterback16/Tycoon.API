using PartyInvites.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;  //  not MVC!!

namespace PartyInvites.Controllers
{
   /// <summary>
   ///   a web service that is capable of delivering data over HTTP!!
   /// </summary>
   public class RsvpController : ApiController
   {
      /// <summary>
      ///   Get a collection of Attendees 
      /// </summary>
      /// <returns>data (not an ActionResult)</returns>
      public IEnumerable<GuestResponse> GetAttendees()
      {
         return Repository.Responses.Where( x => x.WillAttend == true );
      }

      public void PostResponse( GuestResponse response )
      {
         if ( ModelState.IsValid )
         {
            Repository.Add( response );
         }
      }
   }
}