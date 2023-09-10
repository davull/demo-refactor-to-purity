﻿using NSubstitute;
using Refactor.Application.CQRS.Handlers;
using Refactor.Application.CQRS.Requests;
using Refactor.Application.Data;
using Refactor.Application.Repositories.Interfaces;
using Refactor.Application.Services;
using Order = Refactor.Application.Models.Order;
using OrderItem = Refactor.Application.Models.OrderItem;

namespace Refactor.Application.Test.CQRS.Handlers;

public class AddOrderHandlerTests
{
    [Test]
    public async Task Should_Add_Order()
    {
        // Arrange
        var orderService = Substitute.For<IOrderService>();
        var orderItemService = Substitute.For<IOrderItemService>();
        var customerRepository = Substitute.For<ICustomerRepository>();

        customerRepository.Get(default)
            .ReturnsForAnyArgs(new Customer(Guid.NewGuid(), "A.", "Nomymous", "none", true));

        var sut = new AddOrderHandler(orderService, orderItemService, customerRepository);

        // Act
        var request = new AddOrderRequest(new Order(Guid.NewGuid(),
            new Models.Customer(Guid.NewGuid(), "A.", "Nomymous", "none"),
            new[] { new OrderItem(Guid.NewGuid(), Guid.NewGuid(), 1, 119, 100, 19, 19, 119, 100) },
            DateTime.Now));
        await sut.Handle(request, CancellationToken.None);

        // Assert
        await orderService
            .Received(1)
            .AddOrder(Arg.Any<Order>());

        await orderItemService
            .Received(1)
            .AddOrderItem(Arg.Any<OrderItem>(), Arg.Any<Order>());
    }
}
