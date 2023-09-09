using Refactor.Application.Data;
using Refactor.Application.Repositories.Interfaces;

namespace Refactor.Application.Repositories.Implementations;

public class OrderRepository : AbstractRepository<Order>, IOrderRepository

{
    public OrderRepository(IDatabase database) : base(database)
    {
    }

    public override Task<Order> Get(Guid id) => _database.Get<Order>(id);

    public override Task<IEnumerable<Order>> GetAll() => _database.GetAll<Order>();

    public override Task Add(Order entity) => _database.Add(entity);

    public override Task Update(Order entity) => _database.Update(entity);

    public override Task Delete(Order entity) => _database.Delete(entity);
}
