using System;

namespace CQRSLiteDemo.Domain.Commands
{
   public class CreateSeasonCommand : BaseCommand
   {
      public readonly string Year;

      public CreateSeasonCommand( Guid id, string year)
      {
         Id = id;
         Year = year;
      }
   }
}
