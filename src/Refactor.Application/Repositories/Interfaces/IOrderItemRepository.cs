﻿using Refactor.Application.Data;

namespace Refactor.Application.Repositories.Interfaces;

public interface IOrderItemRepository : IRepository<OrderItem>
{
    Task<IReadOnlyCollection<OrderItem>> GetByOrderId(Guid orderId);
}