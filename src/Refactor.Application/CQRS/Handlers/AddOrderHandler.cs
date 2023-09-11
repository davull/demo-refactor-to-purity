using MediatR;
using Refactor.Application.CQRS.Requests;
using Refactor.Application.Repositories.Interfaces;
using Refactor.Application.Services;

namespace Refactor.Application.CQRS.Handlers;

public class AddOrderHandler : IRequestHandler<AddOrderRequest>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IOrderRepository _orderRepository;

    public AddOrderHandler(ICustomerRepository customerRepository,
        IOrderItemRepository orderItemRepository,
        IOrderRepository orderRepository)
    {
        _customerRepository = customerRepository;
        _orderItemRepository = orderItemRepository;
        _orderRepository = orderRepository;
    }

    public async Task Handle(AddOrderRequest request, CancellationToken cancellationToken)
    {
        if (!request.Order.Items.Any())
            throw new InvalidOperationException("Order must have at least one item.");

        var customerData = await _customerRepository.Get(request.Order.Customer.Id);

        if (customerData.Active is false)
            throw new InvalidOperationException("Customer is not active.");

        foreach (var orderItem in request.Order.Items)
        {
            var orderItemData = OrderItemService.AddOrderItem(orderItem, request.Order);
            await _orderItemRepository.Add(orderItemData);
        }

        await OrderService.AddOrder(request.Order, _orderRepository.Add);
    }
}
