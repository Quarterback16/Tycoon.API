using CQRSlite.Commands;
using System;
using System.Runtime.Serialization;

namespace SachaBarber.CQRS.Demo.Orders.Commands
{
   [DataContract]
    [KnownType(typeof(CreateOrderCommand))]
    [KnownType(typeof(ChangeOrderAddressCommand))]
    [KnownType(typeof(DeleteOrderCommand))]
    public abstract class Command : ICommand
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public int ExpectedVersion { get; set; }
    }
}
