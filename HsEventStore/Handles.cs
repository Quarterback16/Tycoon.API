namespace HsEventStore
{
    public interface Handles<T> where T : IEvent
    {
        void Handle(T message);
    }
}
