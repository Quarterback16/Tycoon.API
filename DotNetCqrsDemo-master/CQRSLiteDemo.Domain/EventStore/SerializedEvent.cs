using System;

namespace CQRSLiteDemo.Domain.EventStore
{
   public class SerializedEvent
    {
        public Guid EventId { get; set; }
        public Guid AggregateId { get; set; }
        public string Type { get; set; }
        public string Event { get; set; }
        public DateTime Timestamp { get; set; }
        public int Version { get; set; }
    }
}
