using System.Collections.Generic;
using System.Web.Http;
using TrophyWebApi.Data;
using TrophyWebApi.Models;
using System.Linq;

namespace TrophyWebApi.Controllers
{
   public class WinnersController : BaseApiController
   {

      public IEnumerable<WinnerModel> Get( int compId )
      {
         var results = TrophyRepository.GetWinnersForCompetition( compId )
            .ToList()
            .Select( w => ModelFactory.Create( w ) );
         return results;
      }

      public WinnerModel Get( int compId, int id )
      {
         var result = ModelFactory.Create( TrophyRepository.GetWinner( id ) );
         return result;
      }
   }
}