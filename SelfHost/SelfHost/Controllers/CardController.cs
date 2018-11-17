using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SelfHost.Models;

namespace SelfHost.Controllers
{
	public class CardController : ApiController
   {
      //  http://localhost:5000/api/card
      public IEnumerable<Card> Get()
      {
         return Repository.Current.Cards;
      }

      //  http://localhost:5000/api/card/1
      public Card Get( int id )
      {
         return Repository.Current.Cards.FirstOrDefault(c => c.CardId == id);
      }

      public Card Post( Card card )
      {
         return Repository.Current.SaveCard( card );
      }

      public Card Delete( int id )
      {
         return Repository.Current.DeleteCard( id );
      }
   }
}
