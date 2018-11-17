using System;
using Shuttle.Core.Host;
using Shuttle.Core.Infrastructure;
using Shuttle.ESB.Core;
using Shuttle.ESB.SqlServer;
using Shuttle.Messages;

namespace Shuttle.Subscriber
{
    public class Host : IHost, IDisposable
    {
       private IServiceBus _bus;

       public void Start()
       {
          Log.Assign(new ConsoleLog(GetType()) {LogLevel = LogLevel.Trace});

          ISubscriptionManager subscriptionManager = SubscriptionManager.Default();
          subscriptionManager.Subscribe( new[] {typeof (MemberRegisteredEvent).FullName } );

    );
          _bus = ServiceBus.Create( c => c.SubscriptionManager(subscriptionManager)).Start();
       }

       public void Dispose()
       {
          _bus.Dispose();
       }
    }
}
