using MediatR;
using Refactor.Application.CQRS.Requests;
using Refactor.Application.Models;
using Refactor.Application.Services;

namespace Refactor.Application.CQRS.Handlers;

public class GetOrdersByDateHandler : IRequestHandler<GetOrdersByDateRequest, IEnumerable<Order>>
{
    private readonly IOrderService _orderService;

    public GetOrdersByDateHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<IEnumerable<Order>> Handle(GetOrdersByDateRequest request, CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetOrdersByDate(request.StartDate, request.EndDate);
        return orders;
    }
}
