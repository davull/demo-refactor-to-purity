namespace Refactor.Application.Models;

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
