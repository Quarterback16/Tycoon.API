using EventStore.ClientAPI;
using System;
using System.Net;
using System.Text;

namespace EventStore_Learn
{
	public class ConnectionTest
	{
		public void Execute()
		{
			var connection =
				EventStoreConnection.Create(
					new IPEndPoint(
						address: IPAddress.Loopback,
						port: 1113));

			// Don't forget to tell the connection to connect!
			connection.ConnectAsync().Wait();

			var myEvent = new EventData(
				eventId: Guid.NewGuid(),
				type: "testEvent",
				isJson: false,
				data: Encoding.UTF8.GetBytes("some data"),
				metadata: Encoding.UTF8.GetBytes("some metadata"));

			connection.AppendToStreamAsync(
				stream: "test-stream",
				expectedVersion: ExpectedVersion.Any,
				events: myEvent).Wait();

			var streamEvents =	connection.ReadStreamEventsForwardAsync(
				stream: "test-stream",
				start: 0,
				count: 1,
				resolveLinkTos: false).Result;

			var returnedEvent = streamEvents.Events[0].Event;

			Console.WriteLine("Read event with data: {0}, metadata: {1}",
				Encoding.UTF8.GetString(returnedEvent.Data),
				Encoding.UTF8.GetString(returnedEvent.Metadata));
		}
	}
}
