﻿using FluentAssertions;
using NSubstitute;
using Refactor.Application.Data;
using Refactor.Application.Repositories.Interfaces;
using Refactor.Application.Services;

namespace Refactor.Application.Test.Services;

public class OrderItemServiceTests
{
    [Test]
    public async Task Should_Return_OrderItems()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        var orderItem1 = new OrderItem(Guid.NewGuid(), orderId, Guid.NewGuid(), 2, 19.75m);
        var orderItem2 = new OrderItem(Guid.NewGuid(), orderId, Guid.NewGuid(), 3, 9.66m);
        var orderItemData = new List<OrderItem> { orderItem1, orderItem2 };

        var orderItemRepository = Substitute.For<IOrderItemRepository>();
        orderItemRepository.GetByOrderId(orderId).Returns(orderItemData);

        var taxService = Substitute.For<ITaxService>();

        taxService.CalculateTax(default, default)
            .ReturnsForAnyArgs(info =>
            {
                var netPrice = info.ArgAt<decimal>(0);
                var taxRate = info.ArgAt<decimal>(1);

                var taxAmount = netPrice * taxRate / 100m;
                var grossPrice = netPrice + taxAmount;

                return (taxAmount, grossPrice);
            });

        var sut = new OrderItemService(orderItemRepository, taxService);

        // Act
        var orderItems = await sut.GetOrderItems(orderId);

        // Assert
        orderItems.Should().NotBeNullOrEmpty();
        orderItems.Should().HaveCount(2);

        var firstOrderItem = orderItems.First();
        firstOrderItem.Id.Should().Be(orderItem1.Id);
        firstOrderItem.TaxRate.Should().Be(19);
        firstOrderItem.GrossPrice.Should().Be(19.75m * 1.19m);
    }
}