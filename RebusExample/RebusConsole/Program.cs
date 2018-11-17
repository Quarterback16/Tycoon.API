using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebus.Configuration;
using Rebus.Transports.Msmq;

namespace RebusConsole
{
   class Program
   {
      static void Main(string[] args)
      {
         // we have the container in a variable, but you would probably stash it in a static field somewhere
         using (var adapter = new BuiltinContainerAdapter())
         {
            Configure.With(adapter)
                .Transport(t => t.UseMsmqAndGetInputQueueNameFromAppConfig())
                .MessageOwnership(d => d.FromRebusConfigurationSection())
                .CreateBus().Start();

            adapter.Register(typeof(Handlers.PrintDateTime));

            var timer = new System.Timers.Timer();
            timer.Elapsed += delegate { adapter.Bus.SendLocal(DateTime.Now); };
            timer.Interval = 1000;
            timer.Start();

            Console.WriteLine("Press enter to quit");
            Console.ReadLine();
         } //< always dispose bus when your app quits - here done via the container adapter
      }
   }
}
