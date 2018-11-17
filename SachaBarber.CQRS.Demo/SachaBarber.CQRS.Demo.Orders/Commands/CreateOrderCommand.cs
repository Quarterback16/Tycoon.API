using System.Collections.Generic;
using System.Runtime.Serialization;
using SachaBarber.CQRS.Demo.Orders.DTOs;

namespace SachaBarber.CQRS.Demo.Orders.Commands
{
   [DataContract]
    public class CreateOrderCommand : Command
    {
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public List<OrderItem> OrderItems { get; set; }

    }
}
