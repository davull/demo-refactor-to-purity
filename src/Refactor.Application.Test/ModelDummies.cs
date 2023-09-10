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

    public static Customer FromData(Data.Customer data)
    {
        return Customer(
            id: data.Id,
            firstName: data.FirstName,
            lastName: data.LastName,
            email: data.Email);
    }
}
