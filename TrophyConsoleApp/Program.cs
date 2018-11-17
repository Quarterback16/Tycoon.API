using System;
using TrophyDataModel;

namespace TrophyConsoleApp
{
   internal class Program
   {
      private static void Main( string[] args )
      {
         using ( var dbContext = new TrophyEntities() )
         {
            var comp = new Competition
               {
                  Name = "World Cup",
                  Frequency = "Every 4 years"
               };
            dbContext.Competitions.Add( comp );
            dbContext.SaveChanges();
            Console.WriteLine( "EF Model successfully used in another .NET project." );
            Console.ReadKey();
         }
      }
   }
}