using NSubstitute;
using Refactor.Application.CQRS.Handlers;
using Refactor.Application.CQRS.Requests;
using Refactor.Application.Models;
using Refactor.Application.Repositories.Interfaces;
using Refactor.Application.Services;
using OrderItem = Refactor.Application.Data.OrderItem;

namespace Refactor.Application.Test.CQRS.Handlers;

public class AddOrderHandlerTests
{
    [Test]
    public async Task Should_Add_Order()
    {
        // Arrange
        var orderService = Substitute.For<IOrderService>();
        var customerRepository = Substitute.For<ICustomerRepository>();
        var orderItemRepository = Substitute.For<IOrderItemRepository>();

        customerRepository.Get(default).ReturnsForAnyArgs(DataDummies.ANomymous);

        var sut = new AddOrderHandler(orderService, customerRepository, orderItemRepository);

        // Act
        var request = new AddOrderRequest(ModelDummies.Order(
            customer: ModelDummies.ANomymous,
            items: ModelDummies.Collection(ModelDummies.OrderItem())));
        await sut.Handle(request, CancellationToken.None);

        // Assert
        await orderService
            .Received(1)
            .AddOrder(Arg.Any<Order>());

        await orderItemRepository
            .Received(1)
            .Add(Arg.Any<OrderItem>());
    }
}
