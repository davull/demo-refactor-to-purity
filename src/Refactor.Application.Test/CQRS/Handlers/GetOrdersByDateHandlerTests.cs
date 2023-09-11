using NSubstitute;
using Refactor.Application.CQRS.Handlers;
using Refactor.Application.CQRS.Requests;
using Refactor.Application.Repositories.Interfaces;

namespace Refactor.Application.Test.CQRS.Handlers;

public class GetOrdersByDateHandlerTests
{
    [Test]
    public async Task Should_Call_Service()
    {
        // Arrange
        var orderRepository = Substitute.For<IOrderRepository>();
        var orderItemRepository = Substitute.For<IOrderItemRepository>();
        var customerRepository = Substitute.For<ICustomerRepository>();

        var sut = new GetOrdersByDateHandler(customerRepository, orderItemRepository, orderRepository);

        // Act
        await sut.Handle(new GetOrdersByDateRequest(DateTime.MinValue, DateTime.Now), CancellationToken.None);

        // Assert
        await orderRepository
            .Received(1)
            .GetOrdersByDate(Arg.Any<DateTime>(), Arg.Any<DateTime>());
    }
}
