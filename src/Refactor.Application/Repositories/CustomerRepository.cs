namespace Refactor.Application.Repositories;

public static class CustomerRepository
{
    public static async Task<IEnumerable<CustomerData>> GetAll(IDatabase db)
        => (await db.GetAll<CustomerData>())
            .Where(c => c.Active);

    public static Task Add(CustomerData entity, IDatabase db) => db.Add(entity);

    public static Task Update(CustomerData entity, IDatabase db) => db.Update(entity);

    public static Task Delete(CustomerData entity, IDatabase db) => db.Delete(entity);

    public static Task<CustomerData> Get(Guid id, IDatabase db) => db.Get<CustomerData>(id);
}
