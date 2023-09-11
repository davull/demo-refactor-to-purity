using MediatR;
using Refactor.Application.CQRS.Requests;
using Refactor.Application.Models;
using Refactor.Application.Repositories.Interfaces;
using Refactor.Application.Services;

namespace Refactor.Application.CQRS.Handlers;

public class GetOrdersByDateHandler : IRequestHandler<GetOrdersByDateRequest, IEnumerable<Order>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IOrderRepository _orderRepository;

    public GetOrdersByDateHandler(ICustomerRepository customerRepository, IOrderItemRepository orderItemRepository,
        IOrderRepository orderRepository)
    {
        _customerRepository = customerRepository;
        _orderItemRepository = orderItemRepository;
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<Order>> Handle(GetOrdersByDateRequest request, CancellationToken cancellationToken)
    {
        var orders = await OrderService.GetOrdersByDate(
            request.StartDate, request.EndDate,
            getOrdersByDate: _orderRepository.GetOrdersByDate,
            getCustomer: _customerRepository.Get,
            getOrderItems: _orderItemRepository.GetByOrderId);
        return orders;
    }
}
