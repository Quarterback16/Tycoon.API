using CQRSLiteDemo.Domain.ReadModel.Repositories.Interfaces;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;

namespace CQRSLiteDemo.Domain.ReadModel.Repositories
{
   public class SeasonRepository : BaseRepository, ISeasonRepository
   {
      public SeasonRepository(
           IConnectionMultiplexer redisConnection 
         ): base(redisConnection, "season")
      {
      }

      public IEnumerable<SeasonRM> GetAll()
      {
         return Get<List<SeasonRM>>( "all" );
      }

      public SeasonRM GetByID( int id )
      {
         return Get<SeasonRM>( id );
      }

      public List<SeasonRM> GetMultiple( List<int> ids )
      {
         return GetMultiple<SeasonRM>( ids );
      }

      public void Save( SeasonRM season )
      {
         Save( season.Year, season );
         MergeIntoAllCollection( season );
      }

      private void MergeIntoAllCollection( SeasonRM season )
      {
         var allSeasons = new List<SeasonRM>();
         if ( Exists( "all" ) )
         {
            allSeasons = Get<List<SeasonRM>>( "all" );
         }

         // If the season already exists in the ALL collection, 
         // remove that entry
         if ( allSeasons.Any( x => x.Year == season.Year ) )
         {
            allSeasons.Remove( allSeasons.First( x => x.Year == season.Year ) );
         }

         //Add the modified district to the ALL collection
         allSeasons.Add( season );

         Save( "all", allSeasons );
      }
   }
}
