namespace Refactor.Application.Data;

public record Order(
    Guid Id,
    Guid CustomerId,
    DateTime OrderDate,
    decimal Total) : IData;
