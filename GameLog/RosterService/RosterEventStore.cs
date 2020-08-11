using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace RosterService
{
	public class RosterEventStore : IEventStore
	{
		public List<RosterEvent> Events { get; set; }

		public RosterEventStore()
		{
			using (var r = new StreamReader("RetroEvents.json"))
			{
				var json = r.ReadToEnd();
				Events = JsonConvert.DeserializeObject<List<RosterEvent>>(json);
			}
		}

		public IEnumerable<IEvent> Get<T>(string eventType)
		{
			return Events;
		}

		public IEnumerable<IEvent> Get<T>(
			Guid aggregateId,
			int fromVersion)
		{
			throw new NotImplementedException();
		}

	}
}
