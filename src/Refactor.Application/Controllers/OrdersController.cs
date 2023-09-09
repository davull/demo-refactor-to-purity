using Microsoft.AspNetCore.Mvc;

namespace Refactor.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(ILogger<OrdersController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<string> Get(DateTime? startDate, DateTime? endDate)
    {
        yield return "order 1";
    }
}
