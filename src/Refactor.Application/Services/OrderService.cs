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

    public Task<IReadOnlyCollection<Order>> GetOrdersByDate(DateTime startDate, DateTime endDate) =>
        throw new NotImplementedException();
}
