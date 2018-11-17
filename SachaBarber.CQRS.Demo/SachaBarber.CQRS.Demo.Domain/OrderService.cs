﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Threading.Tasks;
using SachaBarber.CQRS.Demo.Orders.Commands;
using SachaBarber.CQRS.Demo.Orders.Domain.Commands;
using SachaBarber.CQRS.Demo.Orders.ReadModel;
using SachaBarber.CQRS.Demo.Orders.ReadModel.Models;
using SachaBarber.CQRS.Demo.SharedCore.Exceptions;

namespace SachaBarber.CQRS.Demo.Orders.Domain
{
   [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    //Useful when debugging, see App.Config too
    //[ErrorHandlerBehavior]
    public class OrderService : IOrderService
    {
        private readonly OrderCommandHandlers commandHandlers;
        private readonly IReadModelRepository readModelRepository;

        public OrderService(
           OrderCommandHandlers commandHandlers, 
           IReadModelRepository readModelRepository)
        {
            this.commandHandlers = commandHandlers;
            this.readModelRepository = readModelRepository;
        }


        public async Task<bool> SendCommandAsync(Command command)
        {
            await Task.Run(() =>
            {
                var meth = (from m in typeof(OrderCommandHandlers)
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                            let prms = m.GetParameters()
                            where prms.Count() == 1 && prms[0].ParameterType == command.GetType()
                            select m).FirstOrDefault();

                if (meth == null)
                    throw new BusinessLogicException(
                        string.Format("Handler for {0} could not be found", command.GetType().Name));

                meth.Invoke(commandHandlers, new[] { command });
            });
            return true;
        }

        public async Task<List<StoreItem>> GetAllStoreItemsAsync()
        {
            var storeItems = await readModelRepository.GetAll<StoreItem>();
            return storeItems;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            var orders = await readModelRepository.GetAll<Order>();
            return orders;
        }

        public async Task<Order> GetOrderAsync(Guid orderId)
        {
            var order = await readModelRepository.GetOrder(orderId);
            return order;
        }

    }
}
