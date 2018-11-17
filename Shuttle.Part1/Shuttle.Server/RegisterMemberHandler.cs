using System;
using Shuttle.ESB.Core;
using Shuttle.Messages;

namespace Shuttle.Server
{
   public class RegisterMemberHandler : IMessageHandler<RegisterMemberCommand>
   {
      public void ProcessMessage(HandlerContext<RegisterMemberCommand> context)
      {
         Console.WriteLine();
         Console.WriteLine("[MEMBER REGISTERED] : user name = '{0}'", context.Message.UserName );
         Console.WriteLine();

         var memberRegisteredEvent = new MemberRegisteredEvent
         {
            UserName = context.Message.UserName
         };

         context.Publish(memberRegisteredEvent);

         if (!string.IsNullOrEmpty(context.TransportMessage.SenderInboxWorkQueueUri))
         {
            context.Send(new MemberRegisteredEvent
            {
               UserName = context.Message.UserName
            }, c =>
            {
               c.Reply();
            });
         }
      }

      public bool IsReusable
      {
         get { return true; }
      }
   }
}
