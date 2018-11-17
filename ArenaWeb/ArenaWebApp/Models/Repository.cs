using System.Collections.Generic;

namespace ArenaWebApp.Models
{
   public class Repository
   {
      private static readonly Dictionary<string, Card> cards;

      static Repository()
      {
         cards = new Dictionary<string, Card>
         {
            {
               "Grim Patron", new Card
               {
                  Name = "Grim Patron",
                  Mana = 5,
                  Category = "Neutral"
               }
            },
            {
               "Frothing Beserker", new Card
               {
                  Name = "Frothing Beserker",
                  Mana = 3,
                  Category = "Warrior"
               }
            },
            {
               "Flamestrike", new Card
               {
                  Name = "Flamestrike",
                  Mana = 7,
                  Category = "Neutral"
               }
            }
         };
      }

      public static void Add( Card newCard )
      {
         var key = newCard.Name.ToLowerInvariant();
         if ( cards.ContainsKey( key ) )
         {
            cards[ key ] = newCard;
         }
         else
         {
            cards.Add( key, newCard );
         }
      }

      public static IEnumerable<Card> Cards
      {
         get { return cards.Values; }
      }
   }
}