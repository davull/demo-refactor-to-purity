using Refactor.Application.Models;
using Customer = Refactor.Application.Data.Customer;
using OrderItem = Refactor.Application.Data.OrderItem;

namespace Refactor.Application.Services;

public static class OrderService
{
    public static async Task<Order> GetOrder(Guid id,
        Func<Guid, Task<Data.Order>> getOrder,
        Func<Guid, Task<Customer>> getCustomer,
        Func<Guid, Task<IReadOnlyCollection<OrderItem>>> getOrderItems)
    {
        var orderData = await getOrder(id);
        var customerData = await getCustomer(orderData.CustomerId);
        var orderItemData = await getOrderItems(id);
        return GetOrder(orderData, customerData, orderItemData);
    }

    public static async Task<IReadOnlyCollection<Order>> GetOrdersByDate(
        DateTime startDate, DateTime endDate,
        Func<DateTime, DateTime, Task<IEnumerable<Data.Order>>> getOrdersByDate,
        Func<Guid, Task<Customer>> getCustomer,
        Func<Guid, Task<IReadOnlyCollection<OrderItem>>> getOrderItems)
    {
        var orderData = await getOrdersByDate(startDate, endDate);

        var orders = new List<Order>();

        foreach (var order in orderData)
        {
            var customerData = await getCustomer(order.CustomerId);
            var orderItemData = await getOrderItems(order.Id);
            orders.Add(GetOrder(order, customerData, orderItemData));
        }

        return orders;
    }

    public static async Task AddOrder(Order order, Func<Data.Order, Task> add)
    {
        var orderData = new Data.Order(
            Id: order.Id,
            CustomerId: order.Customer.Id,
            OrderDate: order.OrderDate);
        await add(orderData);
    }

    private static Order GetOrder(Data.Order orderData, Customer customerData,
        IEnumerable<OrderItem> orderItemData)
    {
        var orderItems = OrderItemService.GetOrderItems(orderItemData);

        var customerModel = new Models.Customer(
            customerData.Id,
            customerData.FirstName,
            customerData.LastName,
            customerData.Email);

        var orderModel = new Order(
            orderData.Id,
            customerModel,
            orderItems,
            orderData.OrderDate);

        return orderModel;
    }
}
