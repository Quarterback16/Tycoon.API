using System;

namespace SachaBarber.CQRS.Demo.Orders.Domain.Bus
{
   public interface IBusEventHandler
    {
        Type HandlerType { get; }
    }

    public interface IBusEventHandler<T> : IBusEventHandler
    {
        void Handle(T @event);
    }
}
