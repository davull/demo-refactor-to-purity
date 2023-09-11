using Refactor.Application.Data;
using Order = Refactor.Application.Models.Order;

namespace Refactor.Application.Services;

public static class OrderItemService
{
    public static OrderItem AddOrderItem(Models.OrderItem orderItem, Order order)
    {
        var orderItemData = MapOrderItem(orderItem, order);
        return orderItemData;
    }

    public static IReadOnlyCollection<Models.OrderItem> GetOrderItems(IEnumerable<OrderItem> orderItems)
    {
        var orderItemModels = orderItems.Select(MapOrderItem).ToList();
        return orderItemModels;
    }

    private static Models.OrderItem MapOrderItem(OrderItem orderItemData)
    {
        var netPrice = orderItemData.Price;
        var taxRate = TaxService.DefaultTaxRate;

        var (taxAmount, grossPrice) = TaxService.CalculateTax(netPrice, taxRate);

        var totalNetPrice = netPrice * orderItemData.Quantity;
        var totalGrossPrice = grossPrice * orderItemData.Quantity;

        var model = new Models.OrderItem(
            orderItemData.Id,
            orderItemData.ProductId,
            Quantity: orderItemData.Quantity,
            GrossPrice: grossPrice,
            NetPrice: netPrice,
            TaxRate: taxRate,
            TaxAmount: taxAmount,
            TotalGrossPrice: totalGrossPrice,
            TotalNetPrice: totalNetPrice);
        return model;
    }

    private static OrderItem MapOrderItem(Models.OrderItem orderItem, Order order)
    {
        var data = new OrderItem(
            Id: orderItem.Id,
            OrderId: order.Id,
            ProductId: orderItem.ProductId,
            Quantity: orderItem.Quantity,
            Price: orderItem.NetPrice);
        return data;
    }
}
