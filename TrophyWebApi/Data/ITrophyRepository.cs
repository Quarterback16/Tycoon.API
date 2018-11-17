using System.Collections.Generic;
using TrophyDataModel;
using TrophyWebApi.Models;

namespace TrophyWebApi.Data
{
   public interface ITrophyRepository
   {
      ICollection<Competition> GetAllCompetitions();
      Competition GetCompetition( int id );

      ICollection<Winner> GetWinnersForCompetition( int compId );

      Winner GetWinner( int id );
   }
}