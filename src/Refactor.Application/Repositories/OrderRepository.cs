namespace Refactor.Application.Repositories;

public static class OrderRepository
{
    public static Task<OrderData> Get(Guid id, Func<Guid, Task<OrderData>> get) => get(id);

    public static Task<IEnumerable<OrderData>> GetAll(Func<Task<IEnumerable<OrderData>>> getAll) => getAll();

    public static Task Add(OrderData entity, Func<OrderData, Task> add) => add(entity);

    public static Task Update(OrderData entity, Func<OrderData, Task> update) => update(entity);

    public static Task Delete(OrderData entity, Func<OrderData, Task> delete) => delete(entity);

    public static async Task<IEnumerable<OrderData>> GetOrdersByDate(
        DateTime startDate, DateTime endDate, Func<Task<IEnumerable<OrderData>>> getAll)
        => (await getAll()).Where(x => x.OrderDate >= startDate && x.OrderDate <= endDate);
}
