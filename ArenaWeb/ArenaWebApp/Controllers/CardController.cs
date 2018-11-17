using System.Collections.Generic;
using System.Web.Http;
using ArenaWebApp.Models;

namespace ArenaWebApp.Controllers
{
    public class CardController : ApiController
    {
       public IEnumerable<Card> GetCards()
       {
          return Repository.Cards;   //.Where( x => x.WillAttend == true );
       }

       public void PostCard( Card card )
       {
          if ( ModelState.IsValid )
          {
             Repository.Add( card );
          }
       }
    }
}
