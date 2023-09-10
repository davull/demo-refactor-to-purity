﻿using Refactor.Application.Models;

namespace Refactor.Application.Services;

public interface IOrderItemService : IService
{
    Task<IReadOnlyCollection<OrderItem>> GetOrderItems(Guid orderId);

    Task AddOrderItem(OrderItem orderItem, Order order);
}