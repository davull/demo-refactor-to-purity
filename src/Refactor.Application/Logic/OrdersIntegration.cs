using Refactor.Application.Repositories;
using Refactor.Application.Services;

namespace Refactor.Application.Logic;

public static class OrdersIntegration
{
    public static async Task<IEnumerable<Order>> GetOrdersByDate(DateTime startDate, DateTime endDate, IDatabase db)
    {
        var allOrderData = await db.GetAll<OrderData>();

        var customerData = (await db.GetAll<CustomerData>())
            .ToDictionary(x => x.Id, x => x);

        var orderData = (await db.GetAll<OrderItemData>())
            .ToLookup(x => x.OrderId);

        var orders = OrderService.GetOrdersByDate(startDate, endDate, allOrderData, customerData, orderData);
        return orders;
    }

    public static async Task AddOrder(Order order, IDatabase db)
    {
        if (!order.Items.Any())
            throw new InvalidOperationException("Order must have at least one item.");

        var customerData = await CustomerRepository.Get(order.Customer.Id, db.Get<CustomerData>);

        if (customerData.Active is false)
            throw new InvalidOperationException("Customer is not active.");

        foreach (var orderItem in order.Items)
        {
            var orderItemData = OrderItemService.AddOrderItem(orderItem, order);
            await OrderItemRepository.Add(orderItemData, db.Add);
        }

        var orderData = OrderService.AddOrder(order);
        await OrderRepository.Add(orderData, db.Add);
    }
}
