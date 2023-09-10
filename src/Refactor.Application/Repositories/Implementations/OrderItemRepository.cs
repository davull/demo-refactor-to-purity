using Refactor.Application.Data;
using Refactor.Application.Repositories.Interfaces;

namespace Refactor.Application.Repositories.Implementations;

public class OrderItemRepository : AbstractRepository<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(IDatabase database) : base(database)
    {
    }

    public override Task<OrderItem> Get(Guid id) => _database.Get<OrderItem>(id);

    public override Task<IEnumerable<OrderItem>> GetAll() => _database.GetAll<OrderItem>();

    public override Task Add(OrderItem entity) => _database.Add(entity);

    public override Task Update(OrderItem entity) => _database.Update(entity);

    public override Task Delete(OrderItem entity) => _database.Delete(entity);

    public async Task<IReadOnlyCollection<OrderItem>> GetByOrderId(Guid orderId)
    {
        return (await _database.GetAll<OrderItem>())
            .Where(i => i.OrderId == orderId)
            .ToList();
    }
}
