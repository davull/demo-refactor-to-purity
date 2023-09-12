namespace Refactor.Application.Repositories;

public class OrderItemRepository
{
    private readonly IDatabase _database;

    public OrderItemRepository(IDatabase database)
    {
        _database = database;
    }

    public Task<OrderItemData> Get(Guid id) => _database.Get<OrderItemData>(id);

    public Task<IEnumerable<OrderItemData>> GetAll() => _database.GetAll<OrderItemData>();

    public Task Add(OrderItemData entity) => _database.Add(entity);

    public Task Update(OrderItemData entity) => _database.Update(entity);

    public Task Delete(OrderItemData entity) => _database.Delete(entity);

    public async Task<IReadOnlyCollection<OrderItemData>> GetByOrderId(Guid orderId)
    {
        return (await _database.GetAll<OrderItemData>())
            .Where(i => i.OrderId == orderId)
            .ToList();
    }
}
