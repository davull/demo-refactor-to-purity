﻿using FluentAssertions;
using Refactor.Application.Services;
using static Refactor.Application.Test.DataDummies;

namespace Refactor.Application.Test.Services;

public class OrderItemServiceTests
{
    [Test]
    public void Should_Return_OrderItems()
    {
        // Arrange
        var orderItem1 = OrderItem(price: 19.75m);
        var orderItem2 = OrderItem(price: 9.66m);
        var orderItemData = Collection(orderItem1, orderItem2);

        // Act
        var orderItems = OrderItemService.GetOrderItems(orderItemData);

        // Assert
        orderItems.Should().NotBeNullOrEmpty();
        orderItems.Should().HaveCount(2);

        var firstOrderItem = orderItems.First();
        firstOrderItem.Id.Should().Be(orderItem1.Id);
        firstOrderItem.TaxRate.Should().Be(19);
        firstOrderItem.GrossPrice.Should().Be(19.75m * 1.19m);
    }
}
