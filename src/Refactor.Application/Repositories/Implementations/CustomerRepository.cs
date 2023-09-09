using Refactor.Application.Data;
using Refactor.Application.Repositories.Interfaces;

namespace Refactor.Application.Repositories.Implementations;

public class CustomerRepository : AbstractRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(IDatabase database) : base(database)
    {
    }

    public override async Task<IEnumerable<Customer>> GetAll()
        => (await _database.GetAll<Customer>())
            .Where(c => c.Active);

    public override Task Add(Customer entity) => _database.Add(entity);

    public override Task Update(Customer entity) => _database.Update(entity);

    public override Task Delete(Customer entity) => _database.Delete(entity);

    public override Task<Customer> Get(Guid id) => _database.Get<Customer>(id);
}
