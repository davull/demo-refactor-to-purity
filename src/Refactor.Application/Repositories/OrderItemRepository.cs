namespace Refactor.Application.Repositories;

public static class OrderItemRepository
{
    public delegate Task AddDelegate(OrderItemData entity);

    public delegate Task DeleteDelegate(OrderItemData entity);

    public delegate Task<IEnumerable<OrderItemData>> GetAllDelegate();

    public delegate Task<OrderItemData> GetDelegate(Guid id);

    public delegate Task UpdateDelegate(OrderItemData entity);

    public static Task<OrderItemData> Get(Guid id, GetDelegate get) => get(id);

    public static Task<IEnumerable<OrderItemData>> GetAll(GetAllDelegate getAll) => getAll();

    public static Task Add(OrderItemData entity, AddDelegate add) => add(entity);

    public static Task Update(OrderItemData entity, UpdateDelegate update) => update(entity);

    public static Task Delete(OrderItemData entity, DeleteDelegate delete) => delete(entity);

    public static async Task<IReadOnlyCollection<OrderItemData>> GetByOrderId(Guid orderId, GetAllDelegate getAll)
        => (await getAll())
            .Where(i => i.OrderId == orderId)
            .ToList();
}
