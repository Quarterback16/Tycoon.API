using System;
using CQRSlite.Domain;
using CQRSLiteDemo.Domain.Events.Seasons;

namespace CQRSLiteDemo.Domain.WriteModel
{
   public class Season : AggregateRoot
   {
      private string _year;

      public Season( Guid id,
            string year )
      {
         _year = year;

         ApplyChange(
            new SeasonCreatedEvent(
               id, year ));
      }
   }
}
