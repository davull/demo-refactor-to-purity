using MediatR;
using Refactor.Application.Models;

namespace Refactor.Application.CQRS.Requests;

public class AddOrderRequest : IRequest
{
    public AddOrderRequest(Order order)
    {
        Order = order;
    }

    public Order Order { get; }
}
