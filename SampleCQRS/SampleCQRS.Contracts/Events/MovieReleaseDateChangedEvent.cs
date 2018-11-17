using SimpleCqrs.Eventing;
using System;

namespace SampleCQRS.Contracts.Events
{
    public class MovieReleaseDateChangedEvent : DomainEvent
    {
        public DateTime ReleaseDate { get; set; }

        public MovieReleaseDateChangedEvent(
           Guid movieId, DateTime releaseDate)
        {
            AggregateRootId = movieId;
            ReleaseDate = releaseDate;
        }
    }
}
