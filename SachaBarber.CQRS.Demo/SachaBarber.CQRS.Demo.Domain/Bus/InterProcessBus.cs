using System.Configuration;
using System.Text;
using RabbitMQ.Client;

namespace SachaBarber.CQRS.Demo.Orders.Domain.Bus
{
   public class InterProcessBus : IInterProcessBus
    {

        private readonly string busName;
        private readonly string connectionString;

        public InterProcessBus()
        {
           busName = "InterProcessBus";
           connectionString = ConfigurationManager.AppSettings["RabbitMqHost"];
        }

        public void SendMessage(string message)
        {
            var factory = new ConnectionFactory() { HostName = connectionString };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var bytes = Encoding.ASCII.GetBytes(message);
                    channel.ExchangeDeclare(exchange: busName, type: "fanout");
                    channel.BasicPublish(
                       exchange: busName, 
                       routingKey: string.Empty, 
                       basicProperties: null, 
                       body:bytes);
                }
            }
        }
    }
}
