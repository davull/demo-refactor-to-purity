namespace Refactor.Application.Data;

public record OrderItem(
    Guid Id,
    Guid OrderId,
    Guid ProductId,
    int Quantity,
    decimal Price) : IData;
