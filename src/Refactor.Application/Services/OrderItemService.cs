using Refactor.Application.Models;
using Refactor.Application.Repositories.Interfaces;

namespace Refactor.Application.Services;

public class OrderItemService : IOrderItemService
{
    private readonly IOrderItemRepository _orderItemRepository;

    public OrderItemService(IOrderItemRepository orderItemRepository)
    {
        _orderItemRepository = orderItemRepository;
    }

    public async Task<IReadOnlyCollection<OrderItem>> GetOrderItems(Guid orderId)
    {
        var orderItemData = await _orderItemRepository.GetByOrderId(orderId);
        var orderItemModels = orderItemData.Select(MapOrderItem).ToList();

        return orderItemModels;
    }

    public async Task AddOrderItem(OrderItem orderItem, Order order)
    {
        var orderItemData = MapOrderItem(orderItem, order);
        await _orderItemRepository.Add(orderItemData);
    }

    private static OrderItem MapOrderItem(Data.OrderItem orderItemData)
    {
        var netPrice = orderItemData.Price;
        var taxRate = TaxService.DefaultTaxRate;

        var (taxAmount, grossPrice) = TaxService.CalculateTax(netPrice, taxRate);

        var totalNetPrice = netPrice * orderItemData.Quantity;
        var totalGrossPrice = grossPrice * orderItemData.Quantity;

        var model = new OrderItem(
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

    private static Data.OrderItem MapOrderItem(OrderItem orderItem, Order order)
    {
        var data = new Data.OrderItem(
            Id: orderItem.Id,
            OrderId: order.Id,
            ProductId: orderItem.ProductId,
            Quantity: orderItem.Quantity,
            Price: orderItem.NetPrice);
        return data;
    }
}
