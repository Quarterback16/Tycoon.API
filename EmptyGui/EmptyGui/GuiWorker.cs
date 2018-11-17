using NLog;
using System;
using System.ComponentModel;
using System.Threading;

namespace EmptyGui
{
   /// <summary>
   ///   A class that does something while playing nicely with its controlling GUI
   /// </summary>
   public class GuiWorker
   {
      private readonly IDoWork RealWorker;

      public GuiWorker( string version, IDoWork realWorker )
      {
         Version = version;
         RealWorker = realWorker;
      }

      public string Version { get; set; }

      public Logger Logger { get; set; }

      public bool Verbose { get; set; }

      public int Passes { get; set; }

      public BackgroundWorker MyWorker { get; set; }

      public int Pollinterval { get; set; }


      public void ReportProgress( string message )
      {
         ReportProgress( message, 50 );
      }

      public void ReportProgress( string message, int progress )
      {
         if ( MyWorker != null )
            MyWorker.ReportProgress( progress, message );
      }

      public void Go( BackgroundWorker backgroundWorker1, DoWorkEventArgs e )
      {
         try
         {
            MyWorker = backgroundWorker1;


            if ( Passes == 0 )
               ReportProgress( string.Format( "{0} - Starting...", Version ), 99 );

            while ( true )
            {
               Passes++;

               var result = RealWorker.Execute();

               ReportProgress( result, 99 );

               ReportProgress( 
                  string.Format(
                  "Pass Number {0} done - next pass ({1}) {2:HH:mm}",
                  Passes, Pollinterval, DateTime.Now.AddMinutes( Pollinterval ) ), 99 );
               Thread.Sleep( Pollinterval * 60 * 1000 ); //  minutes

               if ( !MyWorker.CancellationPending ) continue;

               e.Cancel = true;
               break;
            }
         }
         catch ( Exception ex )
         {
            if ( Logger == null ) Logger = LogManager.GetCurrentClassLogger();
            Logger.Error( ex.Message );
            Logger.Error( ex.StackTrace );
            throw;
         }
      }
   }
}
