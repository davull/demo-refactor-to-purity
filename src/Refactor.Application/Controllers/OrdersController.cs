using MediatR;
using Microsoft.AspNetCore.Mvc;
using Refactor.Application.CQRS.Requests;
using Refactor.Application.Models;

namespace Refactor.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IMediator _mediator;

    public OrdersController(ILogger<OrdersController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<Order>> Get(DateTime? startDate, DateTime? endDate,
        CancellationToken cancellationToken = default)
    {
        var request = new GetOrdersByDateRequest(
            startDate ?? DateTime.MinValue,
            endDate ?? DateTime.MaxValue);

        var result = await _mediator.Send(request, cancellationToken);
        return result;
    }

    [HttpPost]
    public async Task<IActionResult> Add(Order order, CancellationToken cancellationToken = default)
    {
        var request = new AddOrderRequest(order);
        await _mediator.Send(request, cancellationToken);

        return Ok();
    }
}
