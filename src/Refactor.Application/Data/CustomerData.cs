namespace Refactor.Application.Data;

public record CustomerData(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    bool Active) : IData;
