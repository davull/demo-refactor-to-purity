using Refactor.Application.Models;
using Refactor.Application.Repositories.Interfaces;

namespace Refactor.Application.Services;

public class OrderService : IOrderService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderItemService _orderItemService;
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IOrderItemService orderItemService)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _orderItemService = orderItemService;
    }

    public async Task<Order> GetOrder(Guid id)
    {
        var orderData = await _orderRepository.Get(id);
        return await GetOrder(orderData);
    }

    public async Task<IReadOnlyCollection<Order>> GetOrdersByDate(DateTime startDate, DateTime endDate)
    {
        var orderData = await _orderRepository.GetOrdersByDate(startDate, endDate);

        var orders = new List<Order>();

        foreach (var order in orderData)
            orders.Add(await GetOrder(order));

        return orders;
    }

    public async Task AddOrder(Order order)
    {
        var orderData = new Data.Order(
            Id: order.Id,
            CustomerId: order.Customer.Id,
            OrderDate: order.OrderDate);
        await _orderRepository.Add(orderData);
    }

    private async Task<Order> GetOrder(Data.Order orderData)
    {
        var customerData = await _customerRepository.Get(orderData.CustomerId);
        var orderItems = await _orderItemService.GetOrderItems(orderData.Id);

        var customerModel = new Customer(
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
