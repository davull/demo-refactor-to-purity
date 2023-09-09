namespace Refactor.Application.Data;

public record Customer(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    bool Active) : IData;
