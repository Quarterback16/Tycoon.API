using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TipIt.Events;
using TipIt.Interfaces;

namespace TipIt.Implementations
{
    public class ResultEventStore : IEventStore
    {
        public List<ResultEvent> Events { get; set; }

        public ResultEventStore()
        {
            using var r = new StreamReader("results.json");
            var json = r.ReadToEnd();
            Events = JsonConvert.DeserializeObject<List<ResultEvent>>(json);
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
