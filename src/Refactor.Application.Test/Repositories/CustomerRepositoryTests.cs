using FluentAssertions;
using NSubstitute;
using Refactor.Application.Repositories;
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

        database.GetAll<CustomerData>()
            .Returns(allCustomers);

        // Act
        var actual = (await CustomerRepository.GetAll(database.GetAll<CustomerData>)).ToList();

        // Assert
        actual.Should().OnlyContain(c => c.Active);
        actual.Should().Contain(JohnDoe);
        actual.Should().NotContain(JaneDoe);
    }
}
