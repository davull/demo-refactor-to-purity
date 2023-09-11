using Refactor.Application.Data;

namespace Refactor.Application.Repositories.Interfaces;

public interface IOrderRepository : IRepository<OrderData>
{
    Task<IEnumerable<OrderData>> GetOrdersByDate(DateTime startDate, DateTime endDate);
}
