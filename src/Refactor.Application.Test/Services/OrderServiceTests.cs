using FluentAssertions;
using NSubstitute;
using Refactor.Application.Repositories.Interfaces;
using Refactor.Application.Services;

namespace Refactor.Application.Test.Services;

public class OrderServiceTests
{
    [Test]
    public async Task Should_Return_Order()
    {
        // Arrange
        var peterPan = DataDummies.PeterPan;
        var orderId = Guid.NewGuid();
        var orderData = DataDummies.Order(orderId, peterPan.Id);

        var orderItem1 = ModelDummies.OrderItem();
        var orderItem2 = ModelDummies.OrderItem();
        var orderItemModels = ModelDummies.Collection(orderItem1, orderItem2);

        var orderRepository = Substitute.For<IOrderRepository>();
        orderRepository.Get(orderId).Returns(orderData);

        var customerRepository = Substitute.For<ICustomerRepository>();
        customerRepository.Get(peterPan.Id).Returns(peterPan);

        var orderItemService = Substitute.For<IOrderItemService>();
        orderItemService.GetOrderItems(orderId).Returns(orderItemModels);

        var sut = new OrderService(orderRepository, customerRepository, orderItemService);

        // Act
        var order = await sut.GetOrder(orderId);

        // Assert
        order.Should().NotBeNull();
        order.Customer.Should().Be(ModelDummies.PeterPan);

        order.Items.Should().HaveCount(2);
        order.Items.Should().Contain(x => x.Id == orderItem1.Id);
        order.Items.Should().Contain(x => x.Id == orderItem2.Id);
    }

    [Test]
    public async Task Should_Return_OrdersByDate()
    {
        // Arrange
        var peterPan = DataDummies.PeterPan;
        var orderId = Guid.NewGuid();
        var orderData1 = DataDummies.Order(orderId, peterPan.Id);
        var orderData2 = DataDummies.Order(orderId, peterPan.Id);
        var orderData = DataDummies.Many(orderData1, orderData2);

        var orderItem1 = ModelDummies.OrderItem();
        var orderItem2 = ModelDummies.OrderItem();
        var orderItemModels = DataDummies.Collection(orderItem1, orderItem2);

        var orderRepository = Substitute.For<IOrderRepository>();
        orderRepository.GetOrdersByDate(default, default).ReturnsForAnyArgs(orderData);

        var customerRepository = Substitute.For<ICustomerRepository>();
        customerRepository.Get(peterPan.Id).Returns(peterPan);

        var orderItemService = Substitute.For<IOrderItemService>();
        orderItemService.GetOrderItems(orderId).Returns(orderItemModels);

        var startDate = DateTime.UtcNow.AddDays(-1);
        var endDate = DateTime.UtcNow;

        var sut = new OrderService(orderRepository, customerRepository, orderItemService);

        // Act
        var orders = await sut.GetOrdersByDate(startDate, endDate);

        // Assert
        orders.Should().NotBeNullOrEmpty();
    }
}
