namespace Refactor.Application.Repositories;

public static class OrderItemRepository
{
    public static Task<OrderItemData> Get(Guid id, IDatabase db) => db.Get<OrderItemData>(id);

    public static Task<IEnumerable<OrderItemData>> GetAll(IDatabase db) => db.GetAll<OrderItemData>();

    public static Task Add(OrderItemData entity, IDatabase db) => db.Add(entity);

    public static Task Update(OrderItemData entity, IDatabase db) => db.Update(entity);

    public static Task Delete(OrderItemData entity, IDatabase db) => db.Delete(entity);

    public static async Task<IReadOnlyCollection<OrderItemData>> GetByOrderId(Guid orderId, IDatabase db)
    {
        return (await db.GetAll<OrderItemData>())
            .Where(i => i.OrderId == orderId)
            .ToList();
    }
}
