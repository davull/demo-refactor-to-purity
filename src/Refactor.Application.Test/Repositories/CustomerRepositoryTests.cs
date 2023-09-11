using FluentAssertions;
using NSubstitute;
using Refactor.Application.Data;
using Refactor.Application.Repositories;
using Refactor.Application.Repositories.Implementations;
using static Refactor.Application.Test.DataDummies;

namespace Refactor.Application.Test.Repositories;

public class CustomerRepositoryTests
{
    [Test]
    public async Task Should_Only_Return_Active_Customers()
    {
        // Arrange
        var allCustomers = Many(JohnDoe, JaneDoe);
        var database = Substitute.For<IDatabase>();

        database.GetAll<Customer>()
            .Returns(allCustomers);

        var sut = new CustomerRepository(database);

        // Act
        var actual = (await sut.GetAll()).ToList();

        // Assert
        actual.Should().OnlyContain(c => c.Active);
        actual.Should().Contain(JohnDoe);
        actual.Should().NotContain(JaneDoe);
    }
}
