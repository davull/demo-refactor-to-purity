using FluentAssertions;
using Refactor.Application.Services;

namespace Refactor.Application.Test.Services;

public class OrderServiceTests
{
    [Test]
    public void Should_Return_Order()
    {
        // Arrange
        var orderItems = DataDummies.Collection(DataDummies.OrderItem(), DataDummies.OrderItem());

        // Act
        var order = OrderService.GetOrder(
            DataDummies.Order(customerId: DataDummies.PeterPan.Id),
            DataDummies.PeterPan, orderItems);

        // Assert
        order.Should().NotBeNull();
        order.Customer.Should().Be(ModelDummies.PeterPan);

        order.Items.Should().HaveCount(2);

        foreach (var orderItem in orderItems)
            order.Items.Should().Contain(x => x.Id == orderItem.Id);
    }

    [Test]
    public void Should_Return_OrdersByDate()
    {
        // Arrange
        var peterPan = DataDummies.PeterPan;

        var orderData = DataDummies.Enum(
            DataDummies.Order(customerId: peterPan.Id),
            DataDummies.Order(customerId: peterPan.Id));

        var orderItemData = DataDummies
            .Collection(DataDummies.OrderItem(), DataDummies.OrderItem())
            .ToLookup(x => x.Id);

        var startDate = DateTime.UtcNow.AddDays(-1);
        var endDate = DateTime.UtcNow;

        // Act
        var orders = OrderService.GetOrdersByDate(startDate, endDate,
            orderData,
            new Dictionary<Guid, CustomerData> { { peterPan.Id, peterPan } },
            orderItemData);

        // Assert
        orders.Should().NotBeNullOrEmpty();
    }
}
