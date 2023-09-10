using System.Net;
using FluentAssertions;
using FluentAssertions.Execution;
using Refactor.Application.Models;
using Snapshooter.NUnit;

namespace Refactor.Application.Test.Controllers;

public class OrdersControllerTests : IntegrationTestBase
{
    [TestCase("/orders")]
    [TestCase("/orders?startDate=2021-01-01")]
    [TestCase("/orders?endDate=2021-01-01")]
    [TestCase("/orders?startDate=2021-01-01&endDate=2021-01-31")]
    public async Task Should_Return_Ok(string path)
    {
        var response = await GetAsync(path);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task Should_Match_Snapshot()
    {
        var content = await GetStringAsync("/orders");
        content.Should().MatchSnapshot();
    }

    [Test]
    public async Task Should_Return_Orders()
    {
        var orders = await GetAsync<Order[]>("/orders?startDate=2021-01-01&endDate=2021-01-20");

        using var _ = new AssertionScope();

        orders.Should().HaveCount(2);

        orders.Should().OnlyContain(o => o.OrderDate >= new DateTime(2021, 1, 1) &&
                                         o.OrderDate <= new DateTime(2021, 1, 20));
    }
}
