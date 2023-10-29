using Refactor.Application.Logic;
using Refactor.Application.Repositories;
using Refactor.Application.Services;

namespace Refactor.Application;

public class OrdersIntegration
{
    private readonly IDatabase _db;

    public OrdersIntegration(IDatabase db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Order>> GetOrdersByDate(DateTime startDate, DateTime endDate)
    {
        var allOrderData = await _db.GetAll<OrderData>();

        var customerData = (await _db.GetAll<CustomerData>())
            .ToDictionary(x => x.Id, x => x);

        var orderData = (await _db.GetAll<OrderItemData>())
            .ToLookup(x => x.OrderId);

        var orders = OrderService.GetOrdersByDate(startDate, endDate, allOrderData, customerData, orderData);
        return orders;
    }

    public async Task AddOrder(Order order)
    {
        if (!order.Items.Any())
            throw new InvalidOperationException("Order must have at least one item.");

        var customerData = await CustomerRepository.Get(order.Customer.Id, _db.Get<CustomerData>);

        if (customerData.Active is false)
            throw new InvalidOperationException("Customer is not active.");

        foreach (var orderItem in order.Items)
        {
            var orderItemData = OrderItemService.AddOrderItem(orderItem, order);
            await OrderItemRepository.Add(orderItemData, _db.Add);
        }

        var orderData = OrderService.AddOrder(order);
        await OrderRepository.Add(orderData, _db.Add);
    }
}
