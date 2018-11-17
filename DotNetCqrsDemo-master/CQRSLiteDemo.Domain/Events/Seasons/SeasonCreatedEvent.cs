using System;

namespace CQRSLiteDemo.Domain.Events.Seasons
{

   public class SeasonCreatedEvent : BaseEvent
   {
      public readonly string Year;

      public SeasonCreatedEvent( Guid id,
            string year )
      {
         Id = id;
         Year = year;
      }
   }
  

}
