namespace Refactor.Application.Logic;

public record Customer(
    Guid Id,
    string FirstName,
    string LastName,
    string Email);

public record Order(
    Guid Id,
    Customer Customer,
    IReadOnlyCollection<OrderItem> Items,
    DateTime OrderDate);

public record OrderItem(
    Guid Id,
    Guid ProductId,
    int Quantity,
    decimal GrossPrice,
    decimal NetPrice,
    decimal TaxRate,
    decimal TaxAmount,
    decimal TotalGrossPrice,
    decimal TotalNetPrice);
