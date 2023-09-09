using FluentAssertions;
using Refactor.Application.Data;
using Refactor.Application.Repositories;

namespace Refactor.Application.Test.Repositories;

public class InMemoryDatabaseTests
{
    [Test]
    public async Task Should_Add_Entities()
    {
        var sut = new InMemoryDatabase();

        var customer1 = new Customer(Guid.NewGuid(), "John", "Doe", "john.doe@example.com", true);
        var customer2 = new Customer(Guid.NewGuid(), "Jane", "Doe", "jane.doe@example.com", false);

        await sut.Add(customer1);
        await sut.Add(customer2);

        var actual = (await sut.GetAll<Customer>()).ToList();

        actual.Should().HaveCount(2);
        actual.Should().Contain(customer1);
        actual.Should().Contain(customer2);
    }

    [Test]
    public async Task Should_Remove_Entity()
    {
        var sut = new InMemoryDatabase();

        var customer1 = new Customer(Guid.NewGuid(), "John", "Doe", "john.doe@example.com", true);
        var customer2 = new Customer(Guid.NewGuid(), "Jane", "Doe", "jane.doe@example.com", false);

        await sut.Add(customer1);
        await sut.Add(customer2);

        var actual = (await sut.GetAll<Customer>()).ToList();
        actual.Should().HaveCount(2);

        await sut.Delete(customer1);

        actual = (await sut.GetAll<Customer>()).ToList();
        actual.Should().HaveCount(1);

        actual.Should().NotContain(customer1);
    }
}
