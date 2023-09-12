using Microsoft.AspNetCore.Mvc;
using Refactor.Application.Logic;
using Refactor.Application.Repositories;

namespace Refactor.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IDatabase _db;
    private readonly OrderItemRepository _orderItemRepository;
    private readonly OrderRepository _orderRepository;

    public OrdersController(OrderRepository orderRepository,
        OrderItemRepository orderItemRepository,
        IDatabase db)
    {
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _db = db;
    }

    [HttpGet]
    public async Task<IEnumerable<Order>> Get(DateTime? startDate, DateTime? endDate)
    {
        var result = await OrdersIntegration.GetOrdersByDate(
            startDate ?? DateTime.MinValue,
            endDate ?? DateTime.MaxValue,
            _orderItemRepository,
            _orderRepository,
            _db);
        return result;
    }

    [HttpPost]
    public async Task<IActionResult> Add(Order order)
    {
        await OrdersIntegration.AddOrder(order, _orderItemRepository, _orderRepository, _db);
        return Ok();
    }
}
