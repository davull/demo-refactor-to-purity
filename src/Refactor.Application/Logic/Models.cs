namespace Refactor.Application.Logic;

public record Customer(
    Guid Id,
    string FirstName,
    string LastName,
    string Email) : ModelBase;

public abstract record ModelBase;

public record Order(
    Guid Id,
    Customer Customer,
    IReadOnlyCollection<OrderItem> Items,
    DateTime OrderDate) : ModelBase;

public record OrderItem(
    Guid Id,
    Guid ProductId,
    int Quantity,
    decimal GrossPrice,
    decimal NetPrice,
    decimal TaxRate,
    decimal TaxAmount,
    decimal TotalGrossPrice,
    decimal TotalNetPrice) : ModelBase;
