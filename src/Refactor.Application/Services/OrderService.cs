using Refactor.Application.Logic;

namespace Refactor.Application.Services;

public static class OrderService
{
    public static IReadOnlyCollection<Order> GetOrdersByDate(
        DateTime startDate, DateTime endDate,
        IEnumerable<OrderData> allOrderData,
        IDictionary<Guid, CustomerData> customerData,
        ILookup<Guid, OrderItemData> orderItemData)
    {
        return allOrderData
            .Where(x => x.OrderDate >= startDate && x.OrderDate <= endDate)
            .Select(order => GetOrder(order, customerData[order.CustomerId], orderItemData[order.Id]))
            .ToList();
    }

    public static OrderData AddOrder(Order order)
    {
        var orderData = new OrderData(
            Id: order.Id,
            CustomerId: order.Customer.Id,
            OrderDate: order.OrderDate);
        return orderData;
    }

    public static Order GetOrder(OrderData orderDataData, CustomerData customerDataData,
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
