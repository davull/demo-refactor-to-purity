using Refactor.Application.Data;

namespace Refactor.Application.Repositories.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersByDate(DateTime startDate, DateTime endDate);
}
