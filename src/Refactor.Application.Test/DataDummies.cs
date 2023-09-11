﻿using Refactor.Application.Data;

namespace Refactor.Application.Test;

internal static class DataDummies
{
    public static Customer JohnDoe => Customer(
        new Guid("bfbffb19-cdd4-42ac-b536-606a16d03eae"), "John", "Doe", "john.doe@example.com");

    public static Customer JaneDoe => Customer(
        new Guid("95a6db4a-4635-4fb3-b7f6-c206ff7272f1"), "Jane", "Doe", "Jane.doe@example.com", false);

    public static Customer PeterPan => Customer(
        new Guid("2f6c199e-be0f-4515-9e42-3e3f88c0523a"), "Peter", "Pan", "peter.pan@example.com");

    public static Customer ANomymous => Customer(
        new Guid("2f6c199e-be0f-4515-9e42-3e3f88c0523a"), "A.", "Nomymous", "none");

    public static Customer Customer(
        Guid? id = null, string firstName = "Peter", string lastName = "Parker",
        string email = "peter.parker@example.com", bool active = true)
    {
        return new Customer(id ?? Guid.NewGuid(),
            firstName, lastName, email, active);
    }

    public static Order Order(Guid? id = null, Guid? customerId = null, DateTime? orderDate = null) =>
        new(id ?? Guid.NewGuid(), customerId ?? Guid.NewGuid(), orderDate ?? DateTime.UtcNow);

    public static OrderItem OrderItem(Guid? id = null, Guid? orderId = null,
        Guid? productId = null, int quantity = 1, decimal price = 9.99m)
    {
        return new OrderItem(
            Id: id ?? Guid.NewGuid(),
            OrderId: orderId ?? Guid.NewGuid(),
            ProductId: productId ?? Guid.NewGuid(),
            Quantity: quantity,
            Price: price);
    }


    public static T[] Many<T>(params T[] items) => items.ToArray();

    public static IReadOnlyCollection<T> Collection<T>(params T[] items) => items.ToList();
}
