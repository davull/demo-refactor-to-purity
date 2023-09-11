using NSubstitute;
using Refactor.Application.CQRS.Handlers;
using Refactor.Application.CQRS.Requests;
using Refactor.Application.Data;
using Refactor.Application.Repositories.Interfaces;

namespace Refactor.Application.Test.CQRS.Handlers;

public class AddOrderHandlerTests
{
    [Test]
    public async Task Should_Add_Order()
    {
        // Arrange
        var customerRepository = Substitute.For<ICustomerRepository>();
        var orderItemRepository = Substitute.For<IOrderItemRepository>();
        var orderRepository = Substitute.For<IOrderRepository>();

        customerRepository.Get(default).ReturnsForAnyArgs(DataDummies.ANomymous);

        var sut = new AddOrderHandler(customerRepository, orderItemRepository, orderRepository);

        // Act
        var request = new AddOrderRequest(ModelDummies.Order(
            customer: ModelDummies.ANomymous,
            items: ModelDummies.Collection(ModelDummies.OrderItem())));
        await sut.Handle(request, CancellationToken.None);

        // Assert
        await orderRepository
            .Received(1)
            .Add(Arg.Any<Order>());

        await orderItemRepository
            .Received(1)
            .Add(Arg.Any<OrderItem>());
    }
}
