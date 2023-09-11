using FluentAssertions;
using Refactor.Application.Data;
using Refactor.Application.Repositories;
using static Refactor.Application.Test.DataDummies;

namespace Refactor.Application.Test.Repositories;

public class InMemoryDatabaseTests
{
    [Test]
    public async Task Should_Add_Entities()
    {
        var sut = new InMemoryDatabase();

        await sut.Add(JohnDoe);
        await sut.Add(JaneDoe);

        var actual = (await sut.GetAll<CustomerData>()).ToList();

        actual.Should().HaveCount(2);
        actual.Should().Contain(JohnDoe);
        actual.Should().Contain(JaneDoe);
    }

    [Test]
    public async Task Should_Remove_Entity()
    {
        var sut = new InMemoryDatabase();

        await sut.Add(JohnDoe);
        await sut.Add(JaneDoe);

        var actual = (await sut.GetAll<CustomerData>()).ToList();
        actual.Should().HaveCount(2);

        await sut.Delete(JohnDoe);

        actual = (await sut.GetAll<CustomerData>()).ToList();
        actual.Should().HaveCount(1);

        actual.Should().NotContain(JohnDoe);
    }
}
