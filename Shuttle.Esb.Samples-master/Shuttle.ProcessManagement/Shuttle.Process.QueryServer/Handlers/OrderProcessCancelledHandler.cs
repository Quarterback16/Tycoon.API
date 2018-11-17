﻿using Shuttle.Core.Contract;
using Shuttle.Core.Data;
using Shuttle.Esb;
using Shuttle.ProcessManagement;
using Shuttle.ProcessManagement.Messages;

namespace Shuttle.Process.QueryServer
{
    public class OrderProcessCancelledHandler : IMessageHandler<OrderProcessCancelledEvent>
    {
        private readonly IDatabaseContextFactory _databaseContextFactory;
        private readonly IOrderProcessViewQuery _orderProcessViewQuery;

        public OrderProcessCancelledHandler(IDatabaseContextFactory databaseContextFactory,
            IOrderProcessViewQuery orderProcessViewQuery)
        {
            Guard.AgainstNull(databaseContextFactory, nameof(databaseContextFactory));
            Guard.AgainstNull(orderProcessViewQuery, nameof(orderProcessViewQuery));

            _databaseContextFactory = databaseContextFactory;
            _orderProcessViewQuery = orderProcessViewQuery;
        }

        public bool IsReusable => true;

        public void ProcessMessage(IHandlerContext<OrderProcessCancelledEvent> context)
        {
            using (_databaseContextFactory.Create(ProcessManagementData.ConnectionStringName))
            {
                _orderProcessViewQuery.Remove(context.Message.OrderProcessId);
            }
        }
    }
}