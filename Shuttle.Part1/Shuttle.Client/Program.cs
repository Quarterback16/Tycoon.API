using Shuttle.ESB.Core;
using Shuttle.Messages;
using System;

namespace Shuttle.Client
{
   class Program
   {
      static void Main(string[] args)
      {
         using (var bus= ServiceBus.Create().Start() )
         {
            string username;
            while (!string.IsNullOrEmpty(username = Console.ReadLine()))
            {
               bus.Send(new RegisterMemberCommand
                  {
                     UserName = username
                  });
            }

         }
      }
   }
}
