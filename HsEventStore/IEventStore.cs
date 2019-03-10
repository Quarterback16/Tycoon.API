using System;
using System.Collections.Generic;

namespace HsEventStore
{
    public interface IEventStore
    {
        IEnumerable<IEvent> Get<T>(Guid aggregateId, int fromVersion);
    }
}
