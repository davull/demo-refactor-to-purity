namespace Refactor.Application;

public record OrderItemData(
    Guid Id,
    Guid OrderId,
    Guid ProductId,
    int Quantity,
    decimal Price) : IData;

public record OrderData(
    Guid Id,
    Guid CustomerId,
    DateTime OrderDate) : IData;

public record CustomerData(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    bool Active) : IData;

public interface IData
{
    Guid Id { get; }
}
