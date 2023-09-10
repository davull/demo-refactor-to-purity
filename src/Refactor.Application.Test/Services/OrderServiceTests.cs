using FluentAssertions;
using NSubstitute;
using Refactor.Application.Data;
using Refactor.Application.Repositories.Interfaces;
using Refactor.Application.Services;
using OrderItem = Refactor.Application.Models.OrderItem;

namespace Refactor.Application.Test.Services;

public class OrderServiceTests
{
    [Test]
    public async Task Should_Return_Order()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var orderData = new Order(orderId, customerId, DateTime.UtcNow, 100);
        var customerData = new Customer(customerId, "Peter", "Parker", "peter.parker@example.com", true);

        var orderItem1 = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), 1, 0, 0, 0, 0, 0, 0);
        var orderItem2 = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), 1, 0, 0, 0, 0, 0, 0);
        var orderItemModels = new[] { orderItem1, orderItem2 };

        var orderRepository = Substitute.For<IOrderRepository>();
        orderRepository.Get(orderId).Returns(orderData);

        var customerRepository = Substitute.For<ICustomerRepository>();
        customerRepository.Get(customerId).Returns(customerData);

        var orderItemService = Substitute.For<IOrderItemService>();
        orderItemService.GetOrderItems(orderId).Returns(orderItemModels);

        var sut = new OrderService(orderRepository, customerRepository, orderItemService);

        // Act
        var order = await sut.GetOrder(orderId);

        // Assert
        order.Should().NotBeNull();
        order.Customer.FirstName.Should().Be("Peter");
        order.Customer.LastName.Should().Be("Parker");

        order.Items.Should().HaveCount(2);
        order.Items.Should().Contain(x => x.Id == orderItem1.Id);
        order.Items.Should().Contain(x => x.Id == orderItem2.Id);
    }

    [Test]
    public async Task Should_Return_OrdersByDate()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var orderData1 = new Order(orderId, customerId, DateTime.UtcNow, 100);
        var orderData2 = new Order(orderId, customerId, DateTime.UtcNow, 100);
        var orderData = new[] { orderData1, orderData2 };
        var customerData = new Customer(customerId, "Peter", "Parker", "peter.parker@example.com", true);

        var orderItem1 = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), 1, 0, 0, 0, 0, 0, 0);
        var orderItem2 = new OrderItem(Guid.NewGuid(), Guid.NewGuid(), 1, 0, 0, 0, 0, 0, 0);
        var orderItemModels = new[] { orderItem1, orderItem2 };

        var orderRepository = Substitute.For<IOrderRepository>();
        orderRepository.GetOrdersByDate(default, default).ReturnsForAnyArgs(orderData);

        var customerRepository = Substitute.For<ICustomerRepository>();
        customerRepository.Get(customerId).Returns(customerData);

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
