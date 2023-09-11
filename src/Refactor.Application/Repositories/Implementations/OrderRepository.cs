using Refactor.Application.Data;
using Refactor.Application.Repositories.Interfaces;

namespace Refactor.Application.Repositories.Implementations;

public class OrderRepository : AbstractRepository<OrderData>, IOrderRepository

{
    public OrderRepository(IDatabase database) : base(database)
    {
    }

    public override Task<OrderData> Get(Guid id) => _database.Get<OrderData>(id);

    public override Task<IEnumerable<OrderData>> GetAll() => _database.GetAll<OrderData>();

    public override Task Add(OrderData entity) => _database.Add(entity);

    public override Task Update(OrderData entity) => _database.Update(entity);

    public override Task Delete(OrderData entity) => _database.Delete(entity);

    public async Task<IEnumerable<OrderData>> GetOrdersByDate(DateTime startDate, DateTime endDate)
        => (await _database.GetAll<OrderData>())
            .Where(x => x.OrderDate >= startDate && x.OrderDate <= endDate);
}
