using System;
using System.Runtime.Serialization;

namespace SachaBarber.CQRS.Demo.Orders.DTOs
{
   [DataContract]
    public class OrderItem
    {
        [DataMember]
        public Guid OrderId { get; set; }

        [DataMember]
        public Guid StoreItemId { get; set; }

        [DataMember]
        public string StoreItemDescription { get; set; }

        [DataMember]
        public string StoreItemUrl { get; set; }
 
    }
}
