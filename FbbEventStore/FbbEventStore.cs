using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace FbbEventStore 
{
	public class FbbEventStore : IEventStore
	{
		public List<FbbEvent> Events { get; set; }

		public FbbEventStore()
		{
			using (var r = new StreamReader("fbbEvents.json"))
			{
				var json = r.ReadToEnd();
				Events = JsonConvert.DeserializeObject<List<FbbEvent>>(json);
			}
		}

		//  Get all events for a specific aggregate (order by version)
		public IEnumerable<IEvent> Get<T>(Guid aggregateId, int fromVersion)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IEvent> Get<T>(string eventType)
		{
			return Events;
		}
	}
}
