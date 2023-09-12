namespace Refactor.Application.Repositories;

public static class OrderRepository
{
    public static Task<OrderData> Get(Guid id, IDatabase db) => db.Get<OrderData>(id);

    public static Task<IEnumerable<OrderData>> GetAll(IDatabase db) => db.GetAll<OrderData>();

    public static Task Add(OrderData entity, IDatabase db) => db.Add(entity);

    public static Task Update(OrderData entity, IDatabase db) => db.Update(entity);

    public static Task Delete(OrderData entity, IDatabase db) => db.Delete(entity);

    public static async Task<IEnumerable<OrderData>> GetOrdersByDate(DateTime startDate, DateTime endDate, IDatabase db)
        => (await db.GetAll<OrderData>())
            .Where(x => x.OrderDate >= startDate && x.OrderDate <= endDate);
}
