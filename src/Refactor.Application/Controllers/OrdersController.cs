using Microsoft.AspNetCore.Mvc;
using Refactor.Application.Logic;
using Refactor.Application.Models;
using Refactor.Application.Repositories.Interfaces;

namespace Refactor.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IOrderRepository _orderRepository;

    public OrdersController(IOrderRepository orderRepository,
        IOrderItemRepository orderItemRepository,
        ICustomerRepository customerRepository)
    {
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _customerRepository = customerRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<Order>> Get(DateTime? startDate, DateTime? endDate)
    {
        var result = await OrdersIntegration.GetOrdersByDate(
            startDate ?? DateTime.MinValue,
            endDate ?? DateTime.MaxValue,
            _customerRepository,
            _orderItemRepository,
            _orderRepository);
        return result;
    }

    [HttpPost]
    public async Task<IActionResult> Add(Order order, CancellationToken cancellationToken = default)
    {
        await OrdersIntegration.AddOrder(order, _customerRepository, _orderItemRepository, _orderRepository);
        return Ok();
    }
}
