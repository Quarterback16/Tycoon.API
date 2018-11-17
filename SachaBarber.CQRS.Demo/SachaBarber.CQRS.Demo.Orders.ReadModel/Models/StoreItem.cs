using System;

namespace SachaBarber.CQRS.Demo.Orders.ReadModel.Models
{
    public class StoreItem
    {
        public string Id { get; set; }
        public Guid StoreItemId { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
