using Refactor.Application.Data;
using Refactor.Application.Models;

namespace Refactor.Application.Services;

public static class OrderItemService
{
    public static OrderItemData AddOrderItem(OrderItem orderItem, Order order)
    {
        var orderItemData = MapOrderItem(orderItem, order);
        return orderItemData;
    }

    public static IReadOnlyCollection<OrderItem> GetOrderItems(IEnumerable<OrderItemData> orderItems)
    {
        var orderItemModels = orderItems.Select(MapOrderItem).ToList();
        return orderItemModels;
    }

    private static OrderItem MapOrderItem(OrderItemData orderItemDataData)
    {
        var netPrice = orderItemDataData.Price;
        var taxRate = TaxService.DefaultTaxRate;

        var (taxAmount, grossPrice) = TaxService.CalculateTax(netPrice, taxRate);

        var totalNetPrice = netPrice * orderItemDataData.Quantity;
        var totalGrossPrice = grossPrice * orderItemDataData.Quantity;

        var model = new OrderItem(
            orderItemDataData.Id,
            orderItemDataData.ProductId,
            Quantity: orderItemDataData.Quantity,
            GrossPrice: grossPrice,
            NetPrice: netPrice,
            TaxRate: taxRate,
            TaxAmount: taxAmount,
            TotalGrossPrice: totalGrossPrice,
            TotalNetPrice: totalNetPrice);
        return model;
    }

    private static OrderItemData MapOrderItem(OrderItem orderItem, Order order)
    {
        var data = new OrderItemData(
            Id: orderItem.Id,
            OrderId: order.Id,
            ProductId: orderItem.ProductId,
            Quantity: orderItem.Quantity,
            Price: orderItem.NetPrice);
        return data;
    }
}
