using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrophyDataModel;
using TrophyWebApi.Models;

namespace TrophyWebApi.Data
{
   public class TrophyRepository : ITrophyRepository
   {
      public ICollection<Competition> GetAllCompetitions()
      {
         var list = new List<Competition>();
         var winners = new List<Winner>();
         winners.Add( new Winner { Name = "Brazil", When = "1980" });
         list.Add( new Competition
         {
            Id = 1,
            Name = "World Cup",
            Frequency = "every 4 years",
            //InauguralYear = 1920,
            Winners = winners
         } );
         list.Add( new Competition
         {
            Id = 2,
            Name = "Serie A",
            Frequency = "every year"
         }
         );
         return list;
      }

      public Competition GetCompetition( int id )
      {
         var winners = new List<Winner>();
         winners.Add( new Winner { Name = "Brazil", When = "1980" });
         return new Competition
         {
            Id = 1,
            Name = "World Cup",
            Frequency = "every 4 years",
            //InauguralYear = 1920,
            Winners = winners
         };

      }

      public ICollection<Winner> GetWinnersForCompetition( int compId )
      {
         var winners = new List<Winner>();
         winners.Add( new Winner { Name = "Brazil", When = "1980" } );
         return winners;
      }


      public Winner GetWinner( int id )
      {
         return new Winner { Name = "Brazil", When = "1980" };
      }
   }
}