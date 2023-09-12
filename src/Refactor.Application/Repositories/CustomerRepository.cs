namespace Refactor.Application.Repositories;

public static class CustomerRepository
{
    public static async Task<IEnumerable<CustomerData>> GetAll(Func<Task<IEnumerable<CustomerData>>> getAll)
        => (await getAll()).Where(c => c.Active);

    public static Task Add(CustomerData entity, Func<CustomerData, Task> add) => add(entity);

    public static Task Update(CustomerData entity, Func<CustomerData, Task> update) => update(entity);

    public static Task Delete(CustomerData entity, Func<CustomerData, Task> delete) => delete(entity);

    public static Task<CustomerData> Get(Guid id, Func<Guid, Task<CustomerData>> get) => get(id);
}
