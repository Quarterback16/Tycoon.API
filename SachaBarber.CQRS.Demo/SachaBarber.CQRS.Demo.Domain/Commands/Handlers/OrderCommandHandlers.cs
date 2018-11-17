﻿using System.Linq;

using CQRSlite.Commands;
using CQRSlite.Domain;
using SachaBarber.CQRS.Demo.Orders.Commands;
using SachaBarber.CQRS.Demo.Orders.Domain.Aggregates;

namespace SachaBarber.CQRS.Demo.Orders.Domain.Commands
{
   public class OrderCommandHandlers : ICommandHandler<CreateOrderCommand>,
                                        ICommandHandler<ChangeOrderAddressCommand>,
                                        ICommandHandler<DeleteOrderCommand>
    {
        private readonly ISession _session;

        public OrderCommandHandlers(ISession session)
        {
            _session = session;
        }

        public void Handle(CreateOrderCommand command)
        {
            var item = new Order(
                command.Id, 
                command.ExpectedVersion, 
                command.Description, 
                command.Address,
                command.OrderItems.Select(x => new OrderItem()
                {
                    OrderId = x.OrderId,
                    StoreItemDescription = x.StoreItemDescription,
                    StoreItemId = x.StoreItemId,
                    StoreItemUrl = x.StoreItemUrl
                }).ToList());
            _session.Add(item);
            _session.Commit();
        }

        public void Handle(ChangeOrderAddressCommand command)
        {
            Order item = _session.Get<Order>(
                command.Id, command.ExpectedVersion);
            item.ChangeAddress(command.NewAddress);
            _session.Commit();
        }

        public void Handle(DeleteOrderCommand command)
        {
            Order item = _session.Get<Order>(
                command.Id, command.ExpectedVersion);
            item.Delete();
            _session.Commit();
        }
    }
    }
