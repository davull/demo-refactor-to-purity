using Refactor.Application.Data;
using Refactor.Application.Repositories.Interfaces;

namespace Refactor.Application.Repositories.Implementations;

public class CustomerRepository : AbstractRepository<CustomerData>, ICustomerRepository
{
    public CustomerRepository(IDatabase database) : base(database)
    {
    }

    public override async Task<IEnumerable<CustomerData>> GetAll()
        => (await _database.GetAll<CustomerData>())
            .Where(c => c.Active);

    public override Task Add(CustomerData entity) => _database.Add(entity);

    public override Task Update(CustomerData entity) => _database.Update(entity);

    public override Task Delete(CustomerData entity) => _database.Delete(entity);

    public override Task<CustomerData> Get(Guid id) => _database.Get<CustomerData>(id);
}
