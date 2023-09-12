using Refactor.Application.Repositories;
using Refactor.Application.Services;

namespace Refactor.Application.Logic;

public static class OrdersIntegration
{
    public static async Task<IEnumerable<Order>> GetOrdersByDate(
        DateTime startDate, DateTime endDate,
        OrderItemRepository orderItemRepository,
        OrderRepository orderRepository,
        IDatabase db)
    {
        if (orderItemRepository == null)
            throw new ArgumentNullException(nameof(orderItemRepository));

        var orders = await OrderService.GetOrdersByDate(
            startDate, endDate,
            getOrdersByDate: orderRepository.GetOrdersByDate,
            getCustomer: id => CustomerRepository.Get(id, db),
            getOrderItems: orderItemRepository.GetByOrderId);
        return orders;
    }

    public static async Task AddOrder(Order order,
        OrderItemRepository orderItemRepository,
        OrderRepository orderRepository,
        IDatabase db)
    {
        if (!order.Items.Any())
            throw new InvalidOperationException("Order must have at least one item.");

        var customerData = await CustomerRepository.Get(order.Customer.Id, db);

        if (customerData.Active is false)
            throw new InvalidOperationException("Customer is not active.");

        foreach (var orderItem in order.Items)
        {
            var orderItemData = OrderItemService.AddOrderItem(orderItem, order);
            await orderItemRepository.Add(orderItemData);
        }

        await OrderService.AddOrder(order, orderRepository.Add);
    }
}
