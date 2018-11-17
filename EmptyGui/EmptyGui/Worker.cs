using System;
using NLog;

namespace EmptyGui
{
   public abstract class Worker
   {
      public Logger Logger { get; set; }

      protected Worker()
      {
      }

      public void ReportProgress( string message )
      {
         
      }
   }
}
