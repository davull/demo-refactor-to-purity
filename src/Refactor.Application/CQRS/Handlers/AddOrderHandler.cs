using MediatR;
using Refactor.Application.CQRS.Requests;
using Refactor.Application.Repositories.Interfaces;
using Refactor.Application.Services;

namespace Refactor.Application.CQRS.Handlers;

public class AddOrderHandler : IRequestHandler<AddOrderRequest>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderItemService _orderItemService;
    private readonly IOrderService _orderService;

    public AddOrderHandler(IOrderService orderService,
        IOrderItemService orderItemService,
        ICustomerRepository customerRepository)
    {
        _orderService = orderService;
        _orderItemService = orderItemService;
        _customerRepository = customerRepository;
    }

    public async Task Handle(AddOrderRequest request, CancellationToken cancellationToken)
    {
        if (!request.Order.Items.Any())
            throw new InvalidOperationException("Order must have at least one item.");

        var customerData = await _customerRepository.Get(request.Order.Customer.Id);

        if (customerData.Active is false)
            throw new InvalidOperationException("Customer is not active.");

        foreach (var orderItem in request.Order.Items)
            await _orderItemService.AddOrderItem(orderItem, request.Order);

        await _orderService.AddOrder(request.Order);
    }
}
