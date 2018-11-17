namespace SachaBarber.CQRS.Demo.Orders.Domain.Bus
{
   public interface IInterProcessBus
    {
        void SendMessage(string message);
    }
}
