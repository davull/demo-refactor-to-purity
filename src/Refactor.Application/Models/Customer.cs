namespace Refactor.Application.Models;

public record Customer(
    Guid Id,
    string FirstName,
    string LastName,
    string Email) : ModelBase;
