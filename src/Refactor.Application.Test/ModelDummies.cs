using Refactor.Application.Data;
using Refactor.Application.Models;

namespace Refactor.Application.Test;

internal static class ModelDummies
{
    public static Customer JohnDoe => FromData(DataDummies.JohnDoe);

    public static Customer JaneDoe => FromData(DataDummies.JaneDoe);

    public static Customer PeterPan => FromData(DataDummies.PeterPan);

    public static Customer ANomymous => FromData(DataDummies.ANomymous);

    public static Customer Customer(Guid? id = null, string firstName = "Peter", string lastName = "Parker",
        string email = "peter.parker@example.com") => new(id ?? Guid.NewGuid(), firstName, lastName, email);

    public static Customer FromData(CustomerData data)
    {
        return Customer(
            id: data.Id,
            firstName: data.FirstName,
            lastName: data.LastName,
            email: data.Email);
    }


    public static Order Order(Guid? id = null, Customer? customer = null,
        IReadOnlyCollection<OrderItem>? items = null, DateTime? orderDate = null)
    {
        return new Order(
            Id: id ?? Guid.NewGuid(),
            Customer: customer ?? Customer(),
            Items: items ?? Collection(OrderItem(), OrderItem()),
            OrderDate: orderDate ?? DateTime.UtcNow);
    }

    public static OrderItem OrderItem(Guid? id = null, Guid? orderId = null,
        Guid? productId = null, int quantity = 1, decimal netPrice = 9.99m, decimal taxRate = 19m)
    {
        var grossPrice = netPrice * (1 + taxRate / 100m);

        return new OrderItem(
            Id: id ?? Guid.NewGuid(),
            ProductId: productId ?? Guid.NewGuid(),
            Quantity: quantity,
            GrossPrice: grossPrice,
            NetPrice: netPrice,
            TaxRate: taxRate,
            TaxAmount: grossPrice - netPrice,
            TotalGrossPrice: grossPrice * quantity,
            TotalNetPrice: netPrice * quantity);
    }

    public static T[] Many<T>(params T[] items) => items.ToArray();

    public static IReadOnlyCollection<T> Collection<T>(params T[] items) => items.ToList();
}
