using Microsoft.AspNetCore.Mvc;
using Refactor.Application.Logic;
using Refactor.Application.Repositories;

namespace Refactor.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IDatabase _db;

    public OrdersController(IDatabase db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IEnumerable<Order>> Get(DateTime? startDate, DateTime? endDate)
    {
        var result = await OrdersIntegration.GetOrdersByDate(
            startDate ?? DateTime.MinValue,
            endDate ?? DateTime.MaxValue,
            _db);
        return result;
    }

    [HttpPost]
    public async Task<IActionResult> Add(Order order)
    {
        await OrdersIntegration.AddOrder(order, _db);
        return Ok();
    }
}
