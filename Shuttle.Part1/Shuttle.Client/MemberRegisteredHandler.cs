using Shuttle.ESB.Core;
using Shuttle.Messages;
using System;

namespace Shuttle.Client
{
   public class MemberRegisteredHandler : IMessageHandler<MemberRegisteredEvent>
   {
      public void ProcessMessage(HandlerContext<MemberRegisteredEvent> context)
      {
         Console.WriteLine();
         Console.WriteLine("[REPLY ARRIVED] user name = '{0}'", context.Message.UserName);
         Console.WriteLine();
      }

      public bool IsReusable
      {
         get { return true; }
      }
   }
}