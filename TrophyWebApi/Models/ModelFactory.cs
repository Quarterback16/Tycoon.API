using TrophyDataModel;
using System.Linq;

namespace TrophyWebApi.Models
{
   /// <summary>
   ///   One place where all the mapping aka parsing takes place, contrast vs AutoMapper
   /// </summary>
   public class ModelFactory
   {
      public CompetitionModel Create( Competition competition )
      {
         return new CompetitionModel
         {
            Name = competition.Name,
            Frequency = competition.Frequency,
            Winners = competition.Winners.Select( w => Create( w ) ).ToList()
         };
      }

      public WinnerModel Create( Winner winner )
      {
         return new WinnerModel
         {
            Name = winner.Name,
            When = winner.When

         };
      }
   }
}