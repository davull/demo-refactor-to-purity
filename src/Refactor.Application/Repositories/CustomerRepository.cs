namespace Refactor.Application.Repositories;

public class CustomerRepository
{
    private readonly IDatabase _database;

    public CustomerRepository(IDatabase database)
    {
        _database = database;
    }

    public async Task<IEnumerable<CustomerData>> GetAll()
        => (await _database.GetAll<CustomerData>())
            .Where(c => c.Active);

    public Task Add(CustomerData entity) => _database.Add(entity);

    public Task Update(CustomerData entity) => _database.Update(entity);

    public Task Delete(CustomerData entity) => _database.Delete(entity);

    public Task<CustomerData> Get(Guid id) => _database.Get<CustomerData>(id);
}
