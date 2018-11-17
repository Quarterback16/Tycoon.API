using Rebus;
using System;

namespace RebusConsole.Handlers
{
   public class PrintDateTime : IHandleMessages<DateTime>
   {
      public void Handle(DateTime currentDateTime)
      {
         Console.WriteLine("The time is {0}", currentDateTime);
      }
   }
}