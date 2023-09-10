namespace Refactor.Application.Models;

public record Order(
    Guid Id,
    Customer Customer,
    IReadOnlyCollection<OrderItem> Items,
    DateTime OrderDate) : ModelBase;
