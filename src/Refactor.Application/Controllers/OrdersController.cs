using Microsoft.AspNetCore.Mvc;
using Refactor.Application.Logic;

namespace Refactor.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrdersIntegration _ordersIntegration;

    public OrdersController(OrdersIntegration ordersIntegration)
    {
        _ordersIntegration = ordersIntegration;
    }

    [HttpGet]
    public async Task<IEnumerable<Order>> Get(DateTime? startDate, DateTime? endDate)
    {
        var result = await _ordersIntegration.GetOrdersByDate(
            startDate ?? DateTime.MinValue,
            endDate ?? DateTime.MaxValue);
        return result;
    }

    [HttpPost]
    public async Task<IActionResult> Add(Order order)
    {
        await _ordersIntegration.AddOrder(order);
        return Ok();
    }
}
