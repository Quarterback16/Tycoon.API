using System;

namespace CQRSLiteDemo.Domain.ReadModel
{
   public class SeasonRM
   {
      public Guid AggregateID { get; set; }
      public string Year { get; set; }
   }
}
