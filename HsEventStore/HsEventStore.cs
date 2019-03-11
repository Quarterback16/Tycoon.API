using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace HsEventStore
{
    public class HsEventStore : IEventStore
    {
        public List<HsGamePlayedEvent> Events { get; set; }

        public HsEventStore()
        {
            // load a JSON file    

            using (var r = new StreamReader("gevents.json"))
            {
                var json = r.ReadToEnd();
                Events = JsonConvert.DeserializeObject<List<HsGamePlayedEvent>>(json);
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
