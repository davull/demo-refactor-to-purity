namespace Refactor.Application.Repositories.Interfaces;

public interface IOrderItemRepository : IRepository<OrderItemData>
{
    Task<IReadOnlyCollection<OrderItemData>> GetByOrderId(Guid orderId);
}
