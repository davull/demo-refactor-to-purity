using Refactor.Application.Models;

namespace Refactor.Application.Services;

public static class OrderService
{
    public static async Task<Order> GetOrder(Guid id,
        Func<Guid, Task<OrderData>> getOrder,
        Func<Guid, Task<CustomerData>> getCustomer,
        Func<Guid, Task<IReadOnlyCollection<OrderItemData>>> getOrderItems)
    {
        var orderData = await getOrder(id);
        var customerData = await getCustomer(orderData.CustomerId);
        var orderItemData = await getOrderItems(id);
        return GetOrder(orderData, customerData, orderItemData);
    }

    public static async Task<IReadOnlyCollection<Order>> GetOrdersByDate(
        DateTime startDate, DateTime endDate,
        Func<DateTime, DateTime, Task<IEnumerable<OrderData>>> getOrdersByDate,
        Func<Guid, Task<CustomerData>> getCustomer,
        Func<Guid, Task<IReadOnlyCollection<OrderItemData>>> getOrderItems)
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

    public static async Task AddOrder(Order order, Func<OrderData, Task> add)
    {
        var orderData = new OrderData(
            Id: order.Id,
            CustomerId: order.Customer.Id,
            OrderDate: order.OrderDate);
        await add(orderData);
    }

    private static Order GetOrder(OrderData orderDataData, CustomerData customerDataData,
        IEnumerable<OrderItemData> orderItemData)
    {
        var orderItems = OrderItemService.GetOrderItems(orderItemData);

        var customerModel = new Customer(
            customerDataData.Id,
            customerDataData.FirstName,
            customerDataData.LastName,
            customerDataData.Email);

        var orderModel = new Order(
            orderDataData.Id,
            customerModel,
            orderItems,
            orderDataData.OrderDate);

        return orderModel;
    }
}
