using MediatR;
using Refactor.Application.Models;

namespace Refactor.Application.CQRS.Requests;

public class GetOrdersByDateRequest : IRequest<IEnumerable<Order>>
{
    public GetOrdersByDateRequest(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
}
