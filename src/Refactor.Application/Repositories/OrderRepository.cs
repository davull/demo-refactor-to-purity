namespace Refactor.Application.Repositories;

public class OrderRepository
{
    private readonly IDatabase _database;

    public OrderRepository(IDatabase database)
    {
        _database = database;
    }

    public Task<OrderData> Get(Guid id) => _database.Get<OrderData>(id);

    public Task<IEnumerable<OrderData>> GetAll() => _database.GetAll<OrderData>();

    public Task Add(OrderData entity) => _database.Add(entity);

    public Task Update(OrderData entity) => _database.Update(entity);

    public Task Delete(OrderData entity) => _database.Delete(entity);

    public async Task<IEnumerable<OrderData>> GetOrdersByDate(DateTime startDate, DateTime endDate)
        => (await _database.GetAll<OrderData>())
            .Where(x => x.OrderDate >= startDate && x.OrderDate <= endDate);
}
