﻿using System;
using System.Collections.Generic;
using SachaBarber.CQRS.Demo.Orders.Commands;
using SachaBarber.CQRS.Demo.Orders.ReadModel.Models;

namespace SachaBarber.CQRS.Demo.Orders
{
   public partial class OrderServiceClient :
        System.ServiceModel.ClientBase<IOrderService>, IOrderService
    {

        public OrderServiceClient()
        {
        }

        public OrderServiceClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public OrderServiceClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public OrderServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public OrderServiceClient(System.ServiceModel.Channels.Binding binding,
            System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        public System.Threading.Tasks.Task<bool> SendCommandAsync(Command command)
        {
            return base.Channel.SendCommandAsync(command);
        }

        public System.Threading.Tasks.Task<List<StoreItem>> GetAllStoreItemsAsync()
        {
            return base.Channel.GetAllStoreItemsAsync();
        }

        public System.Threading.Tasks.Task<List<Order>> GetAllOrdersAsync()
        {
            return base.Channel.GetAllOrdersAsync();
        }

        public System.Threading.Tasks.Task<Order> GetOrderAsync(Guid orderId)
        {
            return base.Channel.GetOrderAsync(orderId);
        }




    }
}