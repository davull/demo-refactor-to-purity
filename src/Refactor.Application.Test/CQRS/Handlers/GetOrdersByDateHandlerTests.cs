using NSubstitute;
using Refactor.Application.CQRS.Handlers;
using Refactor.Application.CQRS.Requests;
using Refactor.Application.Services;

namespace Refactor.Application.Test.CQRS.Handlers;

public class GetOrdersByDateHandlerTests
{
    [Test]
    public async Task Should_Call_Service()
    {
        // Arrange
        var orderService = Substitute.For<IOrderService>();

        var sut = new GetOrdersByDateHandler(orderService);

        // Act
        await sut.Handle(new GetOrdersByDateRequest(DateTime.MinValue, DateTime.Now), CancellationToken.None);

        // Assert
        await orderService
            .Received(1)
            .GetOrdersByDate(Arg.Any<DateTime>(), Arg.Any<DateTime>());
    }
}
