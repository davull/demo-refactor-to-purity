using Refactor.Application.Repositories.Interfaces;
using Refactor.Application.Services;

namespace Refactor.Application.Logic;

public static class OrdersIntegration
{
    public static async Task<IEnumerable<Order>> GetOrdersByDate(
        DateTime startDate, DateTime endDate,
        ICustomerRepository customerRepository,
        IOrderItemRepository orderItemRepository,
        IOrderRepository orderRepository)
    {
        var orders = await OrderService.GetOrdersByDate(
            startDate, endDate,
            getOrdersByDate: orderRepository.GetOrdersByDate,
            getCustomer: customerRepository.Get,
            getOrderItems: orderItemRepository.GetByOrderId);
        return orders;
    }

    public static async Task AddOrder(Order order,
        ICustomerRepository customerRepository,
        IOrderItemRepository orderItemRepository,
        IOrderRepository orderRepository)
    {
        if (!order.Items.Any())
            throw new InvalidOperationException("Order must have at least one item.");

        var customerData = await customerRepository.Get(order.Customer.Id);

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
