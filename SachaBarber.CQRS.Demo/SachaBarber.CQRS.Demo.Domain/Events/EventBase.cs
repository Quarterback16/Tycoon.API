using System;
using CQRSlite.Events;

namespace SachaBarber.CQRS.Demo.Orders.Domain.Events
{
    public abstract class EventBase : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}
