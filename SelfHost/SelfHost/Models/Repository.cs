using System.Collections.Generic;

namespace SelfHost.Models
{
   public class Repository
   {
      private readonly Dictionary<int, Card> data;

      static Repository()
      {
         Current = new Repository();
      }

      public static Repository Current { get; private set; }

      public Repository()
      {
         var cards = new Card[]
         {
            new Card() { CardId = 1, Name = "Annoy-o-tron" },
            new Card() { CardId = 2, Name = "Ice Lance"}, 
         };
         data = new Dictionary<int, Card>();
         foreach ( var card in cards )
         {
            data.Add( card.CardId, card );
         }
      }

      public IEnumerable<Card> Cards
      {
         get { return data.Values; }
      }

      public Card GetCard( int id )
      {
         return data[ id ];
      }

      public Card SaveCard( Card newCard )
      {
         newCard.CardId = data.Keys.Count + 1;
         return data[ newCard.CardId ] = newCard;
      }

      public Card DeleteCard( int id )
      {
         var card = data[ id ];
         if (card != null)
         {
            data.Remove( id );
         }
         return card;
      }
   }

}
