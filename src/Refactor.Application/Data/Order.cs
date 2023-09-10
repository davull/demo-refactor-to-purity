namespace Refactor.Application.Data;

public record Order(
    Guid Id,
    Guid CustomerId,
    DateTime OrderDate) : IData;
