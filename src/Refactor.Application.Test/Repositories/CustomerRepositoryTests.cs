using FluentAssertions;
using NSubstitute;
using Refactor.Application.Data;
using Refactor.Application.Repositories;

namespace Refactor.Application.Test.Repositories;

public class CustomerRepositoryTests
{
    [Test]
    public async Task Should_Only_Return_Active_Customers()
    {
        // Arrange
        var customer1 = new Customer(Guid.NewGuid(), "John", "Doe", "john.doe@example.com", true);
        var customer2 = new Customer(Guid.NewGuid(), "Jane", "Doe", "jane.doe@example.com", false);
        var allCustomers = new[] { customer1, customer2 };
        var database = Substitute.For<IDatabase>();

        database.GetAll<Customer>()
            .Returns(allCustomers);

        var sut = new CustomerRepository(database);

        // Act
        var actual = (await sut.GetAll()).ToList();

        // Assert
        actual.Should().OnlyContain(c => c.Active);
        actual.Should().Contain(customer1);
        actual.Should().NotContain(customer2);
    }
}
