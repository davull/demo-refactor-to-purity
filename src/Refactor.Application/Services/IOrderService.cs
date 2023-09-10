using Refactor.Application.Models;

namespace Refactor.Application.Services;

public interface IOrderService : IService
{
    Task<Order> GetOrder(Guid id);

    Task<IReadOnlyCollection<Order>> GetOrdersByDate(DateTime startDate, DateTime endDate);
}
