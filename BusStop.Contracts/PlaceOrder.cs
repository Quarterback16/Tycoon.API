using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusStop.API.Contracts
{
   public class PlaceOrder : IMessage
   {
      public Guid CustomerId { get; set; }
      public Guid OrderId { get; set; }
      public Guid ProductId { get; set; }
   }
}