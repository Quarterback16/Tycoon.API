using NLog;

namespace EmptyGui
{
   public class FakeWorker : Worker, IDoWork
   {
      public FakeWorker()
      {
         Logger = LogManager.GetCurrentClassLogger();         
      }

      /// <summary>
      ///   Do some work and send one message back to the boss
      /// </summary>
      public string Execute()
      {
         //  put real work here

         const string resultMessage = "Execution complete";

         Logger.Info( resultMessage );

         return resultMessage;
      }
   }
}