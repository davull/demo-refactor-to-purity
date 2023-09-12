using Refactor.Application.Repositories;
using Refactor.Application.Services;

namespace Refactor.Application.Logic;

public static class OrdersIntegration
{
    public static async Task<IEnumerable<Order>> GetOrdersByDate(DateTime startDate, DateTime endDate, IDatabase db)
    {
        var orders = await OrderService.GetOrdersByDate(
            startDate, endDate,
            getOrdersByDate: (s, e) => OrderRepository.GetOrdersByDate(s, e, db),
            getCustomer: id => CustomerRepository.Get(id, db),
            getOrderItems: id => OrderItemRepository.GetByOrderId(id, db));
        return orders;
    }

    public static async Task AddOrder(Order order, IDatabase db)
    {
        if (!order.Items.Any())
            throw new InvalidOperationException("Order must have at least one item.");

        var customerData = await CustomerRepository.Get(order.Customer.Id, db);

        if (customerData.Active is false)
            throw new InvalidOperationException("Customer is not active.");

        foreach (var orderItem in order.Items)
        {
            var orderItemData = OrderItemService.AddOrderItem(orderItem, order);
            await OrderItemRepository.Add(orderItemData, db);
        }

        await OrderService.AddOrder(order, o => OrderRepository.Add(o, db));
    }
}
