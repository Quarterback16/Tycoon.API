using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TrophyWebApi.Data;
using TrophyWebApi.Models;

namespace TrophyWebApi.Controllers
{
   public class CompetitionsController : BaseApiController
   {
      
      public IEnumerable<CompetitionModel> Get()
      {
         // get the data from the repository
         var results = TrophyRepository.GetAllCompetitions()
            .OrderBy( c => c.Name )
            .Take( 25 )
            .ToList()
            .Select( c => ModelFactory.Create( c ) );

         //  return the results
         return results;
      }

      public CompetitionModel Get( int id )
      {
         return ModelFactory.Create( TrophyRepository.GetCompetition( id ) );
      }

   }
}