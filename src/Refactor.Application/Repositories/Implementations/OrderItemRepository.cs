using Refactor.Application.Repositories.Interfaces;

namespace Refactor.Application.Repositories.Implementations;

public class OrderItemRepository : AbstractRepository<OrderItemData>, IOrderItemRepository
{
    public OrderItemRepository(IDatabase database) : base(database)
    {
    }

    public override Task<OrderItemData> Get(Guid id) => _database.Get<OrderItemData>(id);

    public override Task<IEnumerable<OrderItemData>> GetAll() => _database.GetAll<OrderItemData>();

    public override Task Add(OrderItemData entity) => _database.Add(entity);

    public override Task Update(OrderItemData entity) => _database.Update(entity);

    public override Task Delete(OrderItemData entity) => _database.Delete(entity);

    public async Task<IReadOnlyCollection<OrderItemData>> GetByOrderId(Guid orderId)
    {
        return (await _database.GetAll<OrderItemData>())
            .Where(i => i.OrderId == orderId)
            .ToList();
    }
}
