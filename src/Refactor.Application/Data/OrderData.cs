namespace Refactor.Application.Data;

public record OrderData(
    Guid Id,
    Guid CustomerId,
    DateTime OrderDate) : IData;
