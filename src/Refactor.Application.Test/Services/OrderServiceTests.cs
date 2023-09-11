using FluentAssertions;
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

        var orderItem1 = DataDummies.OrderItem();
        var orderItem2 = DataDummies.OrderItem();

        var getOrder = (Guid _) => Task.FromResult(DataDummies.Order(orderId, peterPan.Id));
        var getCustomer = (Guid _) => Task.FromResult(peterPan);
        var getByOrderId = (Guid _) => Task.FromResult(DataDummies.Collection(orderItem1, orderItem2));

        // Act
        var order = await OrderService.GetOrder(orderId, getOrder, getCustomer, getByOrderId);

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

        var orderData = DataDummies.Enum(
            DataDummies.Order(orderId, peterPan.Id),
            DataDummies.Order(orderId, peterPan.Id));

        var orderItemData = DataDummies.Collection(DataDummies.OrderItem(), DataDummies.OrderItem());

        var getOrdersByDate = (DateTime _, DateTime _) => Task.FromResult(orderData);
        var getCustomer = (Guid _) => Task.FromResult(peterPan);
        var getByOrderId = (Guid _) => Task.FromResult(orderItemData);

        var startDate = DateTime.UtcNow.AddDays(-1);
        var endDate = DateTime.UtcNow;

        // Act
        var orders = await OrderService.GetOrdersByDate(startDate, endDate,
            getOrdersByDate, getCustomer, getByOrderId);

        // Assert
        orders.Should().NotBeNullOrEmpty();
    }
}
