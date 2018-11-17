using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NServiceBus;
using NServiceBus.Installation.Environments;

namespace ExampleWeb
{
	public static class ServiceBus
	{
		public static IBus Bus { get; private set; }
      private static readonly object padlock = new object();

		public static void Init()
		{
			if (Bus != null)
				return;

			lock (padlock)
			{
				if (Bus != null)
					return;

			   var config = new BusConfiguration();
			   config.UseTransport<MsmqTransport>();
			   config.UsePersistence<InMemoryPersistence>();
            config.EndpointName( "ExampleWeb" );
            config.PurgeOnStartup( true );
            config.EnableInstallers(  );

			   Bus = NServiceBus.Bus.Create( config ).Start();
			   //Bus = Configure.With()
			   //   .DefineEndpointName("ExampleWeb")
			   //   .DefaultBuilder()
			   //   .UseTransport<Msmq>()
			   //   .PurgeOnStartup(true)
			   //   .UnicastBus()
			   //   .CreateBus()
			   //   .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());
			}
		}
	}
}
