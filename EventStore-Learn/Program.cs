using EventStore.ClientAPI;
using System;
using System.Text;

namespace EventStore_Learn
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			var conn = EventStoreConnection.Create(
				new Uri(
					uriString: "tcp://admin:changeit@localhost:1113"),
					connectionName: "InputFromFileConsoleApp");

			conn.ConnectAsync().Wait();   //  otherwise u get "InputFromFileConsoleApp is not active"

			//			var streamName = "http://127.0.0.1:2113/streams/newstream";
			var streamName = "newstream";

			var readEvents = conn.ReadStreamEventsForwardAsync(
				stream: streamName,
				start: 0,
				count: 10,
				resolveLinkTos: true).Result;

			foreach (var evt in readEvents.Events)
				Console.WriteLine(
					value: Encoding.UTF8.GetString(
						bytes: evt.Event.Data));

			//var readResult = conn.ReadEventAsync(streamName, 0, true).Result;
			//Console.WriteLine(Encoding.UTF8.GetString(readResult.Event.Value.Event.Data));

			Console.ReadLine();
		}
	}
}
